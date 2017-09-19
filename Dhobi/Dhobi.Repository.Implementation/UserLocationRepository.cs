using Dhobi.Core.Location.DbModels;
using Dhobi.Repository.Implementation.Base;
using Dhobi.Repository.Interface;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhobi.Repository.Implementation
{
    public class UserLocationRepository : Repository<UserLocation>, IUserLocationRepository
    {
        public async Task<bool> AddLocation(UserLocation location)
        {
            await Collection.InsertOneAsync(location);
            return true;
        }

        public async Task<List<UserLocation>> GetUserLocation(string userId)
        {
            try
            {
                var projection = Builders<UserLocation>.Projection.Exclude("_id");
                var locations = await Collection.Find(s => s.UserId == userId).Project<UserLocation>(projection).ToListAsync();
                return locations;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in getting locations" + ex);
            }
        }
    }
}
