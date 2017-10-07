using Dhobi.Core.Device.DbModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dhobi.Repository.Interface
{
    public interface IDeviceStausRepository
    {
        Task<bool> AddDeviceStatus(DeviceStatus status);
        Task<bool> RemoveDeviceStatus(DeviceStatus status);
        Task<List<DeviceStatus>> GetDeviceStatus(string userId, int deviceOs);
        Task<List<DeviceStatus>> GetDeviceStatus(string userId);
    }
}
