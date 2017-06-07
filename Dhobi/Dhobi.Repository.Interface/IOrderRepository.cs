using Dhobi.Core.OrderModel.DbModels;
using Dhobi.Core.OrderModel.ViewModels;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhobi.Repository.Interface
{
    public interface IOrderRepository
    {
        Task<bool> AddNewOrder(Order order);
        Task<int> GetOrderCount();
        Task<int> GetNewOrderCountByStatus(int orderStatus);
        Task<List<BsonDocument>> GetOrdersGroupByZone(int orderStatus);
        Task<List<Order>> GetOrdersByZone(string zone, int orderStatus);
    }
}
