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
                    .Include(u => u.Photo)
                    .Include(u => u.IcNumber)
                    .Include(u => u.DrivingLicense);
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

        public async Task<Dobi> GetDobiById(string dobiId)
        {
            try
            {
                var builder = Builders<Dobi>.Filter;
                var filter = builder.Eq(d => d.DobiId, dobiId);
                var projection = Builders<Dobi>.Projection.Exclude("_id");
                var result = await Collection.Find(filter).Project<Dobi>(projection).FirstOrDefaultAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in getting user" + ex);
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

        public async Task<Dobi> UpdateDobi(Dobi dobi)
        {
            var update = Builders<Dobi>.Update.Set(d => d.Name, dobi.Name)
                                                .Set(d => d.Phone, dobi.Phone)
                                                .Set(d => d.Email, dobi.Email)
                                                .Set(d => d.Address, dobi.Address)
                                                .Set(d => d.EmergencyContactNumber, dobi.EmergencyContactNumber)
                                                .Set(d => d.PassportNumber, dobi.PassportNumber)
                                                .Set(d => d.IcNumber, dobi.IcNumber)
                                                .Set(d => d.DrivingLicense, dobi.DrivingLicense)
                                                .Set(d => d.Age, dobi.Age)
                                                .Set(d => d.Sex, dobi.Sex)
                                                .Set(d => d.Salary, dobi.Salary);
                                                
            var filter = Builders<Dobi>.Filter.Eq(d => d.DobiId, dobi.DobiId);
            var projection = Builders<Dobi>.Projection.Exclude("_id");
            var options = new FindOneAndUpdateOptions<Dobi, Dobi>();
            options.IsUpsert = false;
            options.ReturnDocument = ReturnDocument.After;
            options.Projection = projection;
            var result = await Collection.FindOneAndUpdateAsync(filter, update, options);
            return result;
        }
    }
}
