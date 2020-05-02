using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DiseaseTracker.Controllers;
using DiseaseTracker.DAL;
using DiseaseTracker.Models;
using Moq;
using Moq.Protected;
using NUnit.Framework;

namespace DiseaseTrackerTests
{
    [TestFixture]
    public class HomeControllerTests
    {
        private HomeController controller;

        [SetUp]
        public void SetUp()
        {
            Mock<HttpMessageHandler> handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()).ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(
                        "{'latest':{'confirmed':3343777,'deaths':238650,'recovered':1542}}")
                }).Verifiable();
            HttpClient httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://test.com/"),
            };

            IQueryable<Visitor> visitors = new List<Visitor>().AsQueryable();
            Mock<DbSet<Visitor>> visitorsMock = new Mock<DbSet<Visitor>>();
            visitorsMock.As<IQueryable<Visitor>>().Setup(m=>m.Provider).Returns(visitors.Provider);
            visitorsMock.As<IQueryable<Visitor>>().Setup(m => m.Expression).Returns(visitors.Expression);
            visitorsMock.As<IQueryable<Visitor>>().Setup(m => m.ElementType).Returns(visitors.ElementType);
            visitorsMock.As<IQueryable<Visitor>>().Setup(m => m.GetEnumerator()).Returns(visitors.GetEnumerator());

            Mock<TrackerContext> visitorContextMock = new Mock<TrackerContext>();
            visitorContextMock.Setup(x => x.Visitors).Returns(visitorsMock.Object);
            
            Mock<IIpProvider> ipProviderMock = new Mock<IIpProvider>(MockBehavior.Strict);
            ipProviderMock.Setup(x => x.GetIp(It.IsAny<HttpServerUtilityBase>(), It.IsAny<HttpRequestBase>()))
                .Returns("::1");
            
            controller = new HomeController(visitorContextMock.Object, httpClient, ipProviderMock.Object);
        }

        [Test]
        public void ShouldReturnCorrectViewName()
        {
            ViewResult result = (ViewResult) controller.Index().Result;
            Assert.AreEqual("Index", result.ViewName);
        }

        [Test]
        public void ShouldContainCorrectStatistics()
        {
            ViewResult result = (ViewResult) controller.Index().Result;
            HomeViewModel homeViewModel = (HomeViewModel) result.ViewData.Model;
            Assert.AreEqual(3343777, homeViewModel.Statistics.Confirmed);
            Assert.AreEqual(238650, homeViewModel.Statistics.Deaths);
            Assert.AreEqual(1542, homeViewModel.Statistics.Recovered);
        }
    }
}