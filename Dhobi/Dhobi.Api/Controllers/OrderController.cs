using Dhobi.Api.Helpers;
using Dhobi.Api.Models;
using Dhobi.Business.Interface;
using Dhobi.Common;
using Dhobi.Core.Dobi.DbModels;
using Dhobi.Core.OrderModel.DbModels;
using Dhobi.Core.OrderModel.ViewModels;
using Dhobi.Core.UserModel.DbModels;
using Dhobi.Repository.Interface;
using Dhobi.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Dhobi.Api.Controllers
{
    [RoutePrefix("api")]
    public class OrderController : ApiController
    {
        private IOrderBusiness _orderBusiness;
        private IPromoOfferBusiness _promoOfferBusiness;
        private IOrderServiceBusiness _orderServiceBusiness;
        private TokenGenerator _tokenGenerator;
        private ILocationService _locationService;
        private IOrderRepository _orderRepository;
        public OrderController(IOrderBusiness orderBusiness, 
            ILocationService locationService, 
            IOrderServiceBusiness orderServiceBusiness, 
            IPromoOfferBusiness promoOfferBusiness,
            IOrderRepository orderRepository)
        {
            _orderBusiness = orderBusiness;
            _orderServiceBusiness = orderServiceBusiness;
            _locationService = locationService;
            _promoOfferBusiness = promoOfferBusiness;
            _tokenGenerator = new TokenGenerator();
            _orderRepository = orderRepository;
        }
        private User GetUserInformationFromToken()
        {
            IEnumerable<string> values;
            var token = "";
            if (Request.Headers.TryGetValues("Authorization", out values))
            {
                token = values.FirstOrDefault();
            }
            if (string.IsNullOrWhiteSpace(token))
            {
                return null;
            }
            var user = _tokenGenerator.GetUserFromToken(token);
            return user;
        }
        private DobiBasicInformation GetDobiInformationFromToken()
        {
            IEnumerable<string> values;
            var token = "";
            if (Request.Headers.TryGetValues("Authorization", out values))
            {
                token = values.FirstOrDefault();
            }
            if (string.IsNullOrWhiteSpace(token))
            {
                return null;
            }
            var dobi = _tokenGenerator.GetDobiFromToken(token);
            return dobi;
        }
        [HttpPost]
        [Route("v1/order")]
        [Authorize]
        public async Task<IHttpActionResult> AddNewOrder(NewOrderViewModel order)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new ResponseModel<string>(ResponseStatus.BadRequest, null, "Invalid order data."));
            }
            var user = GetUserInformationFromToken();
            if (user == null || string.IsNullOrEmpty(user.UserId))
            {
                return BadRequest("Invalid User.");
            }
            var response = await _orderBusiness.AddNewOrder(order, user);
            if (!response)
            {
                return Ok(new ResponseModel<string>(ResponseStatus.BadRequest, null, "Order was not processed successfully."));
            }
            return Ok(new ResponseModel<string>(ResponseStatus.Ok, null, "Order placed successfully."));
        }
        [HttpGet]
        [Route("v1/order/groups")]
        [Authorize]
        public async Task<IHttpActionResult> GetOrdersGroupByZone(int status = 1)
        {
            if(status < 0)
            {
                return BadRequest("Invalid Order Status.");
            }
            var groupedOrders = await _orderBusiness.GetOrdersGroupByZone(status);
            if(groupedOrders == null)
            {
                return Ok(new ResponseModel<string>(ResponseStatus.NotFound, null, "No order available."));
            }
            return Ok(new ResponseModel<List<OrderByZoneViewModel>>(ResponseStatus.Ok, groupedOrders, ""));
        }
        [HttpGet]
        [Route("v1/order/groups/zone")]
        [Authorize]
        public async Task<IHttpActionResult> GetOrdersByZone(string zone, int status = 1)
        {
            if (status < 0 || string.IsNullOrWhiteSpace(zone))
            {
                return BadRequest("Invalid order status or zone.");
            }
            var groupedOrders = await _orderBusiness.GetOrdersByZone(zone, status);
            if (groupedOrders == null)
            {
                return Ok(new ResponseModel<string>(ResponseStatus.NotFound, null, "No order available."));
            }
            return Ok(new ResponseModel<OrderByZoneViewModel>(ResponseStatus.Ok, groupedOrders, ""));
        }
        [HttpGet]
        [Route("v1/order/home")]
        [Authorize]
        public async Task<IHttpActionResult> GetOrderHomePageInformation()
        {
            var zones = await _locationService.GetAvailableActiveZones();
            if (zones == null)
            {
                return Ok(new ResponseModel<string>(ResponseStatus.NotFound, null, "No zone available."));
            }
            var promoOffer = await _promoOfferBusiness.GetPromoOfferForUser();

            var promo = promoOffer == null ? null : new PromoOfferResponse
            {
                PromoText = promoOffer.Text,
                Amount = promoOffer.Amount
            };
            var response = new OrderHomePageResponse
            {
                Zones = zones,
                Promo = promo
            };
            return Ok(new ResponseModel<OrderHomePageResponse>(ResponseStatus.Ok, response, ""));
        }
        [HttpGet]
        [Route("v1/order/new")]
        [Authorize]
        public async Task<IHttpActionResult> GetNewOrderForDobi(string serviceId)
        {
            if (string.IsNullOrWhiteSpace(serviceId))
            {
                return Ok(new ResponseModel<string>(ResponseStatus.BadRequest, null, "Invalid service id"));
            }
            var order = await _orderBusiness.GetNewOrderForDobi(serviceId);
            if (order == null)
            {
                return Ok(new ResponseModel<string>(ResponseStatus.NotFound, null, "No order found."));
            }
            return Ok(new ResponseModel<OrderItemViewModel>(ResponseStatus.Ok, order, ""));
        }
        [HttpPost]
        [Route("v1/order/new/setpickup")]
        [Authorize]
        public async Task<IHttpActionResult> SetOrderPickupDateTime(OrderPickupTimeViewModel order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid order pickup date or time.");
            }
            var dobi = GetDobiInformationFromToken();
            if (dobi == null || string.IsNullOrEmpty(dobi.DobiId))
            {
                return BadRequest("Invalid User.");
            }
            var ack = await _orderBusiness.SetOrderPickupDateTime(order, dobi);
            if (!ack)
            {
                return Ok(new ResponseModel<string>(ResponseStatus.BadRequest, null, "Pickup time and date not set."));
            }
            return Ok(new ResponseModel<string>(ResponseStatus.Ok, "", "Order pickup date time set successfully."));
        }
        [HttpGet]
        [Route("v1/user/orders")]
        [Authorize]
        public async Task<IHttpActionResult> GetUserOrders(int skip = 0, int limit = 10)
        {
            var user = GetUserInformationFromToken();
            if (user == null || string.IsNullOrEmpty(user.UserId))
            {
                return BadRequest("Invalid User.");
            }
            var userOrders = await _orderBusiness.GetUserOrders(user.UserId, skip, limit);
            if (userOrders == null)
            {
                return Ok(new ResponseModel<string>(ResponseStatus.NotFound, null, "No order available."));
            }
            return Ok(new ResponseModel<UserOrderStatusResponseModel>(ResponseStatus.Ok, userOrders, ""));
        }
        [HttpGet]
        [Route("v1/order/pickup")]
        [Authorize]
        public async Task<IHttpActionResult> PickUpOrder(string serviceId)
        {
            if (string.IsNullOrWhiteSpace(serviceId))
            {
                return Ok(new ResponseModel<string>(ResponseStatus.BadRequest, null, "Invalid Order."));
            }
            var orderInformation = await _orderBusiness.PickUpOrder(serviceId);
            if (orderInformation == null)
            {
                return Ok(new ResponseModel<string>(ResponseStatus.NotFound, null, "Order is not available."));
            }
            var services = await _orderServiceBusiness.GetOrderServices();
            if(services == null)
            {
                return Ok(new ResponseModel<string>(ResponseStatus.BadRequest, null, "Service is not available."));
            }
            var response = new OrderPickupResponse
            {
                Services = services,
                Order = orderInformation
            };
            return Ok(new ResponseModel<OrderPickupResponse>(ResponseStatus.Ok, response, ""));
        }
        [HttpPost]
        [Route("v1/order/pickup")]
        [Authorize]
        public async Task<IHttpActionResult> SetPickUpOrder(SetOrderPickUpViewModel setOrderItems)
        {
            if (setOrderItems == null || string.IsNullOrWhiteSpace(setOrderItems.ServiceId) || setOrderItems.OrderItems == null)
            {
                return Ok(new ResponseModel<string>(ResponseStatus.BadRequest, null, "Invalid Order Items."));
            }
            var response = await _orderBusiness.SetPickUpOrder(setOrderItems.OrderItems, setOrderItems.ServiceId);
            if (!response)
            {
                return Ok(new ResponseModel<string>(ResponseStatus.BadRequest, null, "Order items were not set."));
            }
            return Ok(new ResponseModel<string>(ResponseStatus.Ok, null, "Order items set successfully."));
        }
        [HttpGet]
        [Route("v1/order/cancel")]
        [Authorize]
        public async Task<IHttpActionResult> CancelOrder(string serviceId)
        {
            if (string.IsNullOrWhiteSpace(serviceId))
            {
                return Ok(new ResponseModel<string>(ResponseStatus.NotFound, null, "Invalid service."));
            }
            var result = await _orderRepository.CancelOrder(serviceId);
            if (!result)
            {
                return Ok(new ResponseModel<string>(ResponseStatus.NotFound, null, "Service not found"));
            }
            return Ok(new ResponseModel<string>(ResponseStatus.Ok, null, "Order has been cancelled."));
        }
        [HttpGet]
        [Route("v1/order/confirm")]
        [Authorize]
        public async Task<IHttpActionResult> ConfirmOrder(string serviceId)
        {
            var user = GetUserInformationFromToken();
            if (user == null || string.IsNullOrEmpty(user.UserId))
            {
                return BadRequest("Invalid User.");
            }
            if (string.IsNullOrWhiteSpace(serviceId))
            {
                return Ok(new ResponseModel<string>(ResponseStatus.NotFound, null, "Invalid service."));
            }
            var result = await _orderBusiness.ConfirmOrder(serviceId, user);
            if (!result)
            {
                return Ok(new ResponseModel<string>(ResponseStatus.NotFound, null, "Service not found"));
            }
            return Ok(new ResponseModel<string>(ResponseStatus.Ok, null, "Order has been confirmed."));
        }
        [HttpGet]
        [Route("v1/order/services")]
        [Authorize]
        public async Task<IHttpActionResult> GetOrderServices()
        {
            var orderServices = await _orderServiceBusiness.GetOrderServices();
            if (orderServices == null)
            {
                return Ok(new ResponseModel<string>(ResponseStatus.NotFound, null, "No order service."));
            }
            return Ok(new ResponseModel<List<string>>(ResponseStatus.Ok, orderServices, ""));
        }

        [HttpGet]
        [Route("v1/order/summary")]
        [Authorize]
        public async Task<IHttpActionResult> GetOrderSummary(string serviceId)
        {
            if (string.IsNullOrEmpty(serviceId))
            {
                return BadRequest("Invalid Service id.");
            }
            var summary = await _orderBusiness.GetOrderSummary(serviceId);
            if (summary == null)
            {
                return Ok(new ResponseModel<string>(ResponseStatus.NotFound, null, "No order found."));
            }
            return Ok(new ResponseModel<OrderSummaryViewModel>(ResponseStatus.Ok, summary, ""));
        }
    }
}
