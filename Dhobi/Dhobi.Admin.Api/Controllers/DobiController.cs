using Dhobi.Admin.Api.Helpers;
using Dhobi.Business.Interface;
using Dhobi.Core.Dobi.ViewModels;
using Dhobi.Core.Manager.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Dhobi.Admin.Api.Controllers
{
    [RoutePrefix("api/dobi")]
    public class DobiController : ApiController
    {
        private IDobiBusiness _dobiBusiness;
        private TokenGenerator _tokenGenerator;
        public DobiController(IDobiBusiness dobiBusiness)
        {
            _dobiBusiness = dobiBusiness;
            _tokenGenerator = new TokenGenerator();
        }
        private ManagerBasicInformation GetManagerInformationFromToken()
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
        //[HttpPost]
        //[Route("v1")]
        //[Authorize(Roles = "Admin,Superadmin")]
        //public async Task<IHttpActionResult> AddDobi(DobiViewModel dobiModel)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest("Invalid data.");
        //    }
        //    var addedBy = GetManagerInformationFromToken();
        //    if (addedBy == null)
        //    {
        //        return BadRequest("Invalid admin token.");
        //    }
        //    var response = await _managerBusiness.AddManager(manager, addedBy);
        //    return Ok(response);
        //}
    }
}
