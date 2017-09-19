using Dhobi.Admin.Api.Helpers;
using Dhobi.Admin.Api.Models;
using Dhobi.Business.Interface;
using Dhobi.Common;
using Dhobi.Core;
using Dhobi.Core.Dobi.DbModels;
using Dhobi.Core.Manager.DbModels;
using Dhobi.Repository.Interface;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Dhobi.Admin.Api.Controllers
{
    [RoutePrefix("api")]
    public class DobiController : ApiController
    {
        private IDobiBusiness _dobiBusiness;
        private IDobiRepository _dobiRepository;
        private TokenGenerator _tokenGenerator;
        private StorageService _storageService;
        public DobiController(IDobiBusiness dobiBusiness, IDobiRepository dobiRepository)
        {
            _dobiBusiness = dobiBusiness;
            _dobiRepository = dobiRepository;
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
        [Route("v1/dobi/validity/email")]
        [HttpGet]
        [Authorize(Roles = "Admin,Superadmin")]
        public async Task<IHttpActionResult> CheckEmailAvailability(string email)
        {
            if (!Utilities.IsValidEmailAddress(email))
            {
                return BadRequest("Invalid email address.");
            }
            var response = await _dobiRepository.IsEmailAvailable(email);
            return Ok(new GenericResponse<bool>(true, response));
        }
        [Route("v1/dobi/validity/phone")]
        [HttpGet]
        [Authorize(Roles = "Admin,Superadmin")]
        public async Task<IHttpActionResult> CheckPhoneNumberAvailability(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
            {
                return BadRequest("Invalid phone number.");
            }
            var response = await _dobiRepository.IsPhoneNumberAvailable(phone);
            return Ok(new GenericResponse<bool>(true, response));
        }
        [Route("v1/dobi/validity/passport")]
        [HttpGet]
        [Authorize(Roles = "Admin,Superadmin")]
        public async Task<IHttpActionResult> CheckPassportAvailability(string passport)
        {
            if (string.IsNullOrWhiteSpace(passport))
            {
                return BadRequest("Invalid passport number.");
            }
            var response = await _dobiRepository.IsPassportNumberAvailable(passport);
            return Ok(new GenericResponse<bool>(true, response));
        }
        [Route("v1/dobi/validity/icnumber")]
        [HttpGet]
        [Authorize(Roles = "Admin,Superadmin")]
        public async Task<IHttpActionResult> CheckIcNumberAvailability(string icnumber)
        {
            if (string.IsNullOrWhiteSpace(icnumber))
            {
                return BadRequest("Invalid ic number.");
            }
            var response = await _dobiRepository.IsIcNumberAvailable(icnumber);
            return Ok(new GenericResponse<bool>(true, response));
        }
        [Route("v1/dobi/validity/drivinglicense")]
        [HttpGet]
        [Authorize(Roles = "Admin,Superadmin")]
        public async Task<IHttpActionResult> CheckDrivingLicenseAvailability(string drivingLicense)
        {
            if (string.IsNullOrWhiteSpace(drivingLicense))
            {
                return BadRequest("Invalid driving license.");
            }
            var response = await _dobiRepository.IsDrivingLicenseAvailable(drivingLicense);
            return Ok(new GenericResponse<bool>(true, response));
        }
        [Route("v1/dobi")]
        [HttpGet]
        [Authorize(Roles = "Admin,Superadmin")]
        public async Task<IHttpActionResult> GetDobi(int skip=0, int limit=10)
        {
            if(skip < 0 || limit < 1)
            {
                return BadRequest("Invalid pagination data.");
            }
            var dobi = await _dobiRepository.GetDobi(skip, limit);
            var totalDobi = await _dobiRepository.GetDobiCount();
            var response = new DobiListResponse
            {
                DobiList = dobi,
                TotalDobi = totalDobi
            };
            return Ok(new GenericResponse<DobiListResponse>(true, response));
        }
        [Route("v1/dobi")]
        [HttpGet]
        [Authorize(Roles = "Admin,Superadmin")]
        public async Task<IHttpActionResult> GetDobiById(string dobiId)
        {
            if (string.IsNullOrWhiteSpace(dobiId))
            {
                return BadRequest("Invalid dobi id.");
            }
            var dobi = await _dobiRepository.GetDobiById(dobiId);
            if(dobi == null)
            {
                return Ok(new GenericResponse<string>(false, "", "No dobi found"));
            }
            return Ok(new GenericResponse<Dobi>(true, dobi));
        }
        [HttpPost]
        [Route("v1/dobi")]
        [Authorize(Roles = "Admin,Superadmin")]
        public async Task<IHttpActionResult> AddDobi()
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

            var dobi = new Dobi();
            dobi.DobiId = await _dobiBusiness.GenerateDobiId();
            dobi.AddedBy = addedBy;

            string s3Prefix = ConfigurationManager.AppSettings["S3Prefix"];
            var provider = await Request.Content.ReadAsMultipartAsync(new InMemoryMultipartStreamProvider());

            foreach (var key in provider.FormData.AllKeys)
            {
                foreach (var val in provider.FormData.GetValues(key))
                {
                    if (key == "Name")
                    {
                        dobi.Name = val.ToString().Trim();
                        if (string.IsNullOrWhiteSpace(dobi.Name))
                        {
                            return BadRequest("Name Is Required");
                        }
                    }
                    if (key == "Phone")
                    {
                        dobi.Phone = val.ToString().Trim();
                        if (string.IsNullOrWhiteSpace(dobi.Phone))
                        {
                            return BadRequest("Phone Number Is Required");
                        }
                    }
                    else if (key == "Email")
                    {
                        dobi.Email = val.ToString().Trim();
                        if (string.IsNullOrWhiteSpace(dobi.Email))
                        {
                            return BadRequest("Email Is Required");
                        }
                    }
                    else if (key == "Address")
                    {
                        dobi.Address = val.ToString().Trim();
                    }
                    else if (key == "EmergencyContactNumber")
                    {
                        dobi.EmergencyContactNumber = val.ToString().Trim();
                        if (string.IsNullOrWhiteSpace(dobi.EmergencyContactNumber))
                        {
                            return BadRequest("Emergency Contact Number Is Required");
                        }
                    }
                    else if (key == "PassportNumber")
                    {
                        dobi.PassportNumber = val.ToString().Trim();
                    }
                    else if (key == "IcNumber")
                    {
                        dobi.IcNumber = val.ToString().Trim();
                    }
                    else if (key == "DrivingLicense")
                    {
                        dobi.DrivingLicense = val.ToString().Trim();
                    }
                    else if (key == "Age")
                    {
                        dobi.Age = int.Parse(val.ToString().Trim());
                        if (dobi.Age < 0)
                        {
                            return BadRequest("Invalid Age");
                        }
                    }
                    else if (key == "Sex")
                    {
                        dobi.Sex = val.ToString().Trim();
                    }
                    else if (key == "Salary")
                    {
                        dobi.Salary = double.Parse(val.ToString().Trim());
                    }
                }
            }
            _storageService = new StorageService();
            foreach (var file in provider.Files)
            {
                var photoUrl = dobi.DobiId + "/profile/" + "profile_pic.png";
                Stream stream = await file.ReadAsStreamAsync();
                _storageService.UploadFile("dhobi-bucket", photoUrl, stream);
                dobi.Photo = s3Prefix + photoUrl;
            }
            var response = await _dobiBusiness.AddDobi(dobi);
            return Ok(response);
        }



        [HttpPost]
        [Route("v1/dobi/update")]
        [Authorize(Roles = "Admin,Superadmin")]
        public async Task<IHttpActionResult> UpdateDobi()
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
            var dobi = new Dobi();
            string s3Prefix = ConfigurationManager.AppSettings["S3Prefix"];
            var provider = await Request.Content.ReadAsMultipartAsync(new InMemoryMultipartStreamProvider());

            foreach (var key in provider.FormData.AllKeys)
            {
                foreach (var val in provider.FormData.GetValues(key))
                {
                    if (key == "DobiId")
                    {
                        dobi.DobiId = val.ToString().Trim();
                        if (string.IsNullOrWhiteSpace(dobi.DobiId))
                        {
                            return BadRequest("Dobi Id Is Required.");
                        }
                    }
                    else if (key == "Name")
                    {
                        dobi.Name = val.ToString().Trim();
                        if (string.IsNullOrWhiteSpace(dobi.Name))
                        {
                            return BadRequest("Name Is Required.");
                        }
                    }
                    else if (key == "Phone")
                    {
                        dobi.Phone = val.ToString().Trim();
                        if (string.IsNullOrWhiteSpace(dobi.Phone))
                        {
                            return BadRequest("Phone Number Is Required");
                        }
                    }
                    else if (key == "Email")
                    {
                        dobi.Email = val.ToString().Trim();
                        if (string.IsNullOrWhiteSpace(dobi.Email))
                        {
                            return BadRequest("Email Is Required");
                        }
                    }
                    else if (key == "Address")
                    {
                        dobi.Address = val.ToString().Trim();
                    }
                    else if (key == "EmergencyContactNumber")
                    {
                        dobi.EmergencyContactNumber = val.ToString().Trim();
                        if (string.IsNullOrWhiteSpace(dobi.EmergencyContactNumber))
                        {
                            return BadRequest("Emergency Contact Number Is Required");
                        }
                    }
                    else if (key == "PassportNumber")
                    {
                        dobi.PassportNumber = val.ToString().Trim();
                    }
                    else if (key == "IcNumber")
                    {
                        dobi.IcNumber = val.ToString().Trim();
                    }
                    else if (key == "DrivingLicense")
                    {
                        dobi.DrivingLicense = val.ToString().Trim();
                    }
                    else if (key == "Age")
                    {
                        dobi.Age = int.Parse(val.ToString().Trim());
                        if (dobi.Age < 0)
                        {
                            return BadRequest("Invalid Age");
                        }
                    }
                    else if (key == "Sex")
                    {
                        dobi.Sex = val.ToString().Trim();
                    }
                    else if (key == "Salary")
                    {
                        dobi.Salary = double.Parse(val.ToString().Trim());
                    }
                    else if(key == "Photo")
                    {
                        dobi.Photo = val.ToString().Trim();
                    }
                }
            }
            if(provider.Files != null && provider.Files.Count > 0)
            {
                _storageService = new StorageService();
                foreach (var file in provider.Files)
                {
                    var photoUrl = dobi.DobiId + "/profile/" + "profile_pic.png";
                    Stream stream = await file.ReadAsStreamAsync();
                    _storageService.UploadFile("dhobi-bucket", photoUrl, stream);
                    dobi.Photo = s3Prefix + photoUrl;
                }
            }
            var response = await _dobiBusiness.UpdateDobi(dobi);
            return Ok(response);
        }

    }
}
