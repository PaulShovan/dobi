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
            var builder = Builders<Manager>.Filter;
            var filter = builder.Eq(user => user.Status, (int)ManagerStatus.Active) & builder.Eq(manager => manager.Email, email);
            var result = await Collection.CountAsync(filter);
            if (result > 0)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> IsUserNameAvailable(string userName)
        {
            var builder = Builders<Manager>.Filter;
            var filter = builder.Eq(user => user.Status, (int)ManagerStatus.Active) & builder.Eq(manager => manager.UserName, userName);
            var result = await Collection.CountAsync(filter);
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
            if(result == null)
            {
                return false;
            }
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
        public async Task<Manager> GetManagerById(string userId)
        {
            try
            {
                var builder = Builders<Manager>.Filter;
                var filter = builder.Eq(user => user.UserId, userId) & builder.Ne(user => user.Status, (int)ManagerStatus.Removed);
                var projection = Builders<Manager>.Projection.Exclude("_id").Exclude(s => s.AddedBy).Exclude(s => s.Password);
                var manager = await Collection.Find(filter).Project<Manager>(projection).FirstOrDefaultAsync();
                return manager;
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
                var filter = builder.Eq(user => user.UserName, loginModel.UserName) & builder.Eq(user => user.Password, loginModel.Password) & builder.Eq(user => user.Status, (int)ManagerStatus.Active);
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

        public async Task<bool> UpdateManager(Manager manager)
        {
            var update = Builders<Manager>.Update.Set(d => d.Name, manager.Name)
                                                .Set(d => d.Phone, manager.Phone)
                                                .Set(d => d.Email, manager.Email)
                                                .Set(d => d.Address, manager.Address)
                                                .Set(d => d.EmergencyContactNumber, manager.EmergencyContactNumber)
                                                .Set(d => d.PassportNumber, manager.PassportNumber)
                                                .Set(d => d.IcNumber, manager.IcNumber)
                                                .Set(d => d.DrivingLicense, manager.DrivingLicense)
                                                .Set(d => d.Age, manager.Age)
                                                .Set(d => d.Sex, manager.Sex)
                                                .Set(d => d.Roles, manager.Roles)
                                                .Set(d => d.UserName, manager.UserName)
                                                .Set(d => d.Password, manager.Password)
                                                .Set(d => d.Salary, manager.Salary)
                                                .Set(d => d.Photo, manager.Photo);

            var filter = Builders<Manager>.Filter.Eq(d => d.UserId, manager.UserId);
            var projection = Builders<Manager>.Projection.Exclude("_id").Exclude(m => m.AddedBy);
            var options = new FindOneAndUpdateOptions<Manager, Manager>();
            options.IsUpsert = false;
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
