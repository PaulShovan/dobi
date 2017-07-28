using Dhobi.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dhobi.Core.Device.DbModels;
using Dhobi.Repository.Interface;
using Dhobi.Common;

namespace Dhobi.Business.Implementation
{
    public class DeviceStatusBusiness : IDeviceStatusBusiness
    {
        private IDeviceStausRepository _deviceStatusRepository;
        public DeviceStatusBusiness(IDeviceStausRepository deviceStausRepository)
        {
            _deviceStatusRepository = deviceStausRepository;
        }
        public async Task<List<string>> GetDiviceIds(string userId)
        {
            try
            {
                List<string> deviceIds = new List<string>();
                var devices = await _deviceStatusRepository.GetDeviceStatus(userId);
                foreach (var device in devices)
                {
                    deviceIds.Add(device.RegistrationId);
                }
                return deviceIds;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting device id" + ex);
            }
        }

        public async Task<bool> RegisterDevice(DeviceStatus status)
        {
            try
            {
                status.Status = (int)DeviceOnlineStatus.Active;
                return await _deviceStatusRepository.AddDeviceStatus(status);
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding device" + ex);
            }
        }
    }
}
