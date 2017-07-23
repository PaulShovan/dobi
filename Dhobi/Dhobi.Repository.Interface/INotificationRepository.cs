using Dhobi.Core.Notification.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhobi.Repository.Interface
{
    public interface INotificationRepository
    {
        Task<bool> AddNotification(Notification notification);
        Task<List<Notification>> GetNotifications();
        Task<bool> UpdateNotificationStatus(string notificationId, int status);
    }
}
