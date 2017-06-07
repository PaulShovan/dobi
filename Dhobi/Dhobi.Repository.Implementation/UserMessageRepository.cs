using Dhobi.Common;
using Dhobi.Core.UserInbox.DbModels;
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
    public class UserMessageRepository : Repository<UserMessage>, IUserMessageRepository
    {
        public async Task<bool> AddUserMessage(UserMessage message)
        {
            await Collection.InsertOneAsync(message);
            return true;
        }

        public async Task<UserMessage> GetMessageById(string messageId)
        {
            var filter1 = Builders<UserMessage>.Filter.Eq(d => d.MessageId, messageId);
            var update = Builders<UserMessage>.Update.Set(u => u.Status, (int)MessageStatus.Read);
            var projection = Builders<UserMessage>.Projection.Exclude("_id").Exclude(s => s.UserId).Exclude(s => s.IsDelivered).Exclude(s => s.Type);
            var options = new FindOneAndUpdateOptions<UserMessage, UserMessage>();
            options.ReturnDocument = ReturnDocument.After;
            options.Projection = projection;
            var result = await Collection.FindOneAndUpdateAsync(filter1, update, options);
            return result;
        }

        public async Task<List<UserMessage>> GetUserMessage(string userId, int skip, int limit)
        {
            var userMessage = new List<UserMessageBasicInformation>();
            var sortBuilder = Builders<UserMessage>.Sort;
            var sortOrder = sortBuilder.Descending(s => s.Time);
            var projection = Builders<UserMessage>.Projection.Exclude("_id").Exclude(s => s.UserId).Exclude(s => s.IsDelivered).Exclude(s => s.Type);
            var messaages = await Collection.Find(d => d.UserId == userId).Project<UserMessage>(projection).Sort(sortOrder).Skip(skip).Limit(limit).ToListAsync();
            if(messaages == null)
            {
                return null;
            }
            return messaages;
        }
        public async Task<int> GetUserMessageCount(string userId)
        {
            try
            {
                var count = await Collection.CountAsync(o => o.UserId == userId);
                return (int)count;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting order count" + ex);
            }
        }
    }
}
