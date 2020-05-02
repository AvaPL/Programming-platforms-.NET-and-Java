using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using DiseaseTracker.Models;
using Newtonsoft.Json.Linq;

namespace DiseaseTracker.Controllers
{
    public class LocationsController : Controller
    {
        private HttpClient client;

        public LocationsController(HttpClient client)
        {
            this.client = client;
        }

        private static readonly log4net.ILog Log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        
        public async Task<ActionResult> Index()
        {
            List<COVID19Location> locations = await FetchCOVID19LocationsAsync();
            if (locations == null)
            {
                Log.Warn("Fetched empty locations statistics");
                locations = new List<COVID19Location>();
            }
            else
                Log.Info("Fetched location statistics");

            return View("Index", locations);
        }

        public async Task<List<COVID19Location>> FetchCOVID19LocationsAsync()
        {
            HttpResponseMessage response = await client.GetAsync("locations");
            if (!response.IsSuccessStatusCode)
            {
                Log.Warn("Failed to fetch location statistics");
                return null;
            }
            string json = await response.Content.ReadAsStringAsync();
            Log.Debug("Converted fetched information to string");
            return FormatCOVID19Locations(json);
        }

        private static List<COVID19Location> FormatCOVID19Locations(string json)
        {
            List<COVID19Location> locations = JObject.Parse(json)["locations"].ToObject<List<COVID19Location>>();
            Log.Debug("Parsed locations list");
            List<COVID19Location> aggregatedLocations= locations.GroupBy(location => location.Country).Select(grouping =>
                new COVID19Location
                {
                    Country = grouping.Key,
                    Latest = new COVID19Statistics
                    {
                        Confirmed = grouping.Sum(location => location.Latest.Confirmed),
                        Deaths = grouping.Sum(location => location.Latest.Deaths),
                        Recovered = grouping.Sum(location => location.Latest.Recovered)
                    }
                }).OrderBy(location => location.Country).ToList();
            Log.Debug("Calculated latest statistics for each location");
            return aggregatedLocations;
        }
    }
}