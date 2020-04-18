﻿using System;
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

            string ip = Server.HtmlEncode(Request.UserHostAddress);
            Visitor visitor = db.Visitors.SingleOrDefault(v => v.Ip == ip);
            if (visitor == null)
                db.Visitors.Add(new Visitor(ip));
            else
            {
                visitor.UpdateVisitor();
                db.Entry(visitor).State = EntityState.Modified;
            }

            db.SaveChanges();
            return View(statistics);
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