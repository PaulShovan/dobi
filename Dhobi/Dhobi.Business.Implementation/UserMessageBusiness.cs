using Dhobi.Business.Interface;
using Dhobi.Common;
using Dhobi.Core.UserInbox.DbModels;
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
        public UserMessageBusiness(IUserMessageRepository userMessageRepository)
        {
            _userMessageRepository = userMessageRepository;
        }
        private UserMessage PrepareNewOrderMessage(string userId)
        {
            return new UserMessage
            {
                MessageId = Guid.NewGuid().ToString(),
                UserId = userId,
                Title = Constants.NEW_ORDER_MESSAGE_TITLE,
                Message = Constants.NEW_ORDER_MESSAGE_TEXT,
                Time = Utilities.GetPresentDateTime(),
                Status = (int)MessageStatus.Unread,
                IsDelivered = (int)MessageDeliveryStatus.NotDelivered,
                Type = (int)MessageType.NewOrder
            };
        }
        public async Task<bool> AddUserMessage(string userId, int messageType)
        {
            try
            {
                UserMessage message =  null;
                if(messageType == (int)MessageType.NewOrder)
                {
                    message = PrepareNewOrderMessage(userId);
                }
                if(message == null)
                {
                    return false;
                }
                return await _userMessageRepository.AddUserMessage(message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding user message" + ex);
            }
        }
    }
}
