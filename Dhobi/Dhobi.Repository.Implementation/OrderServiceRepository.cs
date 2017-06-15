using Dhobi.Common;
using Dhobi.Core.OrderService.DbModels;
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
    public class OrderServiceRepository : Repository<OrderService>, IOrderServiceRepository
    {
        public async Task<bool> AddService(List<OrderService> orderServices)
        {
            try
            {
                await Collection.InsertManyAsync(orderServices);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding service"+ex);
            }
        }
        public async Task<List<OrderService>> GetOrderServices()
        {
            try
            {
                var projection = Builders<OrderService>.Projection.Exclude("_id");
                var services = await Collection.Find(s => s.ServiceStatus != (int)ServiceStatus.Removed).Project<OrderService>(projection).ToListAsync();
                return services;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in getting services" + ex);
            }
        }

        public async Task<bool> RemoveService(string orderServiceId)
        {
            try
            {
                var update = Builders<OrderService>.Update.Set(d => d.ServiceStatus, (int)ServiceStatus.Removed);
                var filter = Builders<OrderService>.Filter.Eq(d => d.OrderServiceId, orderServiceId);
                var projection = Builders<OrderService>.Projection.Exclude("_id");
                var options = new FindOneAndUpdateOptions<OrderService, OrderService>();
                options.IsUpsert = false;
                options.ReturnDocument = ReturnDocument.After;
                options.Projection = projection;
                var result = await Collection.FindOneAndUpdateAsync(filter, update, options);
                if (result == null)
                {
                    return false;
                }
                return !string.IsNullOrWhiteSpace(result.OrderServiceId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error removing order service"+ex);
            }
        }
    }
}
