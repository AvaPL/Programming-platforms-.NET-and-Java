using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Moq;
using NUnit.Framework;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using DiseaseTracker.Controllers;
using DiseaseTracker.Models;
using Moq.Protected;

namespace DiseaseTrackerTests
{
    [TestFixture]
    public class LocationsControllerTests
    {
        private LocationsController controller;

        [SetUp]
        public void SetUp()
        {
            HttpClient httpClient = MockHttpClient();
            controller = new LocationsController(httpClient);
        }

        private static HttpClient MockHttpClient()
        {
            Mock<HttpMessageHandler> handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()).ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(
                        // 'country':'Afghanistan'
                        // 'latest':{'confirmed':2335,'deaths':68,'recovered':30}
                        // 'country':'Albania'
                        // 'latest':{'confirmed':782,'deaths':31,'recovered':15}
                        "{'latest':{'confirmed':3343777,'deaths':238650,'recovered':0},'locations':[{'id':0,'country':'Afghanistan','country_code':'AF','country_population':37172386,'province':'','last_updated':'2020-05-02T10:54:07.093215Z','coordinates':{'latitude':'33.0','longitude':'65.0'},'latest':{'confirmed':2335,'deaths':68,'recovered':30}},{'id':1,'country':'Albania','country_code':'AL','country_population':2866376,'province':'','last_updated':'2020-05-02T10:54:07.099936Z','coordinates':{'latitude':'41.1533','longitude':'20.1683'},'latest':{'confirmed':782,'deaths':31,'recovered':15}}]}")
                }).Verifiable();
            HttpClient httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://test.com/"),
            };
            return httpClient;
        }

        [Test]
        public void ShouldReturnCorrectViewName()
        {
            ViewResult result = (ViewResult) controller.Index().Result;
            Assert.AreEqual("Index", result.ViewName);
        }

        [Test]
        public void ShouldContainCorrectLocations()
        {
            ViewResult result = (ViewResult) controller.Index().Result;
            List<COVID19Location> locations = (List<COVID19Location>) result.ViewData.Model;
            Assert.AreEqual(new[] {"Afghanistan", "Albania"},
                locations.Select(l => l.Country));
        }

        [Test]
        public void ShouldContainCorrectStatistics()
        {
            ViewResult result = (ViewResult) controller.Index().Result;
            List<COVID19Location> locations = (List<COVID19Location>) result.ViewData.Model;
            List<COVID19Statistics> statistics = locations.Select(l => l.Latest).ToList();
            List<COVID19Statistics> expected = new List<COVID19Statistics>
            {
                new COVID19Statistics {Confirmed = 2335, Deaths = 68, Recovered = 30},
                new COVID19Statistics {Confirmed = 782, Deaths = 31, Recovered = 15}
            };
            Assert.AreEqual(expected.Select(s => s.Confirmed), statistics.Select(s => s.Confirmed));
            Assert.AreEqual(expected.Select(s => s.Deaths), statistics.Select(s => s.Deaths));
            Assert.AreEqual(expected.Select(s => s.Recovered), statistics.Select(s => s.Recovered));
        }
    }
}