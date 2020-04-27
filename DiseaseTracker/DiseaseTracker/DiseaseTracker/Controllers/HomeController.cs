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
        private TrackerContext db = new TrackerContext();
        private HomeViewModel viewModel = new HomeViewModel();

        public async Task<ActionResult> Index()
        {
            COVID19Statistics statistics = await FetchCOVID19StatisticsAsync();
            if (statistics != null)
            {
                viewModel.Statistics = statistics;
                UpdateVisitor(statistics.Confirmed);
            }

            return View(viewModel);
        }

        private void UpdateVisitor(long confirmed)
        {
            string ip = Server.HtmlEncode(Request.UserHostAddress);
            Visitor visitor = db.Visitors.SingleOrDefault(v => v.Ip == ip);
            if (visitor == null)
                AddNewVisitor(ip, confirmed);
            else
                EditExistingVisitor(visitor, confirmed);
        }

        private void EditExistingVisitor(Visitor visitor, long confirmed)
        {
            viewModel.LastVisit = visitor.LastVisit;
            viewModel.ConfirmedIncrease = confirmed - visitor.LastConfirmed;
            visitor.UpdateVisitor(confirmed);
            viewModel.TotalVisits = visitor.TotalVisits;
            db.Entry(visitor).State = EntityState.Modified;
            db.SaveChanges();
        }

        private void AddNewVisitor(string ip, long confirmed)
        {
            Visitor visitor = new Visitor(ip, confirmed);
            viewModel.LastVisit = visitor.LastVisit;
            viewModel.TotalVisits = visitor.TotalVisits;
            viewModel.ConfirmedIncrease = 0;
            db.Visitors.Add(visitor);
            db.SaveChanges();
        }

        public async Task<COVID19Statistics> FetchCOVID19StatisticsAsync()
        {
            using HttpClient client = new HttpClient
                {BaseAddress = new Uri("https://coronavirus-tracker-api.herokuapp.com/v2/")};
            HttpResponseMessage response = await client.GetAsync("latest");
            if (!response.IsSuccessStatusCode) return null;
            string json = await response.Content.ReadAsStringAsync();
            return JObject.Parse(json)["latest"].ToObject<COVID19Statistics>();
        }
    }
}