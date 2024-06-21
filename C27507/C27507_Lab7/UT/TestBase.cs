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
using MySqlConnector;


namespace UT{

    public class TestBase{
        protected string TestConnectionString { get; set; }

        [SetUp]
        public void SetUp(){


            //Conectamos con la BD
            TestConnectionString = "server=localhost;user=root;password=123456;database=MyStoreApi";
            DB_Connection.SET_CONFIG_DB(TestConnectionString);
            DB_Connection.ConnectDB();

            //Volarnos las tablas
            ClearAllTables();

            //Llamamos a Store.Instance que cuando se inicializa revisa por si tiene productos y ventas, sino, entonces
            //llena las tablas
            _ = Store.Instance;
            
            //Llenamos tablas de pago            
            //DB_PaymentMethod.InsertPaymentMethod();
            //Llenamos las tablas productos      
            //DB_Product.InsertProductsInDB(Store.Instance.defaultListProduct);
            //Llenamos las tablas de ventas
            Store.mockDataSales(Store.Instance.Products.ToList());
        }

        private void ClearAllTables(){
            try
            {
                using (var connection = new MySqlConnection(TestConnectionString))
                {
                    connection.Open();

                    // Obtener los nombres de todas las tablas en la base de datos
                    var command = new MySqlCommand("SHOW TABLES", connection);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var tableName = reader.GetString(0);

                            // Eliminar todos los datos de cada tabla
                            var deleteCommand = new MySqlCommand($"DELETE FROM {tableName}", connection);
                            deleteCommand.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex){                
                Console.WriteLine($"Error al limpiar las tablas: {ex.Message}");
            }
        }

    }
}
