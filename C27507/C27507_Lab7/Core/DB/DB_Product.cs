
using System;
using System.Transactions;
using System.Collections.Generic;//para usar list
//API
using MyStoreAPI.Models;
using MySqlConnector;
namespace MyStoreAPI.DB
{
    public class DB_Product{



        //Funciones CRUD
        public static void InsertProductsStore(List<Product> allProducts){
            try{
                using (TransactionScope scopeTransaction = new TransactionScope()){
                    using (MySqlConnection connectionWithDB = new MySqlConnection(DB_Connection.INIT_CONNECTION_DB())){                
                        connectionWithDB.Open();
                        foreach (var actualProduct in allProducts){
                            string insertQuery = @"
                                INSERT INTO Products (Name, ImageUrl, Price, Quantity, Description)
                                VALUES (@name, @imageUrl, @price, @quantity, @description);
                            ";

                            using (MySqlCommand command = new MySqlCommand(insertQuery, connectionWithDB)){                            
                                command.Parameters.AddWithValue("@name", actualProduct.name);
                                command.Parameters.AddWithValue("@imageUrl", actualProduct.imageUrl);
                                command.Parameters.AddWithValue("@price", actualProduct.price);
                                command.Parameters.AddWithValue("@quantity", actualProduct.quantity);
                                command.Parameters.AddWithValue("@description", actualProduct.description);

                                command.ExecuteNonQuery();
                            }
                        }                        
                    }
                    scopeTransaction.Complete();
                    Console.WriteLine("Productos insertados correctamente en la tabla 'Products'.");
                }
            }catch (Exception ex){
                throw;
            }            
        }

        public static List<Product> SelectProducts(){

            List<Product> productListToStoreInstance = new List<Product>();

            try{
                //primer usign para establecer la conexion con la BD
                using (MySqlConnection connectionWithDB = new MySqlConnection(DB_Connection.INIT_CONNECTION_DB())){
                    
                    connectionWithDB.Open();
                    string selectProducts = @"
                        SELECT IdProduct, Name, ImageUrl, Price, Quantity, Description
                        FROM Products;
                        ";                

                    //segundo using para crear un objeto de comandos SQL y que al finalizar se borre y liberen datos
                    using (MySqlCommand command = new MySqlCommand(selectProducts, connectionWithDB)){
                        //tercer using para crear un objeto de lectura SQL
                        using (MySqlDataReader readerTable = command.ExecuteReader()){

                            //Mientras haya al menos una tupla que leer, guarde los datos de ese Select en la lista
                            while(readerTable.Read()){
                                productListToStoreInstance.Add(new Product{
                                    id = Convert.ToInt32(readerTable["IdProduct"]),
                                    name = readerTable["Name"].ToString(),
                                    imageUrl = readerTable["ImageUrl"].ToString(),
                                    price = Convert.ToDecimal(readerTable["Price"]),
                                    quantity = Convert.ToInt32(readerTable["Quantity"]),
                                    description = readerTable["Description"].ToString()
                                });
                            }
                        }
                    }
                }
            }catch (Exception ex){
                throw;
            }            
            return productListToStoreInstance;
        }

        public static bool ProductsInTableExist(){
            try{
                using (MySqlConnection connectionWithDB = new MySqlConnection(DB_Connection.INIT_CONNECTION_DB()))
                {
                    connectionWithDB.Open();
                    string numberOfProduct = "SELECT COUNT(*) FROM Products";
                    using (MySqlCommand command = new MySqlCommand(numberOfProduct, connectionWithDB)){
                        int count = Convert.ToInt32(command.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
            catch (Exception ex){                
                throw;
            }
        }        
    }
}