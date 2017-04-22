using Dhobi.Admin.Api.Helpers;
using Dhobi.Business.Interface;
using Dhobi.Core.Dobi.DbModels;
using Dhobi.Core.Dobi.ViewModels;
using Dhobi.Core.Manager.DbModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Dhobi.Admin.Api.Controllers
{
    [RoutePrefix("api")]
    public class DobiController : ApiController
    {
        private IDobiBusiness _dobiBusiness;
        private TokenGenerator _tokenGenerator;
        private StorageService _storageService;
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
        [HttpPost]
        [Route("v1/dobi")]
        //[Authorize(Roles = "Admin,Superadmin")]
        public async Task<IHttpActionResult> AddDobi()
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest("Invalid data.");
            //}
            //var addedBy = GetManagerInformationFromToken();
            //if (addedBy == null)
            //{
            //    return BadRequest("Invalid admin token.");
            //}
            //var response = await _managerBusiness.AddManager(manager, addedBy);
            //return Ok(response);

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
                        if (string.IsNullOrWhiteSpace(dobi.IcNumber))
                        {
                            return BadRequest("IC Number Is Required");
                        }
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
            dobi.DobiId = new Guid().ToString();
            //_storageService = new StorageService();
            //foreach (var file in provider.Files)
            //{
            //    var photoUrl = dobi.DobiId + "/profile/" + "profile_pic.png";
            //    Stream stream = await file.ReadAsStreamAsync();
            //    _storageService.UploadFile("dobiadmin", photoUrl, stream);
            //    dobi.Photo = s3Prefix + photoUrl;
            //}

            return Ok();
        }
    }
}
