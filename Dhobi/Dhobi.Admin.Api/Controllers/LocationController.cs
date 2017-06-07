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
    public class LocationController : ApiController
    {
        private IAvailableLocationBusiness _availableLocationBusiness;
        public LocationController(IAvailableLocationBusiness availableLocationBusiness)
        {
            _availableLocationBusiness = availableLocationBusiness;
        }
        [HttpPost]
        [Route("v1/location")]
        [Authorize]
        public async Task<IHttpActionResult> AddLocation(List<string> locations)
        {
            if(locations == null)
            {
                return BadRequest("Invalid location data.");
            }
            var response = await _availableLocationBusiness.AddAvailableLocation(locations);
            return Ok(response);
        }
    }
}
