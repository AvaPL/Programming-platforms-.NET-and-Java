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
        public async Task<ActionResult> Index()
        {
            List<COVID19Location> locations = await FetchCOVID19LocationsAsync();
            return View(locations);
        }

        public async Task<List<COVID19Location>> FetchCOVID19LocationsAsync()
        {
            using HttpClient client = new HttpClient
                {BaseAddress = new Uri("https://coronavirus-tracker-api.herokuapp.com/v2/")};
            HttpResponseMessage response = await client.GetAsync("locations");
            if (!response.IsSuccessStatusCode) return null;
            string json = await response.Content.ReadAsStringAsync();
            return FormatCOVID19Locations(json);
        }

        private static List<COVID19Location> FormatCOVID19Locations(string json)
        {
            List<COVID19Location> locations = JObject.Parse(json)["locations"].ToObject<List<COVID19Location>>();
            return locations.GroupBy(location => location.Country).Select(grouping =>
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
        }
    }
}