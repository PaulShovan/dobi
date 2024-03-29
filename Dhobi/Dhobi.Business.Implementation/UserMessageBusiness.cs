﻿using Dhobi.Business.Interface;
using Dhobi.Common;
using Dhobi.Core.UserInbox.DbModels;
using Dhobi.Core.UserInbox.ViewModels;
using Dhobi.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhobi.Business.Implementation
{
    public class UserMessageBusiness : IUserMessageBusiness
    {
        private IUserMessageRepository _userMessageRepository;
        private IOrderRepository _orderRepository;
        public UserMessageBusiness(IUserMessageRepository userMessageRepository, IOrderRepository orderRepository)
        {
            _userMessageRepository = userMessageRepository;
            _orderRepository = orderRepository;
        }
        private UserMessage PrepareNewOrderMessage(string userId, string serviceId)
        {
            return new UserMessage
            {
                MessageId = Guid.NewGuid().ToString(),
                UserId = userId,
                ServiceId = serviceId,
                Title = Constants.NEW_ORDER_MESSAGE_TITLE,
                Message = Constants.NEW_ORDER_MESSAGE_TEXT,
                Time = Utilities.GetPresentDateTime(),
                Status = (int)MessageStatus.Unread,
                IsDelivered = (int)MessageDeliveryStatus.NotDelivered,
                Type = (int)MessageType.NewOrder
            };
        }
        private UserMessage PrepareAcknowledgeOrderMessage(string userId, string serviceId)
        {
            return new UserMessage
            {
                MessageId = Guid.NewGuid().ToString(),
                UserId = userId,
                ServiceId = serviceId,
                Title = Constants.ACK_MESSAGE_TITLE,
                Message = string.Format(Constants.ACK_MESSAGE_TEXT, serviceId),
                Time = Utilities.GetPresentDateTime(),
                Status = (int)MessageStatus.Unread,
                IsDelivered = (int)MessageDeliveryStatus.NotDelivered,
                Type = (int)MessageType.OrderAcknowledge
            };
        }
        private UserMessage PrepareConfirmOrderMessage(string userId, string serviceId)
        {
            return new UserMessage
            {
                MessageId = Guid.NewGuid().ToString(),
                UserId = userId,
                ServiceId = serviceId,
                Title = Constants.CONFIRM_ORDER_MESSAGE_TITLE,
                Message = Constants.CONFIRM_ORDER_MESSAGE_TEXT,
                Time = Utilities.GetPresentDateTime(),
                Status = (int)MessageStatus.Unread,
                IsDelivered = (int)MessageDeliveryStatus.NotDelivered,
                Type = (int)MessageType.ConfirmOrder
            };
        }
        private UserMessage PrepareConfirmDobiOrderMessage(string userId, string serviceId, string customerName)
        {
            return new UserMessage
            {
                MessageId = Guid.NewGuid().ToString(),
                UserId = userId,
                ServiceId = serviceId,
                Title = string.Format(Constants.CONFIRM_ORDER_DOBI_MESSAGE_TITLE, customerName),
                Message = string.Format(Constants.CONFIRM_ORDER_DOBI_MESSAGE_TEXT, serviceId),
                Time = Utilities.GetPresentDateTime(),
                Status = (int)MessageStatus.Unread,
                IsDelivered = (int)MessageDeliveryStatus.NotDelivered,
                Type = (int)MessageType.ConfirmOrderDobi
            };
        }
        public async Task<string> AddUserMessage(string userId, int messageType, string serviceId, string username = "")
        {
            try
            {
                UserMessage message =  null;
                if(messageType == (int)MessageType.NewOrder)
                {
                    message = PrepareNewOrderMessage(userId, serviceId);
                }
                else if (messageType == (int)MessageType.OrderAcknowledge)
                {
                    message = PrepareAcknowledgeOrderMessage(userId, serviceId);
                }
                else if(messageType == (int)MessageType.ConfirmOrder)
                {
                    message = PrepareConfirmOrderMessage(userId, serviceId);
                }
                else if (messageType == (int)MessageType.ConfirmOrderDobi)
                {
                    message = PrepareConfirmDobiOrderMessage(userId, serviceId, username);
                }
                if (message == null)
                {
                    return null;
                }
                var ack = await _userMessageRepository.AddUserMessage(message);
                if (!ack)
                {
                    return null;
                }
                return message.MessageId;
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding user message" + ex);
            }
        }

        public async Task<UserMessageListViewModel> GetUserMessage(string userId, int skip, int limit)
        {
            try
            {
                var userMessage = new List<UserMessageBasicInformation>();
                var messages = await _userMessageRepository.GetUserMessage(userId, skip, limit);
                var count = await _userMessageRepository.GetUserMessageCount(userId);
                if(messages == null || count == 0)
                {
                    return new UserMessageListViewModel
                    {
                        PageCount = 0,
                        Messages = new List<UserMessageBasicInformation>()
                    };
                }
                messages.ForEach(m => userMessage.Add(new UserMessageBasicInformation
                {
                    Message = m.Message,
                    MessageId = m.MessageId,
                    Title = m.Title,
                    Time = Utilities.GetFormattedDateFromMillisecond(m.Time),
                    Status = m.Status,
                    MessageType = m.Type,
                    ServiceId = m.ServiceId
                }));
                return new UserMessageListViewModel {
                    PageCount = (int)Math.Ceiling((double)count / limit),
                    Messages = userMessage
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting user messages" + ex);
            }
        }

        public async Task<UserMessageBasicInformation> GetMessageById(string messageId)
        {
            try
            {
                var message = await _userMessageRepository.GetMessageById(messageId);
                if (message == null)
                {
                    return null;
                }
                return new UserMessageBasicInformation
                {
                    MessageId = message.MessageId,
                    Message = message.Message,
                    Title = message.Title,
                    Time = Utilities.GetFormattedDateFromMillisecond(message.Time),
                    Status = message.Status
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting user messages" + ex);
            }
        }

        public async Task<UserAcknowledgeMessageViewModel> GetOrderAcknowledge(string messageId, string serviceId)
        {
            try
            {
                var order = await _orderRepository.GetOrderAcknowledgeInformation(serviceId);
                if(order == null || order.Dobi == null)
                {
                    return null;
                }
                var message = await GetMessageById(messageId);
                if(message == null)
                {
                    return null;
                }
                return new UserAcknowledgeMessageViewModel
                {
                    ServiceId = order.ServiceId,
                    DobiId = order.Dobi.DobiId,
                    DobiName = order.Dobi.Name,
                    DobiPhoto = order.Dobi.Photo,
                    Time = Utilities.GetFormattedDateFromMillisecond(order.PickUpDate) + " "+ order.PickUpTime
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting ack");
            }
        }
    }
}
