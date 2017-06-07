using Dhobi.Core.OrderModel.DbModels;
using Dhobi.Repository.Implementation.Base;
using Dhobi.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;
using System.Threading.Tasks;
using Dhobi.Common;
using Dhobi.Core.OrderModel.ViewModels;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace Dhobi.Repository.Implementation
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public async Task<bool> AddNewOrder(Order order)
        {
            try
            {
                await Collection.InsertOneAsync(order);
                return true;
            }
            catch (Exception ex)
            {

                throw new Exception("Error adding new order.");
            }
        }

        public async Task<int> GetNewOrderCountByStatus(int orderStatus)
        {
            try
            {
                var count = await Collection.CountAsync(o => o.Status == orderStatus);
                return (int)count;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting order count" + ex);
            }
        }

        public async Task<int> GetOrderCount()
        {
            try
            {
                var count = await Collection.CountAsync(o => o.ServiceId != "");
                return (int)count;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting order count" + ex);
            }
        }

        public async Task<List<Order>> GetOrdersByZone(string zone, int orderStatus)
        {
            try
            {
                var builder = Builders<Order>.Filter;
                var filter = builder.Eq(order => order.Zone, zone) & builder.Eq(order => order.Status, orderStatus);
                var projection = Builders<Order>.Projection.Exclude("_id")
                    .Include(u => u.ServiceId)
                    .Include(u => u.Address)
                    .Include(u => u.OrderBy)
                    .Include(u => u.Status);
                var result = await Collection.Find(filter).Project<Order>(projection).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting orders"+ex);
            }
        }

        public async Task<List<BsonDocument>> GetOrdersGroupByZone(int orderStatus)
        {
            try
            {
                var builder = Builders<Order>.Filter;
                var filter = builder.Eq(o => o.Status, orderStatus);
                var projectionDefinition = new BsonDocument{
                    { "Zone", "$_id"},
                    {"Orders", "$Orders"},
                    {"Count", "$Count"},
                    { "_id", 0}
                };
                var result = await Collection.Aggregate().Match(filter).Group(new BsonDocument { { "_id", "$Zone" }, { "Orders", new BsonDocument("$push", new BsonDocument { {"Address", "$Address"},
                               {"Status", "$Status"},{"Name", "$OrderBy.Name"}, {"ServiceId", "$ServiceId"}}) }, {"Count", new BsonDocument("$sum", 1)} }).Project<BsonDocument>(projectionDefinition).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {

                throw new Exception("Error getting order by zone" + ex);
            }
        }
    }
}
