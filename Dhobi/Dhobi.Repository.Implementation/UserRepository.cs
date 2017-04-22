using Dhobi.Repository.Interface;
using System.Threading.Tasks;
using Dhobi.Core.UserModel.DbModels;
using Dhobi.Repository.Implementation.Base;
using MongoDB.Driver;
using System;

namespace Dhobi.Repository.Implementation
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public async Task<bool> AddUser(User user)
        {
            await Collection.InsertOneAsync(user);
            return true;
        }

        public async Task<User> GetUserById(string userId)
        {
            try
            {
                var builder = Builders<User>.Filter;
                var filter = builder.Eq(user => user.UserId, userId);
                var projection = Builders<User>.Projection.Exclude("_id")
                    .Include(u => u.Name)
                    .Include(u => u.UserId)
                    .Include(u => u.PhoneNumber);
                var result = await Collection.Find(filter).Project<User>(projection).FirstOrDefaultAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in getting user" + ex);
            }
        }

        public async Task<bool> IsPhoneNumberAvailable(string phoneNumber)
        {
            var result = await Collection.CountAsync(user => user.PhoneNumber == phoneNumber);
            if (result > 0)
            {
                return false;
            }
            return true;
        }

        public async Task<User> UpdateUserAsVerified(string userId)
        {
            var update = Builders<User>.Update.Set(u => u.IsVerified, true);
            var filter = Builders<User>.Filter.Eq(u => u.UserId, userId);
            var projection = Builders<User>.Projection.Exclude("_id");
            var options = new FindOneAndUpdateOptions<User, User>();
            options.IsUpsert = false;
            options.ReturnDocument = ReturnDocument.After;
            options.Projection = projection;
            var result = await Collection.FindOneAndUpdateAsync(filter, update, options);
            return result;
        }
    }
}
