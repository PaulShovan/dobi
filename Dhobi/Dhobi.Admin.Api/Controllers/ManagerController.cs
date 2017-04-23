using Dhobi.Admin.Api.Helpers;
using Dhobi.Admin.Api.Models;
using Dhobi.Business.Interface;
using Dhobi.Common;
using Dhobi.Core;
using Dhobi.Core.Manager.DbModels;
using Dhobi.Core.Manager.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Dhobi.Admin.Api.Controllers
{
    [RoutePrefix("api")]
    public class ManagerController : ApiController
    {
        private IManagerBusiness _managerBusiness;
        private TokenGenerator _tokenGenerator;
        public ManagerController(IManagerBusiness managerBusiness)
        {
            _managerBusiness = managerBusiness;
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
        //[Authorize]
        [HttpPost]
        [Route("v1/manager")]
        [Authorize(Roles = "Superadmin")]
        public async Task<IHttpActionResult> AddManager(ManagerViewModel manager)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data.");
            }
            var addedBy = GetManagerInformationFromToken();
            if (addedBy == null)
            {
                return BadRequest("Invalid admin token.");
            }
            var response = await _managerBusiness.AddManager(manager, addedBy);
            return Ok(response);
        }

        //[Authorize]
        
        [Route("v1/manager/validity/email")]
        [HttpGet]
        public async Task<IHttpActionResult> CheckEmailAvailability(string email)
        {
            if (!Utilities.IsValidEmailAddress(email))
            {
                return BadRequest("Invalid email address.");
            }
            var response = await _managerBusiness.IsEmailAvailable(email);
            return Ok(response);
        }

        [Route("v1/manager/validity/username")]
        [HttpGet]
        public async Task<IHttpActionResult> CheckUserNameAvailability(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                return BadRequest("Invalid username.");
            }
            var response = await _managerBusiness.IsUserNameAvailable(userName);
            return Ok(response);
        }

        [Route("v1/manager/login")]
        [HttpPost]
        public async Task<IHttpActionResult> Login(LoginViewModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid login data");
            }
            var loggedInUser = await _managerBusiness.ManagerLogin(loginModel);
            if (string.IsNullOrWhiteSpace(loggedInUser.UserId))
            {
                return NotFound();
            }
            var token = _tokenGenerator.GenerateUserToken(loggedInUser);
            var managerLoginResponse = new ManagerLoginResponse
            {
                Token = token,
                Name = loggedInUser.Name
            };
            var response = new GenericResponse<ManagerLoginResponse>(true, managerLoginResponse);
            return Ok(response);
        }
    }
}
