using Dhobi.Core.Dobi.DbModels;
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
        Task<Order> GetNewOrderForDobi(string serviceId);
        Task<Order> SetOrderPickupDateTime(long date, string time, string serviceId, DobiBasicInformation dobi);
        Task<bool> CancelOrder(string serviceId);
        Task<Order> ConfirmOrder(string serviceId);
        Task<List<Order>> GetUserOrders(string userId, int skip, int limit);
        Task<int> GetUserOrderCount(string userId);
        Task<Order> GetOrderAcknowledgeInformation(string serviceId);
        Task<Order> GetOrderForPickUp(string serviceId);
        Task<bool> SetOrderPickup(OrderItem items, string serviceId);
        Task<Order> GetOrderById(string serviceId);
        Task<List<Order>> GetAllOrders(long from, long to, int skip, int limit);
        Task<bool> UpdateOrderStatus(List<string> orders, int status);
        Task<bool> UpdateOrderAsPaid(decimal amount, string serviceId);
    }
}
