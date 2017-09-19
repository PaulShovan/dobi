using Dhobi.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dhobi.Core.Location.DbModels;

namespace Dhobi.Business.Implementation
{
    public class UserLocationBusiness : IUserLocationBusiness
    {
        public bool ValidateUserLocation(UserLocation userLocation)
        {
            if(userLocation == null)
            {
                return false;
            }
            else if(string.IsNullOrWhiteSpace(userLocation.Address) 
                || string.IsNullOrWhiteSpace(userLocation.Title) 
                || userLocation.Lat < -90 
                || userLocation.Lat > 90
                || userLocation.Lon < -180
                || userLocation.Lon > 180)
            {
                return false;
            }
            return true;
        }
    }
}
