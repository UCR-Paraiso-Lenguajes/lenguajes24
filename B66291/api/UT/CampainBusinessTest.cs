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
            var myDbtest = "Server=localhost;Database=mysql;Uid=root;Pwd=123456;";
            Storage.Init(myDbtest);
            myDbtest = "Server=localhost;Database=store;Uid=root;Pwd=123456;";
            Storage.Init(myDbtest);
            StoreDb.CrearDatosSync();
            _campainBusiness = new CampainBusiness();
        }

       [Test]
        public async Task ObtenerListaMensajes()
        {
            var messages = await _campainBusiness.GetMessageList();
            Assert.IsNotNull(messages);
        }

         [Test]
        public async Task BorrarCampañaExitoso()
        {
            var message = "MessageContent";
            async Task EraseAction() => _campainBusiness.eraseCampain(message);
            Assert.DoesNotThrowAsync(EraseAction);
        }
    }
}