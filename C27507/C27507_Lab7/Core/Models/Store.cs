using System;
using MyStoreAPI.DB;
using MyStoreAPI.Business;
using MyStoreAPI.Models;
using System.Collections.Generic;//para usar list
using System.Linq; // Asegúrate de incluir esto en la parte superior de tu archivo


namespace MyStoreAPI.Models
{
    public sealed class Store{
        public IEnumerable<Product> Products { get; private set; } 
        public IEnumerable<Category> AllProductCategories {get; private set;}
        public IEnumerable<PaymentMethod> AllPaymentMethods {get; private set;}
        public int TaxPercentage { get; private set; }
        public bool StoreConnectedWithDB {get; private set;}
        
        private Store(){            
            this.TaxPercentage = 13;
            this.AllProductCategories = Categories.Instance.AllProductCategories;
            this.Products = new List<Product>();            
            //Hacemos de cuenta que se crea con exito las tablas o las reconoce en Program.cs          
            if(!DB_Product.ProductsInTableExist()){
                List<Product> temporalList = new List<Product>();
                foreach (var productToStoreTable in createStoreProducts()){
                    temporalList.Add(productToStoreTable);                    
                }
                DB_Product.InsertProductsInDB(temporalList);
            }
            if(DB_PaymentMethod.PaymentMethodsInTableExist() == false ) DB_PaymentMethod.InsertPaymentMethod();

            //sobreescribimos la lista para que los productos tengan el ID correcto dado por la tabla
            this.Products = DB_Product.SelectProducts();            
            this.AllPaymentMethods = DB_PaymentMethod.GetAllPaymentMethods();
            var paymentMethods = DB_PaymentMethod.GetAllPaymentMethods();            
            this.StoreConnectedWithDB = true;            
            //Ahora la tienda tendra productos para el StoreController (GET)
        }
        
        public static readonly Store Instance;

        static Store()
        {             
            //unica instancia de Store (con los productos y la conexion a la DB)            
            Store.Instance = new Store();                                    
        }

        //Actualizar la lista en memoria de Store, justo despues de actualizar la DB con el nuevo producto
        public void addNewProductInStore(Product newProduct){
        var productList = Products.ToList();
        productList.Add(newProduct);
        Products = productList;
    }

