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
using Dhobi.Common;
using Dhobi.Core.Dobi.ViewModels;

namespace Dhobi.Business.Implementation
{
    public class DobiBusiness : IDobiBusiness
    {
        private const int DobiIdLength = 6;
        private IDobiRepository _dobiRepository;
        private IOrderRepository _orderRepository;
        public DobiBusiness(IDobiRepository dobiRepository, IOrderRepository orderRepository)
        {
            _dobiRepository = dobiRepository;
            _orderRepository = orderRepository;
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

        public async Task<string> GenerateDobiId()
        {
            try
            {
                var totalDobi = await _dobiRepository.GetDobiCount();
                var newDobiId = (totalDobi + 1).ToString().PadLeft(DobiIdLength, '0');
                var dobiIdTermplate = Constants.DOBIID;
                var dobiId = dobiIdTermplate.Replace("__ID__", newDobiId);
                return dobiId;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in generating dobi id" + ex);
            }
            
        }

        public async Task<DobiHomePageResponse> GetDobiHomePageResponse(string dobiId)
        {
            try
            {
                var dobiInformation = await _dobiRepository.GetDobiById(dobiId);
                if (dobiInformation == null || string.IsNullOrWhiteSpace(dobiInformation.DobiId))
                {
                    return null;
                }
                var newOrderCount = await _orderRepository.GetNewOrderCountByStatus((int)OrderStatus.New);
                var acceptedOrderCount = await _orderRepository.GetNewOrderCountByStatus((int)OrderStatus.Confirmed);
                var deliverableOrderCount = await _orderRepository.GetNewOrderCountByStatus((int)OrderStatus.Deliverable);
                return new DobiHomePageResponse
                {
                    Name = dobiInformation.Name,
                    Phone = dobiInformation.Phone,
                    Photo = dobiInformation.Photo,
                    DobiId = dobiInformation.DobiId,
                    IcNumber = dobiInformation.IcNumber,
                    DrivingLicense = dobiInformation.DrivingLicense,
                    NewOrderCount = newOrderCount,
                    AcceptedOrderCount = acceptedOrderCount,
                    DeliveravbleOrderCount = deliverableOrderCount
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting dobi home page response" + ex);
            }
        }

        public async Task<GenericResponse<string>> UpdateDobi(Dobi dobi)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dobi.DobiId))
                {
                    return new GenericResponse<string>(false, null, "Invalid Dobi.");
                }
                var existingDobi = await _dobiRepository.GetDobiById(dobi.DobiId);
                if(existingDobi == null)
                {
                    return new GenericResponse<string>(false, null, "Invalid Dobi.");
                }

                if ((existingDobi.Email != dobi.Email) && !await _dobiRepository.IsEmailAvailable(dobi.Email))
                {
                    return new GenericResponse<string>(false, null, "Email is not available");
                }
                else if ((existingDobi.Phone != dobi.Phone) && !await _dobiRepository.IsPhoneNumberAvailable(dobi.Phone))
                {
                    return new GenericResponse<string>(false, null, "Phone number is not available");
                }
                else if ((existingDobi.PassportNumber != dobi.PassportNumber) && !string.IsNullOrWhiteSpace(dobi.PassportNumber) && !await _dobiRepository.IsPassportNumberAvailable(dobi.PassportNumber))
                {
                    return new GenericResponse<string>(false, null, "Passport number is not available");
                }
                else if ((existingDobi.IcNumber != dobi.IcNumber) && !await _dobiRepository.IsIcNumberAvailable(dobi.IcNumber))
                {
                    return new GenericResponse<string>(false, null, "IC number is not available");
                }
                else if ((existingDobi.DrivingLicense != dobi.DrivingLicense) && !string.IsNullOrWhiteSpace(dobi.DrivingLicense) && !await _dobiRepository.IsDrivingLicenseAvailable(dobi.DrivingLicense))
                {
                    return new GenericResponse<string>(false, null, "Driving license is not available");
                }
                var response = await _dobiRepository.UpdateDobi(dobi);
                if (response == null)
                {
                    return new GenericResponse<string>(false, null, "Error updating Dobi.");
                }
                return new GenericResponse<string>(true, null, "Dobi updated successfully.");
            }
            catch (Exception exception)
            {
                throw new Exception("Error updating dobi" + exception);
            }
        }
    }
}
