using System;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using DiseaseTracker.DAL;
using Unity;
using Unity.Lifetime;
using Unity.Mvc5;

namespace DiseaseTracker
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers
            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<TrackerContext>();
            container.RegisterFactory<HttpClient>(x => new HttpClient
                    {BaseAddress = new Uri("https://coronavirus-tracker-api.herokuapp.com/v2/")},
                new HierarchicalLifetimeManager());
            container.RegisterFactory<IIpProvider>(x => new ServerIpProvider());

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
        
        private class ServerIpProvider : IIpProvider
        {
            public string GetIp(HttpServerUtilityBase server, HttpRequestBase request)
            {
                return server.HtmlEncode(request.UserHostAddress);
            }
        }
    }
}