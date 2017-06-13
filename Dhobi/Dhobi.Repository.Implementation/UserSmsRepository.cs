using Dhobi.Core.UserSms;
using Dhobi.Repository.Implementation.Base;
using Dhobi.Repository.Interface;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace Dhobi.Repository.Implementation
{
    public class UserSmsRepository : Repository<UserSms>, IUserSmsRepository
    {
        public async Task<bool> AddUserSms(UserSms sms)
        {
            await Collection.InsertOneAsync(sms);
            return true;
        }
        public async Task<bool> ValidateUser(string code, string userId)
        {
            var filter1 = Builders<UserSms>.Filter.Eq(d => d.UserId, userId);
            var filter2 = Builders<UserSms>.Filter.Eq(d => d.ApprovalCode, code);
            var filter3 = Builders<UserSms>.Filter.Eq(d => d.Status, 0);
            var filter = Builders<UserSms>.Filter.And(filter1, filter2, filter3);
            var update = Builders<UserSms>.Update.Set(u => u.Status, 1);
            var projection = Builders<UserSms>.Projection.Exclude("_id");
            var options = new FindOneAndUpdateOptions<UserSms, UserSms>();
            options.ReturnDocument = ReturnDocument.After;
            options.Projection = projection;
            var result = await Collection.FindOneAndUpdateAsync(filter, update, options);
            if(result == null)
            {
                return false;
            }
            return !string.IsNullOrWhiteSpace(result.UserId);
        }
    }
}
