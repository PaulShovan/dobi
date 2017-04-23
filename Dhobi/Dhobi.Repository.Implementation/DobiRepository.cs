using Dhobi.Core.Dobi.DbModels;
using Dhobi.Repository.Implementation.Base;
using Dhobi.Repository.Interface;
using System;
using MongoDB.Driver;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Dhobi.Repository.Implementation
{
    public class DobiRepository : Repository<Dobi>, IDobiRepository
    {
        public async Task<bool> AddDobi(Dobi dobi)
        {
            await Collection.InsertOneAsync(dobi);
            return true;
        }

        public async Task<DobiBasicInformation> DobiLogin(string phone)
        {
            try
            {
                var builder = Builders<Dobi>.Filter;
                var filter = builder.Eq(dobi => dobi.Phone, phone);
                var projection = Builders<Dobi>.Projection.Exclude("_id")
                    .Include(u => u.Name)
                    .Include(u => u.DobiId)
                    .Include(u => u.Phone)
                    .Include(u => u.Photo);
                var result = await Collection.Find(filter).Project<DobiBasicInformation>(projection).FirstOrDefaultAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in login" + ex);
            }
        }

        public async Task<List<Dobi>> GetDobi(int skip, int limit)
        {
            try
            {
                var sortBuilder = Builders<Dobi>.Sort;
                var sortOrder = sortBuilder.Ascending(s => s.JoinDate);
                var projection = Builders<Dobi>.Projection.Exclude("_id").Exclude(s => s.AddedBy);
                var dobi = await Collection.Find(d => d.DobiId != "").Project<Dobi>(projection).Sort(sortOrder).Skip(skip).Limit(limit).ToListAsync();
                return dobi;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting dobi." + ex);
            }
        }

        public async Task<int> GetDobiCount()
        {
            var count = await Collection.CountAsync(dobi => dobi.DobiId != "");
            return (int)count;
        }

        public async Task<bool> IsDrivingLicenseAvailable(string drivingLicense)
        {
            var result = await Collection.CountAsync(dobi => dobi.DrivingLicense == drivingLicense);
            if (result > 0)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> IsEmailAvailable(string email)
        {
            var result = await Collection.CountAsync(dobi => dobi.Email == email);
            if (result > 0)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> IsIcNumberAvailable(string icNo)
        {
            var result = await Collection.CountAsync(dobi => dobi.IcNumber == icNo);
            if (result > 0)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> IsPassportNumberAvailable(string passport)
        {
            var result = await Collection.CountAsync(dobi => dobi.PassportNumber == passport);
            if (result > 0)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> IsPhoneNumberAvailable(string phone)
        {
            var result = await Collection.CountAsync(dobi => dobi.Phone == phone);
            if (result > 0)
            {
                return false;
            }
            return true;
        }
    }
}
