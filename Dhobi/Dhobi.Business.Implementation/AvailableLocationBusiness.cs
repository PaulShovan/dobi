using Dhobi.Business.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dhobi.Core.AvailableLoacation.DbModels;
using Dhobi.Repository.Interface;
using Dhobi.Common;
using Dhobi.Core;

namespace Dhobi.Business.Implementation
{
    public class AvailableLocationBusiness : IAvailableLocationBusiness
    {
        private IAvailableLoacationRepository _availableLocationRepository;
        public AvailableLocationBusiness(IAvailableLoacationRepository availableLoacationRepository)
        {
            _availableLocationRepository = availableLoacationRepository;
        }
        public async Task<GenericResponse<string>> AddAvailableLocation(List<string> locationNames)
        {
            try
            {
                var locations = new List<Location>();
                foreach (var location in locationNames)
                {
                    if (!string.IsNullOrWhiteSpace(location))
                    {
                        locations.Add(new Location
                        {
                            LocationId = Guid.NewGuid().ToString(),
                            LocationName = location,
                            Status = (int)LocationStatus.Active
                        });
                    }
                }
                if(locations.Count <= 0)
                {
                    return new GenericResponse<string>(false, "No location to add.");
                }
                var result = await _availableLocationRepository.AddAvailableLocation(locations);
                if (!result)
                {
                    return new GenericResponse<string>(false, "Error adding location.");
                }
                return new GenericResponse<string>(true, "Locations added successfully.");
            }
            catch (Exception exception)
            {
                throw new Exception("Error in adding locations" + exception);
            }
            
        }

        public Task<List<Location>> GetAllLocations(int status)
        {
            throw new NotImplementedException();
        }

        public Task<List<Location>> GetLocationByStatus(int status)
        {
            throw new NotImplementedException();
        }
    }
}
