using Dhobi.Common;
using Dhobi.Core.Notification.DbModels;
using Dhobi.Core.Notification.ViewModels;
using Dhobi.Repository.Interface;
using Dhobi.Service.Interface;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PushSharp.Apple;
using PushSharp.Core;
using PushSharp.Google;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhobi.Service.Implementation
{
    public class NotificationService : INotificationService
    {
        private INotificationRepository _notificationRepository;
        private IDeviceStausRepository _deviceStausRepository;
        private GcmServiceBroker _gcmBroker;
        private ApnsServiceBroker _apnsBroker;
        public NotificationService(INotificationRepository notificationRepository, IDeviceStausRepository deviceStatusRepository)
        {
            _notificationRepository = notificationRepository;
            _deviceStausRepository = deviceStatusRepository;
            //ConfigureGcmBroker();
            //ConfigureApnsBroker();
        }
        private void ConfigureGcmBroker()
        {
            var config = new GcmConfiguration("AAAALXxcjZI:APA91bHQkzx9W8-zk_Vgmn-N6SaE96HlmYBsaGi4UBxymlnLnbqwM5TwbdnbBpp9aIOs6Fzfjf9-oz18IWe0gS46L3oWX2A4IweuhO1wnLlGLS0domuKJov7nfZifmT_OCZJo7Ir3mif");
            config.GcmUrl = "https://fcm.googleapis.com/fcm/send";
            _gcmBroker = new GcmServiceBroker(config);
            _gcmBroker.OnNotificationFailed += NotificationFailed;
            _gcmBroker.OnNotificationSucceeded += NotificationSent;
        }
        private void ConfigureApnsBroker()
        {
            var config = new ApnsConfiguration(ApnsConfiguration.ApnsServerEnvironment.Sandbox,"push-cert.p12", "push-cert-pwd");
            _apnsBroker = new ApnsServiceBroker(config);
            _apnsBroker.OnNotificationFailed += NotificationFailedApns;
            _apnsBroker.OnNotificationSucceeded += NotificationSent;
        }
        private bool SendAndroidNotification(List<string> devices, string payload)
        {
            try
            {
                ConfigureGcmBroker();
                _gcmBroker.Start();
                _gcmBroker.QueueNotification(new GcmNotification
                {
                    //RegistrationIds = new List<string> {
                    //    "fWeAbdnDzxw:APA91bHGQjLneuTiXRPQZiQXDpBlCn7yegWn2AYfBO7jUdy3PqVM48GgEDTKm4-aa5Lq07sljW_oziOHbUiJJofBswV_rbPtqXiYdKPEY3Fn0g1haQpQvrD9qIdK3H9_1NZkpYw8iyWM"
                    //},
                    //Data = JObject.Parse("{ \"title\" : \"title\", \"message\" : \"message\"}")
                    RegistrationIds = devices,
                    Data = JObject.Parse(payload)
                });

                //_gcmBroker.Stop();
                return true;
            }
            catch (Exception ex)
            {
                //throw new Exception("Error sending notification.");
                return false;
            }
            
        }
        private bool SendIosNotification(List<string> devices, string payload)
        {
            try
            {
                _apnsBroker.Start();

                foreach (var deviceToken in devices)
                {
                    _apnsBroker.QueueNotification(new ApnsNotification
                    {
                        DeviceToken = deviceToken,
                        Payload = JObject.Parse(payload)
                    });
                }

                _apnsBroker.Stop();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error sending notification.");
            }

        }
        private async Task<bool> SendNotificationToDevice(Notification notification)
        {
            try
            {
                var ack = false;
                var updated = false;
                var devices = await _deviceStausRepository.GetDeviceStatus(notification.ReceiverUserId);
                if (devices == null || devices.Count < 1)
                {
                    return false;
                }
                var androidDevices = devices.FindAll(d => d.DeviceOs == (int)DeviceOs.Android).Select(s => s.RegistrationId).ToList();
                var iosDevices = devices.FindAll(d => d.DeviceOs == (int)DeviceOs.Ios).Select(s => s.RegistrationId).ToList();
                var payloadToSend = JsonConvert.SerializeObject(new NotificationBasicInformation {
                    Type = notification.Type,
                    message = notification.Text,
                    title = notification.Title,
                    MessageId = notification.MessageId
                });
                if(androidDevices != null && androidDevices.Count > 0)
                {
                    ack = SendAndroidNotification(androidDevices, payloadToSend);
                }
                if (iosDevices != null && iosDevices.Count > 0)
                {
                    ack = SendIosNotification(iosDevices, payloadToSend);
                }
                if (ack)
                {
                    updated = await _notificationRepository.UpdateNotificationStatus(notification.NotificationId, (int)NotificationStatus.Sent);
                }
                return updated;
            }
            catch (Exception ex)
            {
                throw new Exception("Error sending notification."+ex);
            }
        }
        public async Task<bool> SendNotification()
        {
            try
            {
                var notifications = await _notificationRepository.GetNotifications();
                if (notifications == null || notifications.Count < 1)
                {
                    return false;
                }
                foreach (var notification in notifications)
                {
                    var sendAck = await SendNotificationToDevice(notification);
                    if (sendAck)
                    {
                        _gcmBroker.Stop();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error sending notification." + ex);
            }
        }
        static void NotificationFailed(INotification notification, AggregateException notificationFailureException)
        {
            notificationFailureException.Handle(ex =>
            {

                // See what kind of exception it was to further diagnose
                if (ex is GcmNotificationException)
                {
                    var notificationException = (GcmNotificationException)ex;

                    // Deal with the failed notification
                    var gcmNotification = notificationException.Notification;
                    var description = notificationException.Description;

                    Console.WriteLine($"GCM Notification Failed: ID={gcmNotification.MessageId}, Desc={description}");
                }
                else if (ex is GcmMulticastResultException)
                {
                    var multicastException = (GcmMulticastResultException)ex;

                    foreach (var succeededNotification in multicastException.Succeeded)
                    {
                        Console.WriteLine($"GCM Notification Succeeded: ID={succeededNotification.MessageId}");
                    }

                    foreach (var failedKvp in multicastException.Failed)
                    {
                        var n = failedKvp.Key;
                        var e = failedKvp.Value;

                        Console.WriteLine($"GCM Notification Failed: ID={n.MessageId}, Desc={e.Message}");
                    }

                }
                else if (ex is DeviceSubscriptionExpiredException)
                {
                    var expiredException = (DeviceSubscriptionExpiredException)ex;

                    var oldId = expiredException.OldSubscriptionId;
                    var newId = expiredException.NewSubscriptionId;

                    Console.WriteLine($"Device RegistrationId Expired: {oldId}");

                    if (!string.IsNullOrWhiteSpace(newId))
                    {
                        // If this value isn't null, our subscription changed and we should update our database
                        Console.WriteLine($"Device RegistrationId Changed To: {newId}");
                    }
                }
                else if (ex is RetryAfterException)
                {
                    var retryException = (RetryAfterException)ex;
                    // If you get rate limited, you should stop sending messages until after the RetryAfterUtc date
                    Console.WriteLine($"GCM Rate Limited, don't send more until after {retryException.RetryAfterUtc}");
                }
                else
                {
                    Console.WriteLine("GCM Notification Failed for some unknown reason");
                }

                // Mark it as handled
                return true;
            });
        }
        static void NotificationFailedApns(INotification notification, AggregateException notificationFailureException)
        {
            notificationFailureException.Handle(ex => {

                // See what kind of exception it was to further diagnose
                if (ex is ApnsNotificationException)
                {
                    var notificationException = (ApnsNotificationException)ex;

                    // Deal with the failed notification
                    var apnsNotification = notificationException.Notification;
                    var statusCode = notificationException.ErrorStatusCode;

                    Console.WriteLine($"Apple Notification Failed: ID={apnsNotification.Identifier}, Code={statusCode}");

                }
                else
                {
                    // Inner exception might hold more useful information like an ApnsConnectionException			
                    Console.WriteLine($"Apple Notification Failed for some unknown reason : {ex.InnerException}");
                }

                // Mark it as handled
                return true;
            });
        }
        static void NotificationSent(INotification notification)
        {
            Console.WriteLine("Sent: -> " + notification);
        }
    }
}
