using Dhobi.Business.Interface;
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
        public OrderController(IOrderServiceBusiness orderServiceBusiness)
        {
            _orderServiceBusiness = orderServiceBusiness;
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
    }
}
