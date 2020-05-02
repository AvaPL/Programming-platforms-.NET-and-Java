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
        private HomeViewModel viewModel = new HomeViewModel();
        private TrackerContext db;
        private HttpClient client;

        public HomeController(TrackerContext db, HttpClient client)
        {
            this.db = db;
            this.client = client;
        }

        public async Task<ActionResult> Index()
        {
            COVID19Statistics statistics = await FetchCOVID19StatisticsAsync();
            viewModel.Statistics = statistics ?? new COVID19Statistics();
            UpdateVisitor(statistics);
            return View(viewModel);
        }

        private void UpdateVisitor(COVID19Statistics statistics)
        {
            string ip = Server.HtmlEncode(Request.UserHostAddress);
            Visitor visitor = db.Visitors.SingleOrDefault(v => v.Ip == ip);
            if (visitor == null)
                AddNewVisitor(ip, statistics);
            else
                EditExistingVisitor(visitor, statistics);
        }

        private void EditExistingVisitor(Visitor visitor, COVID19Statistics statistics)
        {
            viewModel.LastVisit = visitor.LastVisit;
            if (statistics != null)
            {
                viewModel.ConfirmedIncrease = statistics.Confirmed - visitor.LastConfirmed;
                visitor.UpdateVisitor(statistics.Confirmed);
            }

            viewModel.TotalVisits = visitor.TotalVisits;
            db.Entry(visitor).State = EntityState.Modified;
            db.SaveChanges();
        }

        private void AddNewVisitor(string ip, COVID19Statistics statistics)
        {
            if (statistics == null) return;
            Visitor visitor = new Visitor(ip, statistics.Confirmed);
            viewModel.LastVisit = visitor.LastVisit;
            viewModel.TotalVisits = visitor.TotalVisits;
            viewModel.ConfirmedIncrease = 0;
            db.Visitors.Add(visitor);
            db.SaveChanges();
        }

        public async Task<COVID19Statistics> FetchCOVID19StatisticsAsync()
        {
            HttpResponseMessage response = await client.GetAsync("latest");
            if (!response.IsSuccessStatusCode) return null;
            string json = await response.Content.ReadAsStringAsync();
            return JObject.Parse(json)["latest"].ToObject<COVID19Statistics>();
        }
    }
}