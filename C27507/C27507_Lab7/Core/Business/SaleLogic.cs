using System.Globalization;
using Core;

//API
using MyStoreAPI.DB;
using MyStoreAPI.Models;
namespace MyStoreAPI.Business
{
    public class SaleLogic{
        
        private DB_Sale db_sale {get;}

        public SaleLogic(){            
            this.db_sale = new DB_Sale();            
        }

        public async Task<Sale> createSaleAsync(Cart cart){

            if (cart == null)
                throw new BussinessException($"{nameof(cart)} no puede ser nulo");            
            // Utilizamos la lógica del carrito y sus validaciones
            CartLogic cartLogic = new CartLogic(cart);            
            cartLogic.validateCart();

            string purchaseNum = generateRandomPurchaseNum();
            DateTime dateTimeSale = DateTime.Now;
            int saleId = await db_sale.InsertSaleAsync(purchaseNum,dateTimeSale,cart);
            Sale sale = new Sale(saleId,purchaseNum,dateTimeSale,cart);
            return sale;
        }


        public async Task<Sale> createSaleAsync(Cart cart,DateTime dateTimeSale){

            if (cart == null)
                throw new BussinessException($"{nameof(cart)} no puede ser nulo");            
            // Utilizamos la lógica del carrito y sus validaciones
            CartLogic cartLogic = new CartLogic(cart);            
            cartLogic.validateCart();
            string purchaseNum = generateRandomPurchaseNum();            
            int saleId = await db_sale.InsertSaleAsync(purchaseNum,dateTimeSale,cart);
            Sale sale = new Sale(saleId,purchaseNum,dateTimeSale,cart);
            return sale;
        }

        public Sale createSaleTest(Cart cart,DateTime dateTimeSale){

            if (cart == null)
                throw new BussinessException($"{nameof(cart)} no puede ser nulo");            
            // Utilizamos la lógica del carrito y sus validaciones
            CartLogic cartLogic = new CartLogic(cart);            
            cartLogic.validateCart();
            string purchaseNum = generateRandomPurchaseNum();            
            int saleId = db_sale.InsertSaleTest(purchaseNum,dateTimeSale,cart);
            Sale sale = new Sale(saleId,purchaseNum,dateTimeSale,cart);
            return sale;
        }
                                                 
        // private async Task<Sale> createSale(Cart cart){            
        //     string purchaseNum = generateRandomPurchaseNum();
        //     DateTime dateTimeSale = DateTime.Now;
        //     int saleId = await DB_Sale.InsertSaleAsync(purchaseNum,dateTimeSale,cart);
        //     Sale sale = new Sale(saleId,purchaseNum,dateTimeSale,cart);
        //     return sale;
        // }


        private string generateRandomPurchaseNum(){            
            Guid purchaseNum = Guid.NewGuid();            
            string largeString = purchaseNum.ToString().Replace("-", "");            
            Random random = new Random();            
            string randomCharacter = "";            
            for (int i = 0; i < 8; i += 2){                
                int randomIndex = random.Next(i, i + 2);
                randomCharacter += largeString[randomIndex];
            }
            return randomCharacter;
        }
        
        //Metodos para los reportes de Ventas
        public async Task<RegisteredSaleReport> getSalesByDayAndWeekAsync(DateTime dateFormat){                    
            if (dateFormat == DateTime.MinValue) throw new BussinessException("El formato de fecha actual no puede ser la fecha minima");
            //Si las validaciones principales son correctas, hacemos el llamado a la bd para cada tarea

            Task<IEnumerable<RegisteredSale>> salesByDayTask = getSalesFromTodayAsync(dateFormat);
            Task<IEnumerable<RegisteredSaleWeek>> salesByWeekTask = getSalesFromLastWeekAsync(dateFormat);

            await Task.WhenAll(salesByDayTask, salesByWeekTask);

            return new RegisteredSaleReport {
                salesByDay = await salesByDayTask,
                salesByWeek = await salesByWeekTask
            };
        }

        private async Task<IEnumerable<RegisteredSale>> getSalesFromTodayAsync(DateTime dateFormat){                                                            

            IEnumerable<RegisteredSale> allRegisteredSalesByDay = await db_sale.GetRegisteredSalesByDayAsync(dateFormat);                                                

            if (allRegisteredSalesByDay == null)throw new BussinessException($"{nameof(allRegisteredSalesByDay)} puede ser 0, pero no nula");
            foreach (var thisRegisteredSale in allRegisteredSalesByDay){

                if (thisRegisteredSale.IdSale <= 0)        
                    throw new BussinessException($"{nameof(thisRegisteredSale)} debe contener un IdSale entero positivo.");
        
                if (string.IsNullOrEmpty(thisRegisteredSale.PurchaseNum))        
                    throw new BussinessException($"{nameof(thisRegisteredSale)} no puede contener un codigo de compra nulo o vacío.");        
                
                if (thisRegisteredSale.SubTotal <= 0)
                    throw new BussinessException($"{nameof(thisRegisteredSale)} el Subtotal de la venta no puede ser menor ni igual que cero.");

                if (thisRegisteredSale.Tax < 0)
                    throw new BussinessException($"{nameof(thisRegisteredSale)} el impuesto de la venta no puede ser menor que cero.");
    

                if (thisRegisteredSale.Total <= 0)        
                    throw new BussinessException($"{nameof(thisRegisteredSale)} el total de la venta no puede ser menor ni igual que cero.");
            }
            return allRegisteredSalesByDay;
        }

        private async Task<IEnumerable<RegisteredSaleWeek>> getSalesFromLastWeekAsync(DateTime dateFormat){
            IEnumerable<RegisteredSaleWeek> allRegisteredSalesByWeek = await db_sale.GetRegisteredSalesByWeekAsync(dateFormat);                                                

            if (allRegisteredSalesByWeek == null)throw new BussinessException("La lista de ventas puede ser 0, pero no nula");

            foreach (var thisRegisteredSale in allRegisteredSalesByWeek){
                if (string.IsNullOrEmpty(thisRegisteredSale.dayOfWeek)) throw new BussinessException("Los dias de la semana no son validos");
                if (thisRegisteredSale.total == null) throw new BussinessException($"{nameof(thisRegisteredSale)} el total de ventas de un dia de la semana no puede ser nulo");
                if (thisRegisteredSale.total <= 0) throw new BussinessException($"{nameof(thisRegisteredSale)} el total de ventas de un dia de la semana no puede ser menor a cero");
            }
            return allRegisteredSalesByWeek;
        }        
    }
}