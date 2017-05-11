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
    }
}
