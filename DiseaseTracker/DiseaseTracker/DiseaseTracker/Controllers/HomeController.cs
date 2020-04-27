using System;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;
using DiseaseTracker.DAL;
using DiseaseTracker.Models;
using Newtonsoft.Json.Linq;

namespace DiseaseTracker.Controllers
{
    public class HomeController : Controller
    {
        private TrackerContext db = new TrackerContext();
        private StatisticsViewModel viewModel = new StatisticsViewModel();

        public ActionResult Index()
        {
            COVID19Statistics statistics = FetchCOVID19Statistics();
            viewModel.Statistics = statistics;
            UpdateVisitor();
            return View(viewModel);
        }

        private void UpdateVisitor()
        {
            string ip = Server.HtmlEncode(Request.UserHostAddress);
            Visitor visitor = db.Visitors.SingleOrDefault(v => v.Ip == ip);
            if (visitor == null)
                AddNewVisitor(ip);
            else
                EditExistingVisitor(visitor);
        }

        private void EditExistingVisitor(Visitor visitor)
        {
            viewModel.LastVisit = visitor.LastVisit;
            visitor.UpdateVisitor();
            viewModel.TotalVisits = visitor.TotalVisits;
            db.Entry(visitor).State = EntityState.Modified;
            db.SaveChanges();
        }

        private void AddNewVisitor(string ip)
        {
            Visitor visitor = new Visitor(ip);
            viewModel.LastVisit = visitor.LastVisit;
            viewModel.TotalVisits = visitor.TotalVisits;
            db.Visitors.Add(visitor);
            db.SaveChanges();
        }

        public COVID19Statistics FetchCOVID19Statistics()
        {
            using HttpClient client = new HttpClient
                {BaseAddress = new Uri("https://coronavirus-tracker-api.herokuapp.com/v2/")};
            HttpResponseMessage response = client.GetAsync("latest").Result;
            if (!response.IsSuccessStatusCode) return null;
            string json = response.Content.ReadAsStringAsync().Result;
            return JObject.Parse(json)["latest"].ToObject<COVID19Statistics>();
        }
    }
}