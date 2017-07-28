using Dhobi.Common;
using Dhobi.Core.Device.DbModels;
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
    public class DeviceStatusRepository : Repository<DeviceStatus>, IDeviceStausRepository
    {
        public async Task<bool> AddDeviceStatus(DeviceStatus status)
        {
            try
            {
                var filter1 = Builders<DeviceStatus>.Filter.Eq(d => d.UserId, status.UserId);
                var filter2 = Builders<DeviceStatus>.Filter.Eq(d => d.AppId, status.AppId);
                var filter = Builders<DeviceStatus>.Filter.And(filter1, filter2);
                var update = Builders<DeviceStatus>.Update.Set(u => u.Status, status.Status)
                    .Set(u => u.RegistrationId, status.RegistrationId)
                    .SetOnInsert(u => u.AppId, status.AppId)
                    .SetOnInsert(u => u.DeviceOs, status.DeviceOs);
                var projection = Builders<DeviceStatus>.Projection.Exclude("_id");
                var options = new FindOneAndUpdateOptions<DeviceStatus, DeviceStatus>();
                options.IsUpsert = true;
                options.Projection = projection;
                options.ReturnDocument = ReturnDocument.After;
                var result = await Collection.FindOneAndUpdateAsync(filter, update, options);
                return !string.IsNullOrWhiteSpace(result.UserId);
            }
            catch (Exception e)
            {
                throw new Exception("Error adding device status" + e);
            }
        }

        public async Task<List<DeviceStatus>> GetDeviceStatus(string userId)
        {
            try
            {
                var projection = Builders<DeviceStatus>.Projection.Exclude("_id");
                var statuses = await Collection.Find(status => status.UserId == userId & status.Status == (int)DeviceOnlineStatus.Active).Project<DeviceStatus>(projection).ToListAsync();
                return statuses;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting device status" + ex);
            }
        }

        public async Task<bool> RemoveDeviceStatus(DeviceStatus status)
        {
            try
            {
                var filter1 = Builders<DeviceStatus>.Filter.Eq(d => d.UserId, status.UserId);
                var filter2 = Builders<DeviceStatus>.Filter.Eq(d => d.AppId, status.AppId);
                var filter3 = Builders<DeviceStatus>.Filter.Eq(d => d.DeviceOs, status.DeviceOs);
                var filter = Builders<DeviceStatus>.Filter.And(filter1, filter2, filter3);
                var result = await Collection.DeleteOneAsync(filter);
                return result.IsAcknowledged;
            }
            catch (Exception ex)
            {
                throw new Exception("Error removing device status" + ex);
            }
        }
    }
}
