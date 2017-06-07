using Dhobi.Core.AvailableLoacation;
using Dhobi.Core.AvailableLoacation.DbModels;
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
    public class AvailableLoacationRepository : Repository<Location>, IAvailableLoacationRepository
    {
        public async Task<bool> AddAvailableLocation(List<Location> locations)
        {
            await Collection.InsertManyAsync(locations);
            return true;
        }

        public async Task<List<Location>> GetAvailableActiveLocations()
        {
            try
            {
                var projection = Builders<Location>.Projection.Exclude("_id");
                var locations = await Collection.Find(d => d.Status == 1).Project<Location>(projection).ToListAsync();
                return locations;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting locations." + ex);
            }
        }

        public async Task<List<Location>> GetAvailableLocations()
        {
            try
            {
                var projection = Builders<Location>.Projection.Exclude("_id");
                var locations = await Collection.Find(d => d.LocationId != "").Project<Location>(projection).ToListAsync();
                return locations;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting locations." + ex);
            }
        }
    }
}
