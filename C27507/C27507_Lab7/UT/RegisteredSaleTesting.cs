using Core;
using MyStoreAPI.Business;
using NUnit.Framework;

namespace UT{

    [TestFixture]
    public class RegisteredSaleTesting : TestBase{

        [SetUp]
        public void SetUp(){



        }


        //Test para los datos de consultas de ventas
        [Test]
        public async Task RegisteredSaleValidData(){

            var saleLogic = new SaleLogic();
            DateTime validDateFormat = new DateTime(2024,05,27);
            var validResult = await saleLogic.getSalesByDayAndWeekAsync(validDateFormat);
            Assert.IsNotNull(validResult);        
        }
        

        // [Test]
        // public async Task RegisteredSaleEmptyData(){
            
        //     DateTime validDateFormat = new DateTime(2024,04,27);
        //     //registro de ventas de una fecha futura (deberia devolver una lista en 0)            
        //     DateTime validDateFormatWithNoData = new DateTime(2024,06,10);
            
        //     var saleLogic = new SaleLogic();        
        //     var noEmptyData= await saleLogic.getSalesByDayAndWeekAsync(validDateFormat);                        
        //     var noEmptyDataSalesByDay = noEmptyData.salesByDay;
        //     var noEmptyDataSalesByWeek = noEmptyData.salesByWeek;


        //     var emptyData= await saleLogic.getSalesByDayAndWeekAsync(validDateFormatWithNoData);                
        //     var emptyDataSalesByDay = emptyData.salesByDay;
        //     var emptyDataSalesByWeek = emptyData.salesByWeek;
            
        //     Assert.IsEmpty(emptyDataSalesByWeek);
        //     Assert.IsEmpty(emptyDataSalesByDay);

        //     Assert.IsNotEmpty(noEmptyDataSalesByDay);
        //     Assert.IsNotEmpty(noEmptyDataSalesByWeek);
        // }        



        // [Test]
        // public async Task RowCountRegisteredSale(){

        //     //var validDateFormat = "2024-04-27";
        //     DateTime validDateFormat = new DateTime(2024,04,27);
        //     //registro de ventas de una fecha futura (deberia devolver una lista en 0)            
            
        //     var saleLogic = new SaleLogic();        
        //     var noEmptyData= await saleLogic.getSalesByDayAndWeekAsync(validDateFormat);                        
        //     var noEmptyDataSalesByDay = noEmptyData.salesByDay;
        //     var noEmptyDataSalesByWeek = noEmptyData.salesByWeek;

        //     Assert.IsNotEmpty(noEmptyDataSalesByWeek);            

        //     // Verificar que el total de las ventas sea mayor o igual a 0
        //     foreach (var sale in noEmptyDataSalesByDay)
        //     {                                
        //         Assert.IsTrue(sale.Total >= 0);
        //     }
        //     foreach (var sale in noEmptyDataSalesByWeek)
        //     {                                              
        //         Assert.IsTrue(sale.total >= 0);
        //     }
        //     // Verificar que la cantidad de filas sea mayor a 0
        //     Assert.IsTrue(noEmptyDataSalesByDay.Count() > 0);
        //     Assert.IsTrue(noEmptyDataSalesByWeek.Count() > 0);            
        // }
    }
}