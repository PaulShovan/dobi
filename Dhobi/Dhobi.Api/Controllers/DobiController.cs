using Dhobi.Api.Helpers;
using Dhobi.Api.Models;
using Dhobi.Business.Interface;
using Dhobi.Common;
using Dhobi.Core.Dobi.DbModels;
using Dhobi.Core.Dobi.ViewModels;
using Dhobi.Core.UserInbox.ViewModels;
using Dhobi.Repository.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Dhobi.Api.Controllers
{
    [RoutePrefix("api")]
    public class DobiController : ApiController
    {
        private IDobiBusiness _dobiBusiness;
        private IDobiRepository _dobiRepository;
        private IUserMessageBusiness _userMessageBusiness;
        private TokenGenerator _tokenGenerator;
        public DobiController(IDobiBusiness dobiBusiness, IDobiRepository dobiRepository, IUserMessageBusiness userMessageBusiness)
        {
            _dobiBusiness = dobiBusiness;
            _dobiRepository = dobiRepository;
            _userMessageBusiness = userMessageBusiness;
            _tokenGenerator = new TokenGenerator();
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
        [Route("v1/dobi/login")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Login(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
            {
                return BadRequest("Invalid login data");
            }
            var loggedInDobi = await _dobiRepository.DobiLogin(phone);
            if (loggedInDobi == null || string.IsNullOrWhiteSpace(loggedInDobi.DobiId))
            {
                return Ok(new ResponseModel<string>(ResponseStatus.NotFound, null, "Dobi Not found"));
            }
            var token = _tokenGenerator.GenerateDobiToken(loggedInDobi);
            return Ok(new ResponseModel<string>(ResponseStatus.Ok, token, ""));
        }
        [Route("v1/dobi/home")]
        [HttpGet]
        [Authorize]
        public async Task<IHttpActionResult> GetDobiHomePageInformation()
        {
            var dobi = GetDobiInformationFromToken();
            if(dobi == null || string.IsNullOrWhiteSpace(dobi.DobiId))
            {
                return BadRequest("Invalid User.");
            }
            var response = await _dobiBusiness.GetDobiHomePageResponse(dobi.DobiId);
            if (response == null)
            {
                return Ok(new ResponseModel<string>(ResponseStatus.NotFound, null, "Dobi Not found"));
            }
            return Ok(new ResponseModel<DobiHomePageResponse>(ResponseStatus.Ok, response, ""));
        }
        [Authorize]
        [Route("v1/dobi/message")]
        [HttpGet]
        public async Task<IHttpActionResult> GetDobiMessage(int skip = 0, int limit = 10)
        {
            var user = GetDobiInformationFromToken();
            if (user == null || string.IsNullOrWhiteSpace(user.DobiId))
            {
                return Ok(new ResponseModel<string>(ResponseStatus.BadRequest, null, "Invalid user."));
            }
            var messages = await _userMessageBusiness.GetUserMessage(user.DobiId, skip, limit);
            return Ok(new ResponseModel<UserMessageListViewModel>(ResponseStatus.Ok, messages, ""));
        }
    }
}
