using Dhobi.Core;
using Dhobi.Core.AvailableLoacation.DbModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dhobi.Business.Interface
{
    public interface IAvailableLocationBusiness
    {
        Task<GenericResponse<string>> AddAvailableLocation(List<string> locationNames);
        Task<List<Location>> GetLocationByStatus(int status);
        Task<List<Location>> GetAllLocations(int status);
    }
}
