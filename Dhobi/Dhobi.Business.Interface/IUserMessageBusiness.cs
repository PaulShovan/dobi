using Dhobi.Core.UserInbox.DbModels;
using Dhobi.Core.UserInbox.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhobi.Business.Interface
{
    public interface IUserMessageBusiness
    {
        Task<string> AddUserMessage(string userId, int messageType, string serviceId, string username = "");
        Task<UserMessageListViewModel> GetUserMessage(string userId, int skip, int limit);
        Task<UserMessageBasicInformation> GetMessageById(string messageId);
        Task<UserAcknowledgeMessageViewModel> GetOrderAcknowledge(string messageId, string serviceId);
    }
}
