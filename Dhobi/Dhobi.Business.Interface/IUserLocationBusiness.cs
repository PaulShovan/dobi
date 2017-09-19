using Dhobi.Core.Location.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhobi.Business.Interface
{
    public interface IUserLocationBusiness
    {
        bool ValidateUserLocation(UserLocation userLocation);
    }
}
