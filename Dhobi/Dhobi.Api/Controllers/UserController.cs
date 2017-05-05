﻿using Dhobi.Api.Helpers;
using Dhobi.Api.Models;
using Dhobi.Business.Interface;
using Dhobi.Common;
using Dhobi.Core;
using Dhobi.Core.Device.DbModels;
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
        private IPromoOfferBusiness _promoOfferBusiness;
        private IDeviceStatusBusiness _deviceStatusBusiness;
        private IDeviceStausRepository _deviceStatusRepository;
        private TokenGenerator _tokenGenerator;
        public UserController(IUserBusiness userBusiness, 
            IPromoOfferBusiness promoOfferBusiness,
            IDeviceStatusBusiness deviceStatusBusiness,
            IDeviceStausRepository deviceStatusRepository)
        {
            _userBusiness = userBusiness;
            _promoOfferBusiness = promoOfferBusiness;
            _deviceStatusBusiness = deviceStatusBusiness;
            _deviceStatusRepository = deviceStatusRepository;
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

        [Route("v1/user/login")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Login(string phone, bool isVerificationRequired = false)
        {
            if (string.IsNullOrWhiteSpace(phone))
            {
                return BadRequest("Invalid login data");
            }
            var loggedInUser = await _userBusiness.UserLogin(phone, isVerificationRequired);
            if (loggedInUser == null || string.IsNullOrWhiteSpace(loggedInUser.UserId))
            {
                return Ok(new ResponseModel<string>(ResponseStatus.NotFound, null, "User Not found"));
            }
            ValidatedUserResponse validatedUser = null;
            if (isVerificationRequired)
            {
                validatedUser = new ValidatedUserResponse(loggedInUser.Name, null, loggedInUser.UserId);
            }
            var token = _tokenGenerator.GenerateUserToken(loggedInUser);
            validatedUser = new ValidatedUserResponse(loggedInUser.Name, token, null);
            return Ok(new ResponseModel<ValidatedUserResponse>(ResponseStatus.Ok, validatedUser, ""));
        }
        [Route("v1/user/promotion")]
        [HttpGet]
        [Authorize]
        public async Task<IHttpActionResult> GetPromoOffer()
        {
            var promoOffer = await _promoOfferBusiness.GetPromoOfferForUser();
            if(promoOffer == null)
            {
                return Ok(new ResponseModel<string>(ResponseStatus.NotFound, null, "No promo offer available"));
            }
            var promo = new PromoOfferResponse
            {
                PromoText = promoOffer.Text,
                Amount = promoOffer.Amount
            };
            return Ok(new ResponseModel<PromoOfferResponse>(ResponseStatus.Ok, promo, ""));
        }

        [Authorize]
        [Route("v1/user/device/add")]
        [HttpPost]
        public async Task<IHttpActionResult> RegisterDevice(DeviceStatus status)
        {
            if (string.IsNullOrWhiteSpace(status.DeviceId))
            {
                return BadRequest("Invalid data.");
            }
            var user = GetUserInformationFromToken();
            if (string.IsNullOrEmpty(user.UserId))
            {
                return BadRequest("Invalid User.");
            }
            status.UserId = user.UserId;
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
            if (string.IsNullOrEmpty(user.UserId))
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
    }
}
