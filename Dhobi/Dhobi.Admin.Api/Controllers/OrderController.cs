using Dhobi.Admin.Api.Models;
using Dhobi.Business.Interface;
using Dhobi.Core;
using Dhobi.Core.OrderModel.ViewModels;
using Dhobi.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Dhobi.Admin.Api.Controllers
{
    [RoutePrefix("api")]
    public class OrderController : ApiController
    {
        private IOrderServiceBusiness _orderServiceBusiness;
        private IOrderBusiness _orderBusiness;
        private IOrderRepository _orderRepository;
        public OrderController(IOrderServiceBusiness orderServiceBusiness, IOrderBusiness orderBusiness, IOrderRepository orderRepository)
        {
            _orderServiceBusiness = orderServiceBusiness;
            _orderBusiness = orderBusiness;
            _orderRepository = orderRepository;
        }
        [HttpPost]
        [Route("v1/order/service")]
        [Authorize]
        public async Task<IHttpActionResult> AddOrderService(List<string> services)
        {
            if (services == null)
            {
                return BadRequest("Invalid service data.");
            }
            var response = await _orderServiceBusiness.AddService(services);
            return Ok(response);
        }
        [HttpGet]
        [Route("v1/orders")]
        [Authorize]
        public async Task<IHttpActionResult> GetAllOrders(string date, int skip = 0, int limit = 10)
        {
            if (skip < 0 || limit < 1 || string.IsNullOrWhiteSpace(date))
            {
                return BadRequest("Invalid pagination data.");
            }
            var response = await _orderBusiness.GetAllOrders(date, skip, limit);
            if(response == null)
            {
                return Ok(new GenericResponse<string>(false, null, "No order found."));
            }
            return Ok(new GenericResponse<List<OrderListItemViewModel>>(true, response));
        }
        [HttpPost]
        [Route("v1/orders/update")]
        [Authorize]
        public async Task<IHttpActionResult> UpdateOrderStatus(OrderStatusUpdateModel orders)
        {
            if (orders == null || orders.Orders == null || orders.Orders.Count < 1 )
            {
                return BadRequest("Invalid order data.");
            }
            var response = await _orderRepository.UpdateOrderStatus(orders.Orders, orders.UpdatedStatus);
            if (!response)
            {
                return Ok(new GenericResponse<string>(false, null, "No matching order found."));
            }
            return Ok(new GenericResponse<bool>(true, response));
        }
    }
}
