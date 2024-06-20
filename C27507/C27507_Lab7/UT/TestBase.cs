using System;
using MyStoreAPI;
using MyStoreAPI.DB;
using MyStoreAPI.Business;
using MyStoreAPI.Models;
using System.Collections.Generic;
using System.Linq;
using Core;
using MyStoreAPI.Business;
using NUnit.Framework;


namespace UT{

    public class TestBase{
        protected string TestConnectionString { get; set; }

        [SetUp]
        public void SetUp()
        {
            TestConnectionString = "server=localhost;user=root;password=123456;database=MyStoreApi";
            DB_Connection.SET_CONFIG_DB(TestConnectionString);
            DB_Connection.ConnectDB();
        }

    }
}
