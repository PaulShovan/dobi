using Dhobi.Common;
using Dhobi.Core.Notification.DbModels;
using Dhobi.Repository.Implementation.Base;
using Dhobi.Repository.Interface;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhobi.Repository.Implementation
{
    public class NotificationRepository : Repository<Notification>, INotificationRepository
    {
        public async Task<bool> AddNotification(Notification notification)
        {
            try
            {
                await Collection.InsertOneAsync(notification);
                return true;
            }
            catch (Exception ex)
            {

                throw new Exception("Error adding notification.");
            }
        }

        public async Task<List<Notification>> GetNotifications()
        {
            var sortBuilder = Builders<Notification>.Sort;
            var sortOrder = sortBuilder.Descending(s => s.Time);
            var projection = Builders<Notification>.Projection.Exclude("_id");
            var notifications = await Collection.Find(d => d.Status == (int)NotificationStatus.NotSent).Project<Notification>(projection).Sort(sortOrder).ToListAsync();
            if (notifications == null)
            {
                return null;
            }
            return notifications;
        }

        public async Task<bool> UpdateNotificationStatus(string notificationId, int status)
        {
            try
            {
                var filter = Builders<Notification>.Filter.Eq(d => d.NotificationId, notificationId);
                var update = Builders<Notification>.Update.Set(u => u.Status, status);
                var projection = Builders<Notification>.Projection.Exclude("_id");
                var options = new FindOneAndUpdateOptions<Notification, Notification>();
                options.ReturnDocument = ReturnDocument.After;
                options.Projection = projection;
                var result = await Collection.FindOneAndUpdateAsync(filter, update, options);
                if (result == null)
                {
                    return false;
                }
                return !string.IsNullOrWhiteSpace(result.NotificationId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error confirming order");
            }
        }
    }
}
