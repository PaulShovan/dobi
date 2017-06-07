using Dhobi.Core.AvailableLoacation;
using Dhobi.Core.AvailableLoacation.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhobi.Repository.Interface
{
    public interface IAvailableLoacationRepository
    {
        Task<bool> AddAvailableLocation(List<Location> locations);
        Task<List<Location>> GetAvailableLocations();
        Task<List<Location>> GetAvailableActiveLocations();
    }
}
