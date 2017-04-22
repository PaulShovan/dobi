using Dhobi.Core.RegistrationSms;
using Dhobi.Repository.Implementation.Base;
using Dhobi.Repository.Interface;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace Dhobi.Repository.Implementation
{
    public class RegistrationSmsRepository : Repository<RegistrationSms>, IRegistrationSmsRepository
    {
        public async Task<bool> AddRegistrationSms(RegistrationSms sms)
        {
            await Collection.InsertOneAsync(sms);
            return true;
        }

        public async Task<bool> ValidateRegisteredUser(string code, string userId)
        {
            var filter1 = Builders<RegistrationSms>.Filter.Eq(d => d.UserId, userId);
            var filter2 = Builders<RegistrationSms>.Filter.Eq(d => d.ApprovalCode, code);
            var filter3 = Builders<RegistrationSms>.Filter.Eq(d => d.Status, 0);
            var filter = Builders<RegistrationSms>.Filter.And(filter1, filter2, filter3);
            var update = Builders<RegistrationSms>.Update.Set(u => u.Status, 1);
            var options = new FindOneAndUpdateOptions<RegistrationSms, RegistrationSms>();
            options.ReturnDocument = ReturnDocument.After;
            var result = await Collection.FindOneAndUpdateAsync(filter, update, options);
            return !string.IsNullOrWhiteSpace(result.UserId);
        }
    }
}
