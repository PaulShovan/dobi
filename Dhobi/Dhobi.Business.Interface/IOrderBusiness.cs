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
        Task<List<OrderByZoneViewModel>> GetOrdersGroupByZone(int orderStatus, string serviceIdString);
        Task<OrderByZoneViewModel> GetOrdersByZone(string zone, int orderStatus, string serviceIdString);
        Task<OrderItemViewModel> GetNewOrderForDobi(string serviceId);
        Task<bool> SetOrderPickupDateTime(OrderPickupTimeViewModel order, DobiBasicInformation dobi);
        Task<bool> ConfirmOrder(string serviceId, User user);
        Task<UserOrderStatusResponseModel> GetUserOrders(string userId, int skip, int limit);
        Task<OrderPickupInformationViewModel> PickUpOrder(string serviceId);
        Task<bool> SetPickUpOrder(OrderItem items, string serviceId);
        Task<OrderSummaryViewModel> GetOrderSummary(string serviceId);
        Task<List<OrderListItemViewModel>> GetAllOrders(string date, int skip, int limit);
    }
}
