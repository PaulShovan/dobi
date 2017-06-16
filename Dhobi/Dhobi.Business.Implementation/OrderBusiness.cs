using Dhobi.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dhobi.Core.OrderModel.DbModels;
using Dhobi.Repository.Interface;
using Dhobi.Core.OrderModel.ViewModels;
using Dhobi.Service.Interface;
using Dhobi.Core.UserModel.DbModels;
using Dhobi.Core.PromoOffer.ViewModels;
using Dhobi.Common;
using MongoDB.Bson.Serialization;
using Dhobi.Core.Dobi.DbModels;

namespace Dhobi.Business.Implementation
{
    public class OrderBusiness : IOrderBusiness
    {
        private const int OrderServiceIdLength = 10;
        private IOrderRepository _orderRepository;
        private IPromoOfferBusiness _promoBusiness;
        private IUserMessageBusiness _userMessageBusiness;
        public OrderBusiness(IOrderRepository orderRepository, IPromoOfferBusiness promoOfferBusiness, IUserMessageBusiness userMessageBusiness)
        {
            _orderRepository = orderRepository;
            _promoBusiness = promoOfferBusiness;
            _userMessageBusiness = userMessageBusiness;
        }
        private async Task<PromoOfferBasicInformation> GetPromoOffer()
        {
            var promo = await _promoBusiness.GetPromoOfferForUser();
            if(promo == null)
            {
                return null;
            }
            return new PromoOfferBasicInformation
            {
                PromoText = promo.Text,
                Amount = promo.Amount
            };
        }
        public async Task<bool> AddNewOrder(NewOrderViewModel order, User orderedBy)
        {
            try
            {
                var totalOrder = await _orderRepository.GetOrderCount();
                var newServiceId = (totalOrder + 1).ToString().PadLeft(OrderServiceIdLength, '0');
                var newOrder = new Order
                {
                    ServiceId = newServiceId,
                    OrderBy = orderedBy,
                    Address = order.Address,
                    Zone = order.Zone,
                    Promotion = await GetPromoOffer(),
                    Status = (int)OrderStatus.New,
                    OrderPlacingTime = Utilities.GetPresentDateTime()
                };
                var addMessageResponse = await _userMessageBusiness.AddUserMessage(orderedBy.UserId, (int)MessageType.NewOrder, newServiceId);
                var addOrderResponse = await _orderRepository.AddNewOrder(newOrder);
                //TODO Send Notification
                return addMessageResponse && addOrderResponse;
            }
            catch (Exception ex)
            {
                throw new Exception("Error placing order." + ex);
            }
        }

        public async Task<List<OrderByZoneViewModel>> GetOrdersGroupByZone(int orderStatus)
        {
            try
            {
                var orders = new List<OrderByZoneViewModel>();
                var bsonOrders = await _orderRepository.GetOrdersGroupByZone(orderStatus);
                foreach (var order in bsonOrders)
                {
                    var deserializedOrder = BsonSerializer.Deserialize<OrderByZoneViewModel>(order);
                    deserializedOrder.Orders = deserializedOrder.Orders.Take(5).ToList();
                    orders.Add(deserializedOrder);
                }
                return orders;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting order by zone" + ex);
            }
        }
        private List<OrderItemViewModel> GetOrderItems(List<Order> orders)
        {
            var orderItems = new List<OrderItemViewModel>();
            foreach (var order in orders)
            {
                orderItems.Add(new OrderItemViewModel
                {
                    Status = order.Status,
                    Name = order.OrderBy.Name,
                    Address = order.Address,
                    ServiceId = order.ServiceId
                });
            }
            return orderItems;
        }
        public async Task<OrderByZoneViewModel> GetOrdersByZone(string zone, int orderStatus)
        {
            try
            {
                var orders = await _orderRepository.GetOrdersByZone(zone, orderStatus);
                if(orders == null || orders.Count <= 0)
                {
                    return null;
                }
                return new OrderByZoneViewModel {
                    Zone = zone,
                    Orders = GetOrderItems(orders)
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting order by zone" + ex);
            }
        }

        public async Task<OrderItemViewModel> GetNewOrderForDobi(string serviceId)
        {
            try
            {
                var order = await _orderRepository.GetNewOrderForDobi(serviceId);
                if (order == null)
                {
                    return null;
                }
                return new OrderItemViewModel
                {
                    ServiceId = order.ServiceId,
                    Address = order.Address,
                    Name = order.OrderBy.Name,
                    Status = order.Status
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting order" + ex);
            }
        }
        public async Task<bool> SetOrderPickupDateTime(OrderPickupTimeViewModel order, Dobi dobi)
        {
            try
            {
                var orderPickupDate = Utilities.GetMillisecondFromDate(order.PickupDate);
                var result = await _orderRepository.SetOrderPickupDateTime(orderPickupDate, order.PickupTime, order.ServiceId, dobi);
                if (result == null)
                {
                    return false;
                }
                var messageSend = await _userMessageBusiness.AddUserMessage(result.OrderBy.UserId, (int)MessageType.OrderAcknowledge, result.ServiceId);
                return messageSend;
            }
            catch (Exception ex)
            {
                throw new Exception("Error setting pickup date time" + ex);
            }
        }

        public async Task<List<UserOrderStatusViewModel>> GetUserOrders(string userId, int skip, int limit)
        {
            try
            {
                var orders = new List<UserOrderStatusViewModel>();
                var result = await _orderRepository.GetUserOrders(userId, skip, limit);
                if(result == null)
                {
                    return null;
                }
                foreach (var order in result)
                {
                    orders.Add(new UserOrderStatusViewModel
                    {
                        ServiceId = order.ServiceId,
                        Title = Constants.USER_ORDER_STATUS_TITLE,
                        Message = string.Format(Constants.USER_ORDER_STATUS_MESSAGE, order.ServiceId),
                        Status = order.Status
                    });
                }
                return orders;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting user orders" + ex);
            }
        }
    }
}
