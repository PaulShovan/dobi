using Dhobi.Api.Helpers;
using Dhobi.Api.Models;
using Dhobi.Business.Interface;
using Dhobi.Common;
using Dhobi.Repository.Interface;
using System.Threading.Tasks;
using System.Web.Http;

namespace Dhobi.Api.Controllers
{
    [RoutePrefix("api")]
    public class DobiController : ApiController
    {
        private IDobiBusiness _dobiBusiness;
        private IDobiRepository _dobiRepository;
        private TokenGenerator _tokenGenerator;
        public DobiController(IDobiBusiness dobiBusiness, IDobiRepository dobiRepository)
        {
            _dobiBusiness = dobiBusiness;
            _dobiRepository = dobiRepository;
            _tokenGenerator = new TokenGenerator();
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
            var dobiLoginResponse = new DobiLoginResponse
            {
                Token = token,
                Name = loggedInDobi.Name,
                Photo = loggedInDobi.Photo,
                DobiId = loggedInDobi.DobiId,
                Phone = loggedInDobi.Phone,
                IcNumber = loggedInDobi.IcNumber,
                DrivingLicense = loggedInDobi.DrivingLicense
            };
            var response = new ResponseModel<DobiLoginResponse>(ResponseStatus.Ok, dobiLoginResponse);
            return Ok(response);
        }
    }
}
