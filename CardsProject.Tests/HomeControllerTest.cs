using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CardsProject;
using CardsProject.Models;
using CardsProject.Controllers;
using System.Web.Routing;
using Moq;
using System.Web;
using System.Security.Principal;
using HomeControllerTest.Reprository;
using ICardReprository = CardsProject.Models.ICardReprository;
using Microsoft.AspNetCore.Mvc;

using ViewResult = System.Web.Mvc.ViewResult;
using ControllerContext = System.Web.Mvc.ControllerContext;
using RedirectToRouteResult = System.Web.Mvc.RedirectToRouteResult;


namespace HomeControllerTest
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void FirstHomePage()
        {
            HomeController controller = new HomeController();
            // Act
            System.Web.Mvc.ViewResult result = controller.Index() as System.Web.Mvc.ViewResult;
            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Index2()
        {
            HomeController controller = new HomeController();
            // Act
            ViewResult result = controller.Index() as ViewResult;
            // Assert
            Assert.AreEqual("Index", result.ViewName);
        }

        [TestMethod]
        public void SecondHomePage()
        {
            HomeController controller = new HomeController();
            // Act
            ViewResult result = controller.Index1() as ViewResult;
            // Assert
            Assert.AreEqual("Index1", result.ViewName);
        }

        [TestMethod]
        public void Create_Get()
        {
            HomeController controller = new HomeController();
            // Act
            ViewResult result = controller.Create() as ViewResult;
            // Assert
            Assert.IsNotNull(result);
        }
        private static HomeController GetHomeController(ICardReprository cardReprository)
        {
            HomeController empcontroller = new HomeController(cardReprository);
            empcontroller.ControllerContext = new ControllerContext()
            {
                Controller = empcontroller,
                RequestContext = new RequestContext(new MockHttpContext(), new RouteData())
            };
            return empcontroller;
        }
        [TestMethod]
        public void GetAllCardsFromRepository()
        {
            // Arrange  
            BusinessCard Card1 = GetCardName(2,"Mhmd", "Male",  "0123456789", "Alexandria");
            BusinessCard Card2 = GetCardName(3,"Mhmd2", "Male", "0123456789", "Alexandria");
            InMemoryCardRepository cardReprository = new InMemoryCardRepository();
            cardReprository.Add(Card1);
            cardReprository.Add(Card2);
            var controller = GetHomeController(cardReprository);
            var result = controller.CardDetails();
            var datamodel = (IEnumerable<BusinessCard>)result.ViewData.Model;
            CollectionAssert.Contains(datamodel.ToList(), Card1);
            CollectionAssert.Contains(datamodel.ToList(), Card2);
        }
        [TestMethod]
        public void SingleCardDetails()
        {
            HomeController controller = new HomeController();
            // Act
            ViewResult result = controller.SingleCardDetails() as ViewResult;
            // Assert
            Assert.AreEqual("SingleCardDetails", result.ViewName);
        }
        [TestMethod]
        public void ImportCsv()
        {
            HomeController controller = new HomeController();
            // Act
            ViewResult result = controller.ImportCsv() as ViewResult;
            // Assert
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void XmlView()
        {
            HomeController controller = new HomeController();
            // Act
            ViewResult result = controller.XmlView() as ViewResult;
            // Assert
            Assert.AreEqual("XmlView", result.ViewName);
        }
        [TestMethod]
        public void CsvCreateCard()
        {
            HomeController controller = new HomeController();
            // Act
           
            // Assert
            var redirectResult = controller.CsvCreateCard() as RedirectToRouteResult;

            Assert.AreEqual("ImportCsv", redirectResult.RouteValues["Action"]);
        }
        [TestMethod]
        public void XMlCreateCard()
        {
            HomeController controller = new HomeController();
            // Act

            // Assert
            var redirectResult = controller.XMlCreateCard() as RedirectToRouteResult;

            Assert.AreEqual("XmlView", redirectResult.RouteValues["Action"]);
        }
        // helping methods
        BusinessCard GetCardName(int id, string Name, string Gender, string phone, string address)
        {
            return new BusinessCard
            {
               ID=id,
                Phone = phone,
                Name = Name,
                Gender = Gender,
                Address = address,
              
            };
        }
        private class MockHttpContext : HttpContextBase
        {
            private readonly IPrincipal _user = new GenericPrincipal(new GenericIdentity("someUser"), null /* roles */);
            public override IPrincipal User
            {
                get
                {
                    return _user;
                }
                set
                {
                    base.User = value;
                }
            }
        }
    }
}
