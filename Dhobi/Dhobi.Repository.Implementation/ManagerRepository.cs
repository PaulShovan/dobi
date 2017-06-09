using Dhobi.Core.Manager.DbModels;
using Dhobi.Repository.Implementation.Base;
using Dhobi.Repository.Interface;
using MongoDB.Driver;
using System.Threading.Tasks;
using System;
using Dhobi.Core.Manager.ViewModels;
using System.Collections.Generic;
using Dhobi.Common;

namespace Dhobi.Repository.Implementation
{
    public class ManagerRepository : Repository<Manager>, IManagerRepository
    {
        public async Task<bool> AddManager(Manager manager)
        {
            await Collection.InsertOneAsync(manager);
            return true;
        }

        public async Task<bool> IsEmailAvailable(string email)
        {
            var result = await Collection.CountAsync(manager => manager.Email == email);
            if (result > 0)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> IsUserNameAvailable(string userName)
        {
            var result = await Collection.CountAsync(manager => manager.UserName == userName);
            if (result > 0)
            {
                return false;
            }
            return true;
        }
        public async Task<int> GetManagerCount()
        {
            var count = await Collection.CountAsync(m => m.Status != (int)ManagerStatus.Removed);
            return (int)count;
        }
        public async Task<bool> RemoveManager(string managerId)
        {
            var update = Builders<Manager>.Update.Set(d => d.Status, (int)ManagerStatus.Removed);
            var filter = Builders<Manager>.Filter.Eq(d => d.UserId, managerId);
            var projection = Builders<Manager>.Projection.Exclude("_id");
            var options = new FindOneAndUpdateOptions<Manager, Manager>();
            options.IsUpsert = false;
            options.ReturnDocument = ReturnDocument.After;
            options.Projection = projection;
            var result = await Collection.FindOneAndUpdateAsync(filter, update, options);
            return !string.IsNullOrWhiteSpace(result.UserId);
        }
        public async Task<List<Manager>> GetManager(int skip, int limit)
        {
            try
            {
                var sortBuilder = Builders<Manager>.Sort;
                var sortOrder = sortBuilder.Ascending(s => s.JoinDate);
                var projection = Builders<Manager>.Projection.Exclude("_id").Exclude(s => s.AddedBy);
                var managers = await Collection.Find(d => d.Status != (int)ManagerStatus.Removed).Project<Manager>(projection).Sort(sortOrder).Skip(skip).Limit(limit).ToListAsync();
                return managers;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting manager." + ex);
            }
        }
        public async Task<ManagerBasicInformation> ManagerLogin(LoginViewModel loginModel)
        {
            try
            {
                var builder = Builders<Manager>.Filter;
                var filter = builder.Eq(user => user.UserName, loginModel.UserName) & builder.Eq(user => user.Password, loginModel.Password);
                var projection = Builders<Manager>.Projection.Exclude("_id")
                    .Include(u => u.UserName)
                    .Include(u => u.UserId)
                    .Include(u => u.Name)
                    .Include(u => u.Roles)
                    .Include(u => u.Email);
                var result = await Collection.Find(filter).Project<Manager>(projection).FirstOrDefaultAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in login" + ex);
            }
        }
    }
}
