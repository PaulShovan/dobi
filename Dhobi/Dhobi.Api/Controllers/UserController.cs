using Dhobi.Api.Helpers;
using Dhobi.Api.Models;
using Dhobi.Business.Interface;
using Dhobi.Common;
using Dhobi.Core;
using Dhobi.Core.UserModel.DbModels;
using Dhobi.Core.UserModel.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Dhobi.Api.Controllers
{
    [RoutePrefix("api")]
    public class UserController : ApiController
    {
        private IUserBusiness _userBusiness;
        private TokenGenerator _tokenGenerator;
        public UserController(IUserBusiness userBusiness)
        {
            _userBusiness = userBusiness;
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
        [Route("v1/user")]
        public async Task<IHttpActionResult> AddUser(UserViewModel user)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new ResponseModel<string>(ResponseStatus.BadRequest, null, "Invalid user data."));
            }
            var response = await _userBusiness.AddUser(user);
            if(response == null)
            {
                return Ok(new ResponseModel<string>(ResponseStatus.BadRequest, null, "Invalid user data."));
            }
            return Ok(new ResponseModel<string>(ResponseStatus.Ok, response.UserId, "User added successfully."));
        }

        [HttpGet]
        [Route("v1/user/validate")]
        public async Task<IHttpActionResult> ValidateRegisteredUser(string code, string userId)
        {
            if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(userId))
            {
                return BadRequest("Invalid data.");
            }
            var response = await _userBusiness.ValidateRegisteredUser(code, userId);
            if (response == null)
            {
                return Ok(new ResponseModel<string>(ResponseStatus.NotFound,null, "Invalid user."));
            }
            var token = _tokenGenerator.GenerateUserToken(response);
            var validatedUser = new ValidatedUserResponse(response.Name, token);
            return Ok(new ResponseModel<ValidatedUserResponse>(ResponseStatus.Ok, validatedUser, "User has been authenticated successfully."));
        }

    }
}
