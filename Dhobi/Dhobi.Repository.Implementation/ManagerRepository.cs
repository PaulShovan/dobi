using Dhobi.Core.Manager.DbModels;
using Dhobi.Repository.Implementation.Base;
using Dhobi.Repository.Interface;
using MongoDB.Driver;
using System.Threading.Tasks;
using System;
using Dhobi.Core.Manager.ViewModels;

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
