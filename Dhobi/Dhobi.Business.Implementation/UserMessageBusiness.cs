using Dhobi.Business.Interface;
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

        public async Task<UserMessageListViewModel> GetUserMessage(string userId, int skip, int limit)
        {
            try
            {
                var userMessage = new List<UserMessageBasicInformation>();
                var messages = await _userMessageRepository.GetUserMessage(userId, skip, limit);
                var count = await _userMessageRepository.GetUserMessageCount(userId);
                if(messages == null || count <= 0 || limit <= 0)
                {
                    return null;
                }
                messages.ForEach(m => userMessage.Add(new UserMessageBasicInformation
                {
                    Message = m.Message,
                    MessageId = m.MessageId,
                    Title = m.Title,
                    Time = Utilities.GetFormattedDateFromMillisecond(m.Time),
                    Status = m.Status
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
    }
}
