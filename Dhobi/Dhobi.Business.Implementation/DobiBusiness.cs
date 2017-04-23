using Dhobi.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dhobi.Core;
using Dhobi.Core.Dobi.DbModels;
using Dhobi.Core.Manager.DbModels;
using Dhobi.Repository.Interface;

namespace Dhobi.Business.Implementation
{
    public class DobiBusiness : IDobiBusiness
    {
        private IDobiRepository _dobiRepository;
        public DobiBusiness(IDobiRepository dobiRepository)
        {
            _dobiRepository = dobiRepository;
        }
        public async Task<GenericResponse<string>> AddDobi(Dobi dobi)
        {
            try
            {
                if (!await _dobiRepository.IsEmailAvailable(dobi.Email))
                {
                    return new GenericResponse<string>(false, null, "Email is not available");
                }
                else if (!await _dobiRepository.IsPhoneNumberAvailable(dobi.Phone))
                {
                    return new GenericResponse<string>(false, null, "Phone number is not available");
                }
                else if (!string.IsNullOrWhiteSpace(dobi.PassportNumber) && !await _dobiRepository.IsPassportNumberAvailable(dobi.PassportNumber))
                {
                    return new GenericResponse<string>(false, null, "Passport number is not available");
                }
                else if (!await _dobiRepository.IsIcNumberAvailable(dobi.IcNumber))
                {
                    return new GenericResponse<string>(false, null, "IC number is not available");
                }
                else if (!string.IsNullOrWhiteSpace(dobi.DrivingLicense) && !await _dobiRepository.IsDrivingLicenseAvailable(dobi.DrivingLicense))
                {
                    return new GenericResponse<string>(false, null, "Driving license is not available");
                }
                dobi.DobiId = Guid.NewGuid().ToString();
                dobi.JoinDate = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
                var response = await _dobiRepository.AddDobi(dobi);
                if (!response)
                {
                    return new GenericResponse<string>(false, null, "Error adding Dobi.");
                }
                return new GenericResponse<string>(true, null, "Dobi added successfully.");
            }
            catch (Exception exception)
            {
                throw new Exception("Error adding dobi" + exception);
            }
        }
    }
}
