using Dhobi.Api.Helpers;
using Dhobi.Api.Models;
using Dhobi.Business.Interface;
using Dhobi.Common;
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
            var zone = _locationService.GetZoneFromAddress(order.Latitude, order.Longitude, order.Address);
            if(zone == null || string.IsNullOrEmpty(zone))
            {
                return Ok(new ResponseModel<string>(ResponseStatus.BadRequest, null, "Service is not available for this address."));
            }
            var response = await _orderBusiness.AddNewOrder(order, user, zone);
            if (!response)
            {
                return Ok(new ResponseModel<string>(ResponseStatus.BadRequest, null, "Order was not processed successfully."));
            }
            return Ok(new ResponseModel<string>(ResponseStatus.Ok, null, "Order placed successfully."));
        }
    }
}
