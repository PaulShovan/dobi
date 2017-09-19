using Dhobi.Api.Helpers;
using Dhobi.Api.Models;
using Dhobi.Business.Interface;
using Dhobi.Common;
using Dhobi.Core;
using Dhobi.Core.AvailableLoacation;
using Dhobi.Core.AvailableLoacation.DbModels;
using Dhobi.Core.Device.DbModels;
using Dhobi.Core.Location.DbModels;
using Dhobi.Core.UserInbox.DbModels;
using Dhobi.Core.UserInbox.ViewModels;
using Dhobi.Core.UserModel.DbModels;
using Dhobi.Core.UserModel.ViewModels;
using Dhobi.Repository.Interface;
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
        private IDeviceStatusBusiness _deviceStatusBusiness;
        private IDeviceStausRepository _deviceStatusRepository;
        private IUserMessageBusiness _userMessageBusiness;
        private IAvailableLocationBusiness _availableLocationBusiness;
        private IUserLocationBusiness _userLocationBusiness;
        private IUserLocationRepository _userLocationRepository;
        private TokenGenerator _tokenGenerator;
        public UserController(IUserBusiness userBusiness, 
            IDeviceStatusBusiness deviceStatusBusiness,
            IDeviceStausRepository deviceStatusRepository,
            IUserMessageBusiness userMessageBusiness,
            IAvailableLocationBusiness availableLocationBusiness,
            IUserLocationBusiness userLocationBusiness,
            IUserLocationRepository userLocationRepository)
        {
            _userBusiness = userBusiness;
            _deviceStatusBusiness = deviceStatusBusiness;
            _deviceStatusRepository = deviceStatusRepository;
            _userMessageBusiness = userMessageBusiness;
            _availableLocationBusiness = availableLocationBusiness;
            _userLocationBusiness = userLocationBusiness;
            _userLocationRepository = userLocationRepository;
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
            var isPhoneAvailable = await _userBusiness.IsPhoneNumberAvailable(user.PhoneNumber);
            if (!isPhoneAvailable)
            {
                return Ok(new ResponseModel<string>(ResponseStatus.BadRequest, null, "Phone number is not available."));
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
                return Ok(new ResponseModel<string>(ResponseStatus.NotFound, null, "Invalid verification code."));
            }
            var token = _tokenGenerator.GenerateUserToken(response);
            var validatedUser = new ValidatedUserResponse(response.Name, token);
            return Ok(new ResponseModel<ValidatedUserResponse>(ResponseStatus.Ok, validatedUser, "User has been authenticated successfully."));
        }

        [Route("v1/user/login")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Login(LoginViewModel login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid login data");
            }
            var loggedInUser = await _userBusiness.UserLogin(login.Phone, login.IsVerificationRequired);
            if (loggedInUser == null || string.IsNullOrWhiteSpace(loggedInUser.UserId))
            {
                return Ok(new ResponseModel<string>(ResponseStatus.NotFound, null, "User Not found"));
            }
            ValidatedUserResponse validatedUser = null;
            if (login.IsVerificationRequired)
            {
                validatedUser = new ValidatedUserResponse(loggedInUser.Name, null, loggedInUser.UserId);
                return Ok(new ResponseModel<ValidatedUserResponse>(ResponseStatus.Ok, validatedUser, ""));
            }
            var token = _tokenGenerator.GenerateUserToken(loggedInUser);
            validatedUser = new ValidatedUserResponse(loggedInUser.Name, token, null);
            return Ok(new ResponseModel<ValidatedUserResponse>(ResponseStatus.Ok, validatedUser, ""));
        }
        [Route("v1/user/availablelocation")]
        [HttpGet]
        [Authorize]
        public async Task<IHttpActionResult> GetAvailableLocations()
        {
            var locations = await _availableLocationBusiness.GetLocationByStatus((int)LocationStatus.Active);
            if (locations == null)
            {
                return Ok(new ResponseModel<string>(ResponseStatus.NotFound, null, "No location available"));
            }
            return Ok(new ResponseModel<List<Location>>(ResponseStatus.Ok, locations, ""));
        }

        [Authorize]
        [Route("v1/user/device/add")]
        [HttpPost]
        public async Task<IHttpActionResult> RegisterDevice(DeviceStatus status)
        {
            if (string.IsNullOrWhiteSpace(status.RegistrationId))
            {
                return BadRequest("Invalid data.");
            }
            var user = GetUserInformationFromToken();
            if (string.IsNullOrEmpty(user.UserId))
            {
                return BadRequest("Invalid User.");
            }
            status.UserId = user.UserId;
            status.Application = (int)Application.DobiUser;
            var registerAck = await _deviceStatusBusiness.RegisterDevice(status);
            if (!registerAck)
            {
                return Ok(new ResponseModel<string>(ResponseStatus.BadRequest, "Device could not be added.", ""));
            }
            return Ok(new ResponseModel<string>(ResponseStatus.Ok, "Device added successfully.", ""));
        }
        [Authorize]
        [Route("v1/user/device/remove")]
        [HttpPost]
        public async Task<IHttpActionResult> RemoveDevice(DeviceStatus status)
        {
            if (string.IsNullOrWhiteSpace(status.AppId))
            {
                return BadRequest("Invalid data.");
            }
            var user = GetUserInformationFromToken();
            if (user == null || string.IsNullOrEmpty(user.UserId))
            {
                return BadRequest("Invalid User.");
            }
            status.UserId = user.UserId;
            var registerAck = await _deviceStatusRepository.RemoveDeviceStatus(status);
            if (!registerAck)
            {
                return Ok(new ResponseModel<string>(ResponseStatus.BadRequest, "Device could not be removed.", ""));
            }
            return Ok(new ResponseModel<string>(ResponseStatus.Ok, "Device removes successfully.", ""));
        }

        [Authorize]
        [Route("v1/user/message")]
        [HttpGet]
        public async Task<IHttpActionResult> GetUserMessage(int skip = 0, int limit = 10)
        {
            var user = GetUserInformationFromToken();
            if(user == null || string.IsNullOrWhiteSpace(user.UserId))
            {
                return Ok(new ResponseModel<string>(ResponseStatus.BadRequest, null, "Invalid user."));
            }
            var messages = await _userMessageBusiness.GetUserMessage(user.UserId, skip, limit);
            return Ok(new ResponseModel<UserMessageListViewModel>(ResponseStatus.Ok, messages, ""));
        }
        [Authorize]
        [Route("v1/user/message/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetMessageById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return Ok(new ResponseModel<string>(ResponseStatus.BadRequest, null, "Invalid message id."));
            }
            var user = GetUserInformationFromToken();
            if (user == null || string.IsNullOrWhiteSpace(user.UserId))
            {
                return Ok(new ResponseModel<string>(ResponseStatus.BadRequest, null, "Invalid user."));
            }
            var message = await _userMessageBusiness.GetMessageById(id);
            return Ok(new ResponseModel<UserMessageBasicInformation>(ResponseStatus.Ok, message, ""));
        }

        [Authorize]
        [Route("v1/user/order/acknowledge")]
        [HttpGet]
        public async Task<IHttpActionResult> GetOrderAcknowledge(string messageId, string serviceId)
        {
            if (string.IsNullOrWhiteSpace(messageId) || string.IsNullOrWhiteSpace(serviceId))
            {
                return Ok(new ResponseModel<string>(ResponseStatus.BadRequest, null, "Invalid message or service."));
            }
            var user = GetUserInformationFromToken();
            if (user == null || string.IsNullOrWhiteSpace(user.UserId))
            {
                return Ok(new ResponseModel<string>(ResponseStatus.BadRequest, null, "Invalid user."));
            }
            var ack = await _userMessageBusiness.GetOrderAcknowledge(messageId, serviceId);
            if(ack == null)
            {
                return Ok(new ResponseModel<string>(ResponseStatus.BadRequest, null, "Service has not been acknowledged yet."));
            }
            return Ok(new ResponseModel<UserAcknowledgeMessageViewModel>(ResponseStatus.Ok, ack, ""));
        }


        [Authorize]
        [Route("v1/user/validate/resend")]
        [HttpGet]
        public async Task<IHttpActionResult> ResendVerificationCode()
        {
            var user = GetUserInformationFromToken();
            if (user == null || string.IsNullOrWhiteSpace(user.UserId))
            {
                return Ok(new ResponseModel<string>(ResponseStatus.BadRequest, null, "Invalid user."));
            }
            var message = await _userBusiness.SendUserSms(user, SmsType.NewLogin);
            if (!message)
            {
                return Ok(new ResponseModel<string>(ResponseStatus.BadRequest, "Error sending message", ""));
            }
            return Ok(new ResponseModel<string>(ResponseStatus.Ok, "Message has been sent", ""));
        }
        [Authorize]
        [Route("v1/user/location")]
        [HttpPost]
        public async Task<IHttpActionResult> AddLocation(UserLocation location)
        {
            var user = GetUserInformationFromToken();
            if (user == null || string.IsNullOrWhiteSpace(user.UserId))
            {
                return Ok(new ResponseModel<string>(ResponseStatus.BadRequest, null, "Invalid user."));
            }
            else if (!_userLocationBusiness.ValidateUserLocation(location))
            {
                return Ok(new ResponseModel<string>(ResponseStatus.BadRequest, null, "Invalid location data."));
            }
            location.UserId = user.UserId;
            var success = await _userLocationRepository.AddLocation(location);
            if (!success)
            {
                return Ok(new ResponseModel<string>(ResponseStatus.BadRequest, null, "Error adding user location"));
            }
            return Ok(new ResponseModel<string>(ResponseStatus.Ok, "Location added successfully.", "Location added successfully."));
        }
        [Authorize]
        [Route("v1/user/location")]
        [HttpGet]
        public async Task<IHttpActionResult> GetLocation()
        {
            var user = GetUserInformationFromToken();
            if (user == null || string.IsNullOrWhiteSpace(user.UserId))
            {
                return Ok(new ResponseModel<string>(ResponseStatus.BadRequest, null, "Invalid user."));
            }
            var locations = await _userLocationRepository.GetUserLocation(user.UserId);
            return Ok(new ResponseModel<List<UserLocation>>(ResponseStatus.Ok, locations, ""));
        }
    }
}
