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
            viewModel.Statistics = await FetchCOVID19StatisticsAsync();
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