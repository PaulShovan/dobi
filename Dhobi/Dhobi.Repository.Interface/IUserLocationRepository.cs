using Dhobi.Core.Location.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhobi.Repository.Interface
{
    public interface IUserLocationRepository
    {
        Task<bool> AddLocation(UserLocation location);
        Task<List<UserLocation>> GetUserLocation(string userId);
    }
}