        public void updatePaymentMethodInStore(int paymentMethodId, int newStatus){
            var paymentMethodsList = AllPaymentMethods.ToList();
            var paymentMethod = paymentMethodsList.FirstOrDefault(pm => (int)pm.payment == paymentMethodId);
            if (paymentMethod != null){
                paymentMethod.verify = newStatus == 1;
                AllPaymentMethods = paymentMethodsList;
            }
        }


                
        //Generamos productos en caso de que por alguna razon la tabla este vacia     
        public List<Product> createStoreProducts(){
            var products = new List<Product>();

                //Generar 30 productos
                products.Add(new Product
                {                
                    name = "Tablet Samsung",
                    imageUrl = "./img/tablet_samsung.jpg",
                    price = 25,
                    quantity = 0,                
                    description = "lorem ipsum",                    
                    category = Categories.Instance.AllProductCategories.FirstOrDefault(c => c.id == 2)
                });

                products.Add(new Product
                {                
                    name = "TV LG UHD",
                    imageUrl = "./img/tv.jfif",                
                    price = 50,
                    quantity = 0,
                    description = "lorem ipsum",
                    category = Categories.Instance.AllProductCategories.FirstOrDefault( c => c.id == 4)
                });

                products.Add(new Product
                {                
                    name = "Auriculares Genericos",
                    imageUrl = "./img/auri.jfif",
                    price = 100,
                    quantity = 0,
                    description = "lorem ipsum",
                    category = Categories.Instance.AllProductCategories.FirstOrDefault( c => c.id == 5)                    
                });

                products.Add(new Product
                {                
                    name = "Dualshock PS4",
                    imageUrl = "./img/dualshock4.jpg",
                    price = 35,
                    quantity = 0,                
                    description = "lorem ipsum",
                    category = Categories.Instance.AllProductCategories.FirstOrDefault( c => c.id == 3)
                });

                products.Add(new Product
                {                
                    name = "Teclado LED",
                    imageUrl = "./img/teclado.jpg",
                    price = 75,
                    quantity = 0,              
                    description = "lorem ipsum",
                    category = Categories.Instance.AllProductCategories.FirstOrDefault( c => c.id == 6)
                });

                products.Add(new Product
                {                
                    name = "Samsung A54",
                    imageUrl = "./img/a54_samsung.jpg",
                    price = 250,
                    quantity = 0,              
                    description = "lorem ipsum",
                    category = Categories.Instance.AllProductCategories.FirstOrDefault( c => c.id == 2)
                });

                products.Add(new Product
                {             
                    name = "Dualshock PS5",
                    imageUrl = "./img/dualshock5.jpg",
                    price = 250,
                    quantity = 0,                
                    description = "lorem ipsum",                    
                    category = Categories.Instance.AllProductCategories.FirstOrDefault( c => c.id == 3)
                });

                products.Add(new Product
                {                             
                    name = "Samsung Galaxy A54",
                    imageUrl = "./img/a54_samsung.png",
                    price = 150,
                    quantity = 0,                
                    description = "carousel",                    
                    category = Categories.Instance.AllProductCategories.FirstOrDefault( c => c.id == 2)
                });

                products.Add(new Product
                {                
                    name = "Mouse Microsoft",
                    imageUrl = "./img/mouse.png",
                    price = 2500,
                    quantity = 0,                
                    description = "lorem ipsum",
                    category = Categories.Instance.AllProductCategories.FirstOrDefault( c => c.id == 1)
                });

                products.Add(new Product
                {             
                    name = "Módem Router - Archer VR400",
                    imageUrl = "./img/router_archerVR400.jpg",
                    price = 75,
                    quantity = 0,                
                    description = "lorem ipsum",
                    category = Categories.Instance.AllProductCategories.FirstOrDefault( c => c.id == 1)
                });

                return products;
        }


        //Esta funcion es solo para generar ventas automaticas a lo largo de 2 semanas
        //por si se necesitan pruebas de algo. Por eso esta comentada mas arriba
        public static void mockDataSales(List<Product> productsFromDB){

            try{                
                //Generamos ventas
                //var productsFromDB = Store.Products;            
                Random rand = new Random();
                SaleLogic saleLogic = new SaleLogic();            
                DateTime startDate = new DateTime(2024, 04, 29);

                for (int week = 0; week < 2; week++){

                    for (int day = 0; day < 7; day++){
                        
                        DateTime currentDate = startDate.AddDays(week * 7 + day);

                        //cantidad de ventas por dia
                        for (int saleCount = 0; saleCount < 5; saleCount++){            

                            //cantidad de productos por venta
                            List<Product> someProductsFromDB = new List<Product>(); 
                            int countProduct = 0;

                            //cantidad de productos por venta
                            while (countProduct < 4){

                                // Generamos un índice aleatorio para seleccionar un producto de la lista
                                int randomIndex = rand.Next(productsFromDB.Count());
                                Product selectedProduct = productsFromDB.ElementAt(randomIndex);

                                // Verificamos si el producto seleccionado ya está en la lista someProductsFromDB
                                if (!someProductsFromDB.Any(p => p.name == selectedProduct.name)){
                                    someProductsFromDB.Add(selectedProduct);
                                    countProduct++;
                                }
                            }

                            decimal subTotal = 0;
                            decimal total = 0;
                            foreach (var product in someProductsFromDB){
                                subTotal += product.price;
                                total += ( product.price + (product.price * 0.13m / 100));
                            }

                            var cartTest = new Cart{
                                allProduct = someProductsFromDB,
                                Subtotal = subTotal,
                                Total = total,
                                Direction = "Costa Rica",
                                PaymentMethod = PaymentMethods.paymentMethodsList.First(p => p.payment == PaymentMethodNumber.CASH)
                            };                        
                            saleLogic.createSaleTest(cartTest,currentDate);
                        }
                    }
                }
            }catch (Exception ex){
                Console.WriteLine($"Error en mockDataAsync: {ex.Message}");
                throw;
            }
        }
        
    }
}
