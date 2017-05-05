using Dhobi.Core.Device.DbModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dhobi.Business.Interface
{
    public interface IDeviceStatusBusiness
    {
        Task<bool> RegisterDevice(DeviceStatus status);
        Task<List<string>> GetDiviceIds(string userId);
    }
}
