using Dhobi.Common;
using Dhobi.Repository.Interface;
using Dhobi.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Dhobi.Service.Implementation
{
    public class LocationService : ILocationService
    {
        private IAvailableLoacationRepository _availableLoacationRepository;
        public LocationService(IAvailableLoacationRepository availableLoacationRepository)
        {
            _availableLoacationRepository = availableLoacationRepository;
        }
        private List<string> availableZones = new List<string>();
        public async Task<List<string>> GetAvailableActiveZones()
        {
            var availableLocations = new List<string>();
            var locations = await _availableLoacationRepository.GetAvailableActiveLocations();
            foreach (var location in locations)
            {
                availableLocations.Add(location.LocationName);
            }
            return availableLocations;
        }
        private string GetZoneNameFromGivenAddress(string address)
        {
            foreach (var zone in availableZones)
            {
                if (address.ToLower().Contains(zone.ToLower()))
                {
                    return zone;
                }
            }
            return null;
        }
        private string GetAddressUsingLatLong(double lat, double lon)
        {
            var zone = "";
            string url = string.Format(Constants.MAPURL, lat, lon);
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(url);
            foreach (XmlNode node in xDoc.DocumentElement.ChildNodes)
            {
                if (node.Name == "result")
                {
                    foreach (XmlNode node1 in node)
                    {
                        if (node1.Name == "formatted_address")
                        {
                            zone = GetZoneNameFromGivenAddress(node1.InnerText);
                            if (zone != null)
                            {
                                return zone;
                            }
                        }
                    }
                }
            }
            return null;
        }
        public async Task<string> GetZoneFromAddress(double lat, double lon, string address)
        {
            var zoneName = "";
            availableZones = await GetAvailableActiveZones();
            zoneName = GetZoneNameFromGivenAddress(address);
            if (zoneName != null)
            {
                return zoneName;
            }
            zoneName = GetAddressUsingLatLong(lat, lon);
            return zoneName;
        }
    }
}
