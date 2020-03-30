using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DiseaseTracker.Models;

namespace DiseaseTracker.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        public ActionResult COVID19Statistics()
        {
            COVID19Statistics statistics = FetchCOVID19Statistics();
            return View(statistics);
        }

        public COVID19Statistics FetchCOVID19Statistics()
        {
            COVID19Statistics statistics = new COVID19Statistics();
            using HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://coronavirus-tracker-api.herokuapp.com/v2/");
            Task<HttpResponseMessage> responseTask = client.GetAsync("latest");
            responseTask.Wait();
            HttpResponseMessage result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                Task<COVID19Statistics> readTask = result.Content.ReadAsAsync<COVID19Statistics>();
                readTask.Wait();
                statistics = readTask.Result;
            }

            return statistics;
        }
    }
}