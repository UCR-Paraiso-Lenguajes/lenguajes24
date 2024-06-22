
using System;
using System.Transactions;
using System.Collections.Generic;//para usar list
//API
using MyStoreAPI.Models;
using MySqlConnector;
namespace MyStoreAPI.DB
{
    public class DB_Product{

        public DB_Product(){}


        //Funciones CRUD
        //Metodo para la instancia de Store, solo se usa si detecta que no hay productos al construir la app
        public static void InsertProductsInDB(IEnumerable<Product> allProducts){
            try{
                using (TransactionScope scopeTransaction = new TransactionScope()){
                    using (MySqlConnection connectionWithDB = new MySqlConnection(DB_Connection.INIT_CONNECTION_DB())){                
                        connectionWithDB.Open();
                        foreach (var actualProduct in allProducts){
                            string insertQuery = @"
                                INSERT INTO Products (Name, ImageUrl, Price, Quantity, Description, Category)
                                VALUES (@name, @imageUrl, @price, @quantity, @description,@idCategory);
                            ";

                            using (MySqlCommand command = new MySqlCommand(insertQuery, connectionWithDB)){                            
                                command.Parameters.AddWithValue("@name", actualProduct.name);
                                command.Parameters.AddWithValue("@imageUrl", actualProduct.imageUrl);
                                command.Parameters.AddWithValue("@price", actualProduct.price);
                                command.Parameters.AddWithValue("@quantity", actualProduct.quantity);
                                command.Parameters.AddWithValue("@description", actualProduct.description);
                                command.Parameters.AddWithValue("@idCategory", actualProduct.category.id);

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

        public static IEnumerable<Product> SelectProducts(){

            List<Product> productListToStoreInstance = new List<Product>();

            try{
                //primer usign para establecer la conexion con la BD
                using (MySqlConnection connectionWithDB = new MySqlConnection(DB_Connection.INIT_CONNECTION_DB())){
                    
                    connectionWithDB.Open();
                    string selectProducts = @"
                        SELECT IdProduct, Name, ImageUrl, Price, Quantity, Description, Category
                        FROM Products;
                        ";                

                    //segundo using para crear un objeto de comandos SQL y que al finalizar se borre y liberen datos
                    using (MySqlCommand command = new MySqlCommand(selectProducts, connectionWithDB)){
                        //tercer using para crear un objeto de lectura SQL
                        using (MySqlDataReader readerTable = command.ExecuteReader()){

                            //Mientras haya al menos una tupla que leer, guarde los datos de ese Select en la lista
                            while(readerTable.Read()){

                                //recibimos por aparte el id y construimos la categorias con la
                                //lista de categorias en memoria
                                int idCategory = Convert.ToInt32(readerTable["Category"]);

                                if(idCategory <= 0) throw new Exception($"{nameof(idCategory)} no es válido.");

                                var categoryForProduct = Categories.Instance.AllProductCategories.FirstOrDefault( c => c.id == idCategory );

                                if(categoryForProduct.id <= 0) throw new Exception($"{nameof(categoryForProduct)} no es valido.");
                                if(string.IsNullOrEmpty(categoryForProduct.name)) throw new Exception($"{nameof(categoryForProduct)} no es valido.");

                                productListToStoreInstance.Add(new Product{
                                    id = Convert.ToInt32(readerTable["IdProduct"]),
                                    name = readerTable["Name"].ToString(),
                                    imageUrl = readerTable["ImageUrl"].ToString(),
                                    price = Convert.ToDecimal(readerTable["Price"]),
                                    quantity = Convert.ToInt32(readerTable["Quantity"]),
                                    description = readerTable["Description"].ToString(),
                                    category = categoryForProduct
                                });
                            }
                        }
                    }
                }
            }catch (Exception ex){
                Console.WriteLine("Error desde DB_Sale: " + ex);
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

        public async Task<int> InsertProductInDBAsync(Product newProduct){
            
            if(newProduct == null) throw new ArgumentException($"{nameof(newProduct)} no puede ser nulo");

            MySqlConnection connectionWithDB = null;
            MySqlTransaction transaction = null;
            int idInserted =  -1;

            try{
                connectionWithDB = new MySqlConnection(DB_Connection.INIT_CONNECTION_DB());                
                await connectionWithDB.OpenAsync();
                transaction = await connectionWithDB.BeginTransactionAsync();

                string insertQuery = @"
                    INSERT INTO Products (Name, ImageUrl, Price, Quantity, Description, Category)
                    VALUES (@name, @imageUrl, @price, @quantity, @description, @idCategory);
                ";

                using (MySqlCommand command = new MySqlCommand(insertQuery, connectionWithDB)){
                    command.Transaction = transaction;

                    command.Parameters.AddWithValue("@name", newProduct.name);
                    command.Parameters.AddWithValue("@imageUrl", newProduct.imageUrl);
                    command.Parameters.AddWithValue("@price", newProduct.price);
                    command.Parameters.AddWithValue("@quantity", newProduct.quantity);
                    command.Parameters.AddWithValue("@description", newProduct.description);
                    command.Parameters.AddWithValue("@idCategory", newProduct.category.id);
                    await command.ExecuteNonQueryAsync();

                    //Retornamos el id del producto insertado
                    command.CommandText = "SELECT LAST_INSERT_ID();";
                    idInserted = Convert.ToInt32(await command.ExecuteScalarAsync());
                }
                await transaction.CommitAsync();                
            }catch (Exception ex){ 
                await transaction.RollbackAsync();
                throw;                
            }finally{                
                await connectionWithDB.CloseAsync();
            }
            //Si devuelve -1 es por error
            return idInserted;
        }
    }
}