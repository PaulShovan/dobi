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
using Dhobi.Core.Dobi.DbModels;

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

        public async Task<List<Order>> GetOrdersByZone(string zone, int orderStatus, string serviceIdString)
        {
            try
            {
                var builder = Builders<Order>.Filter;
                var filter = builder.Eq(order => order.Zone, zone) & builder.Eq(order => order.Status, orderStatus);
                if (!string.IsNullOrWhiteSpace(serviceIdString))
                {
                    var regexFilter = ".*" + serviceIdString + ".*";
                    var bsonRegex = new BsonRegularExpression(regexFilter, "i");
                    filter = builder.Eq(order => order.Zone, zone) & builder.Eq(order => order.Status, orderStatus) & builder.Regex(x => x.ServiceId, bsonRegex);
                }
                var projection = Builders<Order>.Projection.Exclude("_id")
                    .Include(u => u.ServiceId)
                    .Include(u => u.Address)
                    .Include(u => u.OrderBy)
                    .Include(u => u.Status)
                    .Include(u => u.Lat)
                    .Include(u => u.Lon);
                var result = await Collection.Find(filter).Project<Order>(projection).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting orders"+ex);
            }
        }
        public async Task<Order> GetNewOrderForDobi(string serviceId)
        {
            try
            {
                var builder = Builders<Order>.Filter;
                var filter = builder.Eq(order => order.ServiceId, serviceId);
                var projection = Builders<Order>.Projection.Exclude("_id")
                    .Include(u => u.ServiceId)
                    .Include(u => u.Address)
                    .Include(u => u.OrderBy)
                    .Include(u => u.Status)
                    .Include(u => u.Lat)
                    .Include(u => u.Lon);
                var result = await Collection.Find(filter).Project<Order>(projection).FirstOrDefaultAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting orders" + ex);
            }
        }

        public async Task<List<BsonDocument>> GetOrdersGroupByZone(int orderStatus, string serviceIdString)
        {
            try
            {
                var builder = Builders<Order>.Filter;
                var filter = builder.Eq(o => o.Status, orderStatus);
                if (!string.IsNullOrWhiteSpace(serviceIdString))
                {
                    var regexFilter = ".*" + serviceIdString + ".*";
                    var bsonRegex = new BsonRegularExpression(regexFilter, "i");
                    filter = builder.Eq(o => o.Status, orderStatus) & builder.Regex(x => x.ServiceId, bsonRegex);
                }
                var projectionDefinition = new BsonDocument{
                    { "Zone", "$_id"},
                    {"Orders", "$Orders"},
                    {"Count", "$Count"},
                    {"Lat", "$Lat"},
                    {"Lon", "$Lon"},
                    { "_id", 0}
                };
                var result = await Collection.Aggregate().Match(filter).Group(new BsonDocument { { "_id", "$Zone" }, { "Orders", new BsonDocument("$push", new BsonDocument { {"Address", "$Address"},
                               {"Status", "$Status"},{"Name", "$OrderBy.Name"},{"PhoneNumber", "$OrderBy.PhoneNumber"}, {"Lat", "$Lat"}, {"Lon", "$Lon"}, {"ServiceId", "$ServiceId"}}) }, {"Count", new BsonDocument("$sum", 1)} }).Project<BsonDocument>(projectionDefinition).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {

                throw new Exception("Error getting order by zone" + ex);
            }
        }

        public async Task<Order> SetOrderPickupDateTime(long date, string time, string serviceId, DobiBasicInformation dobi)
        {
            try
            {
                var filter = Builders<Order>.Filter.Eq(d => d.ServiceId, serviceId);
                var update = Builders<Order>.Update.Set(u => u.Status, (int)OrderStatus.Acknowledged)
                                                   .Set(u => u.PickUpDate, date)
                                                   .Set(u => u.PickUpTime, time)
                                                   .Set(u => u.Dobi, dobi);
                var projection = Builders<Order>.Projection.Exclude("_id");
                var options = new FindOneAndUpdateOptions<Order, Order>();
                options.ReturnDocument = ReturnDocument.After;
                options.Projection = projection;
                var result = await Collection.FindOneAndUpdateAsync(filter, update, options);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error setting pickup date time");
            }
        }

        public async Task<bool> CancelOrder(string serviceId)
        {
            try
            {
                var filter = Builders<Order>.Filter.Eq(d => d.ServiceId, serviceId);
                var update = Builders<Order>.Update.Set(u => u.Status, (int)OrderStatus.Cancelled);
                var projection = Builders<Order>.Projection.Exclude("_id");
                var options = new FindOneAndUpdateOptions<Order, Order>();
                options.ReturnDocument = ReturnDocument.After;
                options.Projection = projection;
                var result = await Collection.FindOneAndUpdateAsync(filter, update, options);
                if(result == null)
                {
                    return false;
                }
                return !string.IsNullOrWhiteSpace(result.ServiceId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error cancelling order");
            }
        }

        public async Task<Order> ConfirmOrder(string serviceId)
        {
            try
            {
                var filter = Builders<Order>.Filter.Eq(d => d.ServiceId, serviceId);
                var update = Builders<Order>.Update.Set(u => u.Status, (int)OrderStatus.Confirmed);
                var projection = Builders<Order>.Projection.Exclude("_id");
                var options = new FindOneAndUpdateOptions<Order, Order>();
                options.ReturnDocument = ReturnDocument.After;
                options.Projection = projection;
                var result = await Collection.FindOneAndUpdateAsync(filter, update, options);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error confirming order");
            }
        }

        public async Task<List<Order>> GetUserOrders(string userId, int skip, int limit)
        {
            try
            {
                var builder = Builders<Order>.Filter;
                var filter = builder.Eq(order => order.OrderBy.UserId, userId)
                    & builder.Ne(order => order.Status, (int)OrderStatus.Paid)
                    & builder.Ne(order => order.Status, (int)OrderStatus.New)
                    & builder.Ne(order => order.Status, (int)OrderStatus.Acknowledged)
                    & builder.Ne(order => order.Status, (int)OrderStatus.Confirmed)
                    & builder.Ne(order => order.Status, (int)OrderStatus.Cancelled);
                var sortBuilder = Builders<Order>.Sort;
                var sortOrder = sortBuilder.Ascending(s => s.ServiceId);
                var projection = Builders<Order>.Projection.Exclude("_id").Exclude(s => s.OrderBy);
                var orders = await Collection.Find(filter).Project<Order>(projection).Sort(sortOrder).Skip(skip).Limit(limit).ToListAsync();
                return orders;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting orders." + ex);
            }
        }

        public async Task<Order> GetOrderAcknowledgeInformation(string serviceId)
        {
            try
            {
                var builder = Builders<Order>.Filter;
                var filter = builder.Eq(order => order.ServiceId, serviceId);
                var projection = Builders<Order>.Projection.Exclude("_id")
                    .Include(u => u.ServiceId)
                    .Include(u => u.Dobi)
                    .Include(u => u.PickUpDate)
                    .Include(u => u.PickUpTime);
                var result = await Collection.Find(filter).Project<Order>(projection).FirstOrDefaultAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting order" + ex);
            }
        }

        public async Task<Order> GetOrderForPickUp(string serviceId)
        {
            try
            {
                var builder = Builders<Order>.Filter;
                var filter = builder.Eq(order => order.ServiceId, serviceId);
                var projection = Builders<Order>.Projection.Exclude("_id")
                    .Include(u => u.ServiceId)
                    .Include(u => u.OrderBy)
                    .Include(u => u.Address)
                    .Include(u => u.PickUpDate)
                    .Include(u => u.PickUpTime);
                var result = await Collection.Find(filter).Project<Order>(projection).FirstOrDefaultAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting order" + ex);
            }
        }

        public async Task<int> GetUserOrderCount(string userId)
        {
            try
            {
                var builder = Builders<Order>.Filter;
                var filter = builder.Eq(order => order.OrderBy.UserId, userId)
                    & builder.Ne(order => order.Status, (int)OrderStatus.Paid)
                    & builder.Ne(order => order.Status, (int)OrderStatus.New)
                    & builder.Ne(order => order.Status, (int)OrderStatus.Acknowledged)
                    & builder.Ne(order => order.Status, (int)OrderStatus.Confirmed)
                    & builder.Ne(order => order.Status, (int)OrderStatus.Cancelled);
                var count = await Collection.CountAsync(filter);
                return (int)count;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting orders." + ex);
            }
        }

        public async Task<bool> SetOrderPickup(OrderItem items, string serviceId)
        {
            try
            {
                var filter = Builders<Order>.Filter.Eq(d => d.ServiceId, serviceId);
                var update = Builders<Order>.Update.AddToSet(u => u.OrderItems, items)
                    .Set(u => u.Status, (int)OrderStatus.PickedUp);
                var projection = Builders<Order>.Projection.Exclude("_id");
                var options = new FindOneAndUpdateOptions<Order, Order>();
                options.ReturnDocument = ReturnDocument.After;
                options.Projection = projection;
                var result = await Collection.FindOneAndUpdateAsync(filter, update, options);
                if (result == null)
                {
                    return false;
                }
                return !string.IsNullOrWhiteSpace(result.ServiceId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error confirming order");
            }
        }

        public async Task<Order> GetOrderById(string serviceId)
        {
            try
            {
                var builder = Builders<Order>.Filter;
                var filter = builder.Eq(order => order.ServiceId, serviceId);
                var projection = Builders<Order>.Projection.Exclude("_id");
                var result = await Collection.Find(filter).Project<Order>(projection).FirstOrDefaultAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting order" + ex);
            }
        }

        public async Task<List<Order>> GetAllOrders(long from, long to, int skip, int limit)
        {
            try
            {
                var builder = Builders<Order>.Filter;
                var filter =  builder.Ne(order => order.Status, (int)OrderStatus.Paid)
                    & builder.Ne(order => order.Status, (int)OrderStatus.New)
                    & builder.Ne(order => order.Status, (int)OrderStatus.Acknowledged)
                    & builder.Ne(order => order.Status, (int)OrderStatus.Confirmed)
                    & builder.Ne(order => order.Status, (int)OrderStatus.Cancelled)
                    & builder.Gte(order => order.PickUpDate, from)
                    & builder.Lte(order => order.PickUpDate, to);
                var sortBuilder = Builders<Order>.Sort;
                var sortOrder = sortBuilder.Ascending(s => s.ServiceId);
                var projection = Builders<Order>.Projection.Exclude("_id").Exclude(s => s.Dobi);
                var orders = await Collection.Find(filter).Project<Order>(projection).Sort(sortOrder).Skip(skip).Limit(limit).ToListAsync();
                return orders;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting orders." + ex);
            }
        }

        public async Task<bool> UpdateOrderStatus(List<string> orders, int status)
        {
            try
            {
                var filter = Builders<Order>.Filter.In(d => d.ServiceId, orders);
                var update = Builders<Order>.Update.Set(u => u.Status, status);
                var result = await Collection.UpdateManyAsync(filter, update);
                return result.IsAcknowledged;
            }
            catch (Exception ex)
            {
                throw new Exception("Error confirming order");
            }
        }

        public async Task<bool> UpdateOrderAsPaid(decimal amount, string serviceId)
        {
            try
            {
                var filter = Builders<Order>.Filter.Eq(d => d.ServiceId, serviceId);
                var update = Builders<Order>.Update.Set(u => u.Status, (int)OrderStatus.Paid)
                                                   .Set(u => u.GrandTotal, amount);
                var result = await Collection.UpdateOneAsync(filter, update);
                return result.IsAcknowledged;
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating order");
            }
        }
    }
}
