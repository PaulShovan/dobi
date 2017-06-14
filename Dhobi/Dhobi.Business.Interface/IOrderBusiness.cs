using Dhobi.Core.Dobi.DbModels;
using Dhobi.Core.OrderModel.DbModels;
using Dhobi.Core.OrderModel.ViewModels;
using Dhobi.Core.UserModel.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhobi.Business.Interface
{
    public interface IOrderBusiness
    {
        Task<bool> AddNewOrder(NewOrderViewModel order, User orderedBy);
        Task<List<OrderByZoneViewModel>> GetOrdersGroupByZone(int orderStatus);
        Task<OrderByZoneViewModel> GetOrdersByZone(string zone, int orderStatus);
        Task<OrderItemViewModel> GetNewOrderForDobi(string serviceId);
        Task<bool> SetOrderPickupDateTime(OrderPickupTimeViewModel order, Dobi dobi);
    }
}
