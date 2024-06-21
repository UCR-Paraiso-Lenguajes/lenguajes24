
using System;
using System.Transactions;
using System.Collections.Generic;//para usar list
//API
using MyStoreAPI.Models;
using MySqlConnector;
namespace MyStoreAPI.DB
{
    public class DB_SaleLine{

        public DB_SaleLine(){}

        //Reutlizamos la conexion de InsertSale para no duplicar conexion abierta
        //Reutilizamos tambien la misma transaction para evitar deadlocks y que si se hace rollback, 
        //tanto InsertSale e InsertSaleLine se deshagan
        public async Task InsertSalesLineAsync(MySqlConnection connectionWithDB, MySqlTransaction transaction,int idSale, string guid, Cart purchasedCart){

            if (connectionWithDB == null) throw new ArgumentException($"{nameof(connectionWithDB)} no puede ser una conexion nula");
            if (transaction == null) throw new ArgumentException($"{nameof(transaction)} no puede ser una transaction nula");
            if (string.IsNullOrEmpty(guid)) throw new ArgumentException($"{nameof(guid)} no puede ser nulo ni estar vacío");
            if (idSale <= 0) throw new ArgumentException($"{nameof(idSale)} debe ser un valor positivo");
            if (purchasedCart == null) throw new ArgumentException($"{nameof(purchasedCart)} no puede ser nulo");

            try{                                                
                string insertSalesLine = @"
                    INSERT INTO SalesLines (IdSale, IdProduct,Quantity,PricePaid,OriginalProductName, OriginalProductPrice)
                    VALUES (@saleId, @productId,@productQuantity,@pricePaid, @originalProductName, @originalProductPrice);";


                using (MySqlCommand command = new MySqlCommand(insertSalesLine, connectionWithDB)){                        
                    command.Transaction = transaction;             
                    foreach (var actualProductId in purchasedCart.allProduct){
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@saleId", idSale);
                        command.Parameters.AddWithValue("@productId", actualProductId.id);
                        command.Parameters.AddWithValue("@productQuantity", actualProductId.quantity);
                        command.Parameters.AddWithValue("@pricePaid", actualProductId.price);
                        command.Parameters.AddWithValue("@originalProductName", actualProductId.name);
                        command.Parameters.AddWithValue("@originalProductPrice", actualProductId.price);                        
                        await command.ExecuteNonQueryAsync();
                    }                                                                    
                }                      
            }catch (Exception ex){
                throw new Exception("Error al insertar línea de venta", ex);
            }                    
        }

        public void InsertSalesLinesTest(MySqlConnection connectionWithDB, MySqlTransaction transaction, int idSale, string guid, Cart purchasedCart) {
            if (connectionWithDB == null) throw new ArgumentException($"{nameof(connectionWithDB)} no puede ser una conexion nula");
            if (transaction == null) throw new ArgumentException($"{nameof(transaction)} no puede ser una transaction nula");
            if (string.IsNullOrEmpty(guid)) throw new ArgumentException($"{nameof(guid)} no puede ser nulo ni estar vacío");
            if (idSale <= 0) throw new ArgumentException($"{nameof(idSale)} debe ser un valor positivo");
            if (purchasedCart == null) throw new ArgumentException($"{nameof(purchasedCart)} no puede ser nulo");
            

            try {                
                string insertSalesLine = @"
                    INSERT INTO SalesLines (IdSale, IdProduct,Quantity,PricePaid,OriginalProductName, OriginalProductPrice)
                    VALUES (@saleId, @productId,@productQuantity,@pricePaid, @originalProductName, @originalProductPrice);";

                using (MySqlCommand command = new MySqlCommand(insertSalesLine, connectionWithDB)) {
                    command.Transaction = transaction;
                    foreach (var actualProductId in purchasedCart.allProduct) {
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@saleId", idSale);
                        command.Parameters.AddWithValue("@productId", actualProductId.id);
                        command.Parameters.AddWithValue("@productQuantity", actualProductId.quantity);
                        command.Parameters.AddWithValue("@pricePaid", actualProductId.price);
                        command.Parameters.AddWithValue("@originalProductName", actualProductId.name);
                        command.Parameters.AddWithValue("@originalProductPrice", actualProductId.price);
                        command.ExecuteNonQuery();         
                    }
                }
            } catch (Exception ex) {
                throw new Exception("Error al insertar línea de venta", ex);
            }
        }
    }    
}