using Dhobi.Business.Interface;
using Dhobi.Core.PromoOffer.ViewModels;
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
    public class PromoOfferController : ApiController
    {
        private IPromoOfferBusiness _promoOfferBusiness;
        public PromoOfferController(IPromoOfferBusiness promoOfferBusiness)
        {
            _promoOfferBusiness = promoOfferBusiness;
        }
        [HttpPost]
        [Route("v1/promo")]
        [Authorize]
        public async Task<IHttpActionResult> AddPromoOffer(List<PromoViewModel> promoOffers)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid promo offer");
            }
            var response = await _promoOfferBusiness.AddPromoOffer(promoOffers);
            return Ok(response);
        }
    }
}
