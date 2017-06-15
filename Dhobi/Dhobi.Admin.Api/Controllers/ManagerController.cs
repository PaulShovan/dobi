using Dhobi.Admin.Api.Helpers;
using Dhobi.Admin.Api.Models;
using Dhobi.Business.Interface;
using Dhobi.Common;
using Dhobi.Core;
using Dhobi.Core.Manager.DbModels;
using Dhobi.Core.Manager.ViewModels;
using Dhobi.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Security;

namespace Dhobi.Admin.Api.Controllers
{
    [RoutePrefix("api")]
    public class ManagerController : ApiController
    {
        private IManagerBusiness _managerBusiness;
        private IManagerRepository _managerRepository;
        private TokenGenerator _tokenGenerator;
        private StorageService _storageService;
        public ManagerController(IManagerBusiness managerBusiness, IManagerRepository managerRepository)
        {
            _managerBusiness = managerBusiness;
            _managerRepository = managerRepository;
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
        [Route("v1/manager")]
        [HttpGet]
        [Authorize(Roles = "Admin,Superadmin")]
        public async Task<IHttpActionResult> GetManager(int skip = 0, int limit = 10)
        {
            if (skip < 0 || limit < 1)
            {
                return BadRequest("Invalid pagination data.");
            }
            var managers = await _managerRepository.GetManager(skip, limit);
            var totalManager = await _managerRepository.GetManagerCount();
            var response = new ManagerListResponse
            {
                ManagerList = managers,
                TotalManager = totalManager
            };
            return Ok(new GenericResponse<ManagerListResponse>(true, response));
        }
        [Route("v1/manager")]
        [HttpGet]
        [Authorize(Roles = "Admin,Superadmin")]
        public async Task<IHttpActionResult> GetManagerById(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return BadRequest("Invalid manager id.");
            }
            var manager = await _managerRepository.GetManagerById(userId);
            if (manager == null)
            {
                return Ok(new GenericResponse<string>(false, null, "No manager found"));
            }
            return Ok(new GenericResponse<Manager>(true, manager));
        }
        [Route("v1/manager/{id}")]
        [HttpDelete]
        [Authorize(Roles = "Admin,Superadmin")]
        public async Task<IHttpActionResult> RemoveManager(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return Ok(new GenericResponse<string>(false, null, "Invalid manager id."));
            }
            var isRemoved = await _managerRepository.RemoveManager(id);
            if (!isRemoved)
            {
                return Ok(new GenericResponse<string>(false, null, "Invalid manager id."));
            }
            return Ok(new GenericResponse<string>(true, null, "Manager removed successfully."));
        }
        [HttpPost]
        [Route("v1/manager/update")]
        [Authorize(Roles = "Admin,Superadmin")]
        public async Task<IHttpActionResult> UpdateManager()
        {
            if (!Request.Content.IsMimeMultipartContent("form-data"))
            {
                this.Request.CreateResponse(HttpStatusCode.UnsupportedMediaType);
            }
            var addedBy = GetManagerInformationFromToken();
            if (addedBy == null)
            {
                return BadRequest("Invalid admin token.");
            }
            var manager = new Manager();
            string s3Prefix = ConfigurationManager.AppSettings["S3Prefix"];
            var provider = await Request.Content.ReadAsMultipartAsync(new InMemoryMultipartStreamProvider());

            foreach (var key in provider.FormData.AllKeys)
            {
                foreach (var val in provider.FormData.GetValues(key))
                {
                    if (key == "UserId")
                    {
                        manager.UserId = val.ToString().Trim();
                        if (string.IsNullOrWhiteSpace(manager.UserId))
                        {
                            return BadRequest("Manager Id Is Required.");
                        }
                    }
                    else if (key == "Name")
                    {
                        manager.Name = val.ToString().Trim();
                        if (string.IsNullOrWhiteSpace(manager.Name))
                        {
                            return BadRequest("Name Is Required");
                        }
                    }
                    else if (key == "UserName")
                    {
                        manager.UserName = val.ToString().Trim();
                        if (string.IsNullOrWhiteSpace(manager.UserName))
                        {
                            return BadRequest("User Name Is Required");
                        }
                    }
                    else if (key == "Phone")
                    {
                        manager.Phone = val.ToString().Trim();
                        if (string.IsNullOrWhiteSpace(manager.Phone))
                        {
                            return BadRequest("Phone Number Is Required");
                        }
                    }
                    else if (key == "Email")
                    {
                        manager.Email = val.ToString().Trim();
                        if (string.IsNullOrWhiteSpace(manager.Email))
                        {
                            return BadRequest("Email Is Required");
                        }
                    }
                    else if (key == "Address")
                    {
                        manager.Address = val.ToString().Trim();
                    }
                    else if (key == "EmergencyContactNumber")
                    {
                        manager.EmergencyContactNumber = val.ToString().Trim();
                        if (string.IsNullOrWhiteSpace(manager.EmergencyContactNumber))
                        {
                            return BadRequest("Emergency Contact Number Is Required");
                        }
                    }
                    else if (key == "PassportNumber")
                    {
                        manager.PassportNumber = val.ToString().Trim();
                    }
                    else if (key == "IcNumber")
                    {
                        manager.IcNumber = val.ToString().Trim();
                    }
                    else if (key == "DrivingLicense")
                    {
                        manager.DrivingLicense = val.ToString().Trim();
                    }
                    else if (key == "Age")
                    {
                        manager.Age = int.Parse(val.ToString().Trim());
                        if (manager.Age < 0)
                        {
                            return BadRequest("Invalid Age");
                        }
                    }
                    else if (key == "Sex")
                    {
                        manager.Sex = val.ToString().Trim();
                    }
                    else if (key == "Salary")
                    {
                        manager.Salary = double.Parse(val.ToString().Trim());
                    }
                    else if (key == "Roles")
                    {
                        var roles = val.ToString().Trim();
                        if (string.IsNullOrWhiteSpace(roles))
                        {
                            return BadRequest("Role is required.");
                        }
                        manager.Roles = roles.Split(',').ToList();
                    }
                    else if (key == "Password")
                    {
                        manager.Password = val.ToString().Trim();
                        if (string.IsNullOrWhiteSpace(manager.Password))
                        {
                            return BadRequest("Password is required.");
                        }
                    }
                    else if (key == "Photo")
                    {
                        manager.Photo = val.ToString().Trim();
                    }
                }
            }
            if (provider.Files != null && provider.Files.Count > 0)
            {
                _storageService = new StorageService();
                foreach (var file in provider.Files)
                {
                    var photoUrl = manager.UserId + "/profile/" + "profile_pic.png";
                    Stream stream = await file.ReadAsStreamAsync();
                    _storageService.UploadFile("dhobi", photoUrl, stream);
                    manager.Photo = s3Prefix + photoUrl;
                }
            }
            var response = await _managerBusiness.UpdateManager(manager);
            return Ok(response);
        }
        [HttpPost]
        [Route("v1/manager")]
        [Authorize(Roles = "Admin,Superadmin")]
        public async Task<IHttpActionResult> AddManager()
        {
            if (!Request.Content.IsMimeMultipartContent("form-data"))
            {
                this.Request.CreateResponse(HttpStatusCode.UnsupportedMediaType);
            }
            var addedBy = GetManagerInformationFromToken();
            if (addedBy == null)
            {
                return BadRequest("Invalid admin token.");
            }
            var manager = new Manager();
            manager.UserId = Guid.NewGuid().ToString();
            manager.AddedBy = addedBy;
            string s3Prefix = ConfigurationManager.AppSettings["S3Prefix"];
            var provider = await Request.Content.ReadAsMultipartAsync(new InMemoryMultipartStreamProvider());

            foreach (var key in provider.FormData.AllKeys)
            {
                foreach (var val in provider.FormData.GetValues(key))
                {
                    if (key == "Name")
                    {
                        manager.Name = val.ToString().Trim();
                        if (string.IsNullOrWhiteSpace(manager.Name))
                        {
                            return BadRequest("Name Is Required");
                        }
                    }
                    else if (key == "UserName")
                    {
                        manager.UserName = val.ToString().Trim();
                        if (string.IsNullOrWhiteSpace(manager.UserName))
                        {
                            return BadRequest("User Name Is Required");
                        }
                    }
                    else if (key == "Phone")
                    {
                        manager.Phone = val.ToString().Trim();
                        if (string.IsNullOrWhiteSpace(manager.Phone))
                        {
                            return BadRequest("Phone Number Is Required");
                        }
                    }
                    else if (key == "Email")
                    {
                        manager.Email = val.ToString().Trim();
                        if (string.IsNullOrWhiteSpace(manager.Email))
                        {
                            return BadRequest("Email Is Required");
                        }
                    }
                    else if (key == "Address")
                    {
                        manager.Address = val.ToString().Trim();
                    }
                    else if (key == "EmergencyContactNumber")
                    {
                        manager.EmergencyContactNumber = val.ToString().Trim();
                        if (string.IsNullOrWhiteSpace(manager.EmergencyContactNumber))
                        {
                            return BadRequest("Emergency Contact Number Is Required");
                        }
                    }
                    else if (key == "PassportNumber")
                    {
                        manager.PassportNumber = val.ToString().Trim();
                    }
                    else if (key == "IcNumber")
                    {
                        manager.IcNumber = val.ToString().Trim();
                    }
                    else if (key == "DrivingLicense")
                    {
                        manager.DrivingLicense = val.ToString().Trim();
                    }
                    else if (key == "Age")
                    {
                        manager.Age = int.Parse(val.ToString().Trim());
                        if (manager.Age < 0)
                        {
                            return BadRequest("Invalid Age");
                        }
                    }
                    else if (key == "Sex")
                    {
                        manager.Sex = val.ToString().Trim();
                    }
                    else if (key == "Salary")
                    {
                        manager.Salary = double.Parse(val.ToString().Trim());
                    }
                    else if (key == "Roles")
                    {
                        var roles = val.ToString().Trim();
                        if (string.IsNullOrWhiteSpace(roles))
                        {
                            return BadRequest("Role is required.");
                        }
                        manager.Roles = roles.Split(',').ToList();
                    }
                    else if (key == "Password")
                    {
                        manager.Password = val.ToString().Trim();
                        if (string.IsNullOrWhiteSpace(manager.Password))
                        {
                            return BadRequest("Password is required.");
                        }
                    }
                }
            }
            _storageService = new StorageService();
            foreach (var file in provider.Files)
            {
                var photoUrl = manager.UserId + "/profile/" + "profile_pic.png";
                Stream stream = await file.ReadAsStreamAsync();
                _storageService.UploadFile("dhobi", photoUrl, stream);
                manager.Photo = s3Prefix + photoUrl;
            }
            var response = await _managerBusiness.AddManager(manager);
            return Ok(response);
        }
        [Authorize]
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
            if (loggedInUser == null || string.IsNullOrWhiteSpace(loggedInUser.UserId))
            {
                var notFound = new GenericResponse<ManagerLoginResponse>(false, null, "Invalid Username or Password!");
                return Ok(notFound);
            }
            var token = _tokenGenerator.GenerateUserToken(loggedInUser);
            var managerLoginResponse = new ManagerLoginResponse
            {
                Token = token,
                Name = loggedInUser.Name,
                Role = loggedInUser.Roles[0]
            };
            var response = new GenericResponse<ManagerLoginResponse>(true, managerLoginResponse);
            return Ok(response);
        }
    }
}
