using Dhobi.Api.Helpers;
using Dhobi.Api.Models;
using Dhobi.Business.Interface;
using Dhobi.Common;
using Dhobi.Core.Dobi.DbModels;
using Dhobi.Core.OrderModel.ViewModels;
using Dhobi.Core.UserModel.DbModels;
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
        private TokenGenerator _tokenGenerator;
        private ILocationService _locationService;
        public OrderController(IOrderBusiness orderBusiness, ILocationService locationService)
        {
            _orderBusiness = orderBusiness;
            _locationService = locationService;
            _tokenGenerator = new TokenGenerator();
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
        private Dobi GetDobiInformationFromToken()
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
        [Route("v1/order/zones")]
        [Authorize]
        public async Task<IHttpActionResult> GetAvailableZones()
        {
            var zones = await _locationService.GetAvailableActiveZones();
            if (zones == null)
            {
                return Ok(new ResponseModel<string>(ResponseStatus.NotFound, null, "No zone available."));
            }
            return Ok(new ResponseModel<List<string>>(ResponseStatus.Ok, zones, ""));
        }
        [HttpGet]
        [Route("v1/order/new")]
        [Authorize]
        public async Task<IHttpActionResult> GetNewOrderForDobi(string id)
        {
            var order = await _orderBusiness.GetNewOrderForDobi(id);
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

    }
}
