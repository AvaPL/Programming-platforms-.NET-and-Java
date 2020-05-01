using System;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using DiseaseTracker.DAL;
using DiseaseTracker.Models;
using Newtonsoft.Json.Linq;

namespace DiseaseTracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly TrackerContext db = new TrackerContext();
        private readonly HomeViewModel viewModel = new HomeViewModel();

        private static readonly log4net.ILog Log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public async Task<ActionResult> Index()
        {
            COVID19Statistics statistics = await FetchCOVID19StatisticsAsync();

            if (statistics == null)
                Log.Warn("Fetched empty statistics");
            else Log.Info("Fetched latest statistics");
            viewModel.Statistics = statistics ?? new COVID19Statistics();
            UpdateVisitor(statistics);
            return View(viewModel);
        }

        private void UpdateVisitor(COVID19Statistics statistics)
        {
            string ip = Server.HtmlEncode(Request.UserHostAddress);
            Log.Debug("Requested user ip");
            Visitor visitor = db.Visitors.SingleOrDefault(v => v.Ip == ip);
            if (visitor == null)
            {
                AddNewVisitor(ip, statistics);
                Log.Debug("Added new visitor with ip = "+ip);
            }
            else
            {
                EditExistingVisitor(visitor, statistics);
                Log.Debug("Edited existing visitor with ip = "+ip);
            }
            Log.Info("Updated visitor with ip" + ip);
        }

        private void EditExistingVisitor(Visitor visitor, COVID19Statistics statistics)
            {
                viewModel.LastVisit = visitor.LastVisit;
                if (statistics != null)
                {
                    viewModel.ConfirmedIncrease = statistics.Confirmed - visitor.LastConfirmed;
                    Log.Debug("Calculated confirmed increase for visitor with ip = "+visitor.Ip);
                    visitor.UpdateVisitor(statistics.Confirmed);
                }

                viewModel.TotalVisits = visitor.TotalVisits;
                db.Entry(visitor).State = EntityState.Modified;
                Log.Debug("Modified state fo visitor with ip = " + visitor.Ip);
                db.SaveChanges();
            }

            private void AddNewVisitor(string ip, COVID19Statistics statistics)
            {
                if (statistics == null) return;
                Visitor visitor = new Visitor(ip, statistics.Confirmed);
                Log.Debug("Created new visitor with ip = " + ip);
                viewModel.LastVisit = visitor.LastVisit;
                viewModel.TotalVisits = visitor.TotalVisits;
                viewModel.ConfirmedIncrease = 0;
                Log.Debug("Updated new visitor with ip = " +ip);
                db.Visitors.Add(visitor);
                db.SaveChanges();
            }

            public async Task<COVID19Statistics> FetchCOVID19StatisticsAsync()
            {
                using HttpClient client = new HttpClient
                    {BaseAddress = new Uri("https://coronavirus-tracker-api.herokuapp.com/v2/")};
                HttpResponseMessage response = await client.GetAsync("latest");
                if (!response.IsSuccessStatusCode)
                {
                    Log.Warn("Failed to fetch latest statistics");
                    return null;
                }
                string json = await response.Content.ReadAsStringAsync();
                Log.Debug("Converted fetched information to string");
                return JObject.Parse(json)["latest"].ToObject<COVID19Statistics>();
            }
        }
    }