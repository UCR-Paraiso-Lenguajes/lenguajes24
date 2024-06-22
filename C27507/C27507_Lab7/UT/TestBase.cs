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

    public static class TestBase{
        private static bool _isInitialized = false;
        private static string TestConnectionString;

        public static void EnsureInitialized() {
            if (!_isInitialized) {
                _isInitialized = true;
                // Realiza la inicialización de la base de datos aquí
                InitializeDatabase();
            }
        }
        
            private static void InitializeDatabase() {

                //Conectamos con la BD
                TestConnectionString = "server=localhost;user=root;password=123456;database=MyStoreApi";
                DB_Connection.SET_CONFIG_DB(TestConnectionString);
                DB_Connection.ConnectDB();
                
                //Volarnos las tablas
                ClearAllTables();

                //Llamamos a Store.Instance que cuando se inicializa revisa por si tiene productos y ventas, sino, entonces
                //llena las tablas
                _ = Store.Instance;                            
                //Llenamos las tablas de ventas
                Store.mockDataSales(Store.Instance.Products.ToList());            
        }

        private static void ClearAllTables(){
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

                        // Eliminar la tabla completamente si existe
                        var dropCommand = new MySqlCommand($"DROP TABLE IF EXISTS {tableName}", connection);
                        dropCommand.ExecuteNonQuery();
                    }
                }
            }
        }
        catch (Exception ex){
            Console.WriteLine($"Error al borrar las tablas: {ex.Message}");
        }
    }


    }
}
