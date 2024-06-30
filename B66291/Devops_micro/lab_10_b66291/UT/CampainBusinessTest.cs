using NUnit.Framework;
using core;
using core.DataBase;
using core.Models;
using core.Business;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UT
{
    public class CampainBusinessTests
    {
        private CampainBusiness _campainBusiness;

        [SetUp]
        public void Setup()
        {
            var myDbtest = "Server=localhost;Database=store;Uid=root;Pwd=123456;";
            Storage.Init(myDbtest);
            StoreDb.CrearDatosSync();

            _campainBusiness = new CampainBusiness();
        }

        [Test]
        public async Task SaveCampain_CasoExitoso()
        {
            var campain = new Campain("TestSender", "Test Message", 1);
            _campainBusiness.SaveCampain(campain);
            var messages = await _campainBusiness.GetMessageList();
            var savedCampain = messages.FirstOrDefault(c => c.Sender == "TestSender" && c.MessageContent == "Test Message");
            Assert.IsNotNull(savedCampain, "La campaña no se guardó correctamente.");
        }

        [Test]
        public async Task GetMessageList_CasoExitoso()
        {
            var campain = new Campain("TestSender", "Test Message", 1);
            _campainBusiness.SaveCampain(campain);
            var messages = await _campainBusiness.GetMessageList();
            Assert.IsNotNull(messages);
            Assert.GreaterOrEqual(((List<Campain>)messages).Count, 1);
        }

         [Test]
        public async Task EraseCampain_CasoExitoso()
        {
            var campain = new Campain("TestSender", "Message to delete", 1);
            _campainBusiness.SaveCampain(campain);
           _campainBusiness.eraseCampain("Message to delete");
            var messages = await _campainBusiness.GetMessageList();
            var deletedCampain = messages.FirstOrDefault(c => c.MessageContent == "Message to delete");
            Assert.IsNotNull(deletedCampain, "La campaña no se encontró.");
            Assert.AreEqual(0, deletedCampain.Status, "La campaña no se borró correctamente.");
        }
    }
}