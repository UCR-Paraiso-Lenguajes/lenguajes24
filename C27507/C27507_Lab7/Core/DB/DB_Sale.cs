using System;
using System.Globalization;
using System.Transactions;
using System.Collections.Generic;//para usar list
//API
using MyStoreAPI.Models;
using MySqlConnector;
using System.Runtime.InteropServices.Marshalling;
using Core;
//using Moq;

namespace MyStoreAPI.DB{    
    

    public class DB_Sale{

        private DB_SaleLine DB_SaleLine {get;}
        
        public DB_Sale(){            
            DB_SaleLine = new DB_SaleLine();
        }


        public static bool SalesInTableExist(){
            try{
                using (MySqlConnection connectionWithDB = new MySqlConnection(DB_Connection.INIT_CONNECTION_DB()))
                {
                    connectionWithDB.Open();
                    string numberOfSales = "SELECT COUNT(*) FROM Sales";
                    using (MySqlCommand command = new MySqlCommand(numberOfSales, connectionWithDB))
                    {
                        int count = Convert.ToInt32(command.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
            catch (Exception ex){
                throw;
            }
        }
                
        public async Task<int> InsertSaleAsync(string purchaseNum, DateTime dateTimeSale,Cart purchasedCart){

            if(string.IsNullOrEmpty(purchaseNum)) throw new BussinessException($"{nameof(purchaseNum)} no puede ser nulo ni estar vacio");
            if(dateTimeSale == DateTime.MinValue) throw new BussinessException($"{nameof(dateTimeSale)} es fecha no valida");
            if(purchasedCart == null) throw new ArgumentException($"{nameof(purchasedCart)} no puede ser nulo");

            MySqlConnection connectionWithDB = null;
            MySqlTransaction transaction = null;
            int thisIdSale = 0; // id a devolver

            try{
                connectionWithDB = new MySqlConnection(DB_Connection.INIT_CONNECTION_DB());                
                await connectionWithDB.OpenAsync();
                transaction = await connectionWithDB.BeginTransactionAsync();

                //Hacemos el insert
                string insertSale = @"
                INSERT INTO Sales (Total,PurchaseNum, Subtotal, Direction, IdPayment,DateSale)
                VALUES (@total, @purchaseNum, @subtotal, @direction, @idPayment,@dateSale);
                ";
                using(MySqlCommand command = new MySqlCommand(insertSale,connectionWithDB)){

                        //asociamos las acciones a realizar con una transaction
                        command.Transaction = transaction;

                        command.Parameters.AddWithValue("@total", purchasedCart.Total);
                        command.Parameters.AddWithValue("@purchaseNum", purchaseNum);
                        command.Parameters.AddWithValue("@subtotal", purchasedCart.Subtotal);
                        command.Parameters.AddWithValue("@direction", purchasedCart.Direction);                  
                        command.Parameters.AddWithValue("@idPayment", purchasedCart.PaymentMethod.payment);
                        command.Parameters.AddWithValue("@dateSale", dateTimeSale);
                        await command.ExecuteNonQueryAsync();


                         //Devolver el id de la venta generada (porque es IDENTITY(1,1))
                         //Reutilizamos el comando para evitar crear otro solo por un SELECT
                        command.Parameters.Clear();
                        string selectThisId = "SELECT IdSale FROM Sales WHERE PurchaseNum = @purchaseNum";
                        command.CommandText = selectThisId;
                        command.Parameters.AddWithValue("@purchaseNum", purchaseNum);
                        thisIdSale = Convert.ToInt32(await command.ExecuteScalarAsync());                                                                    
                }
                await DB_SaleLine.InsertSalesLineAsync(connectionWithDB,transaction,thisIdSale,purchaseNum,purchasedCart);
                //Commiteamos tanto la insercion en DB_Sale y DB_SaleLine
                await transaction.CommitAsync();
            }catch(Exception ex){
                await transaction.RollbackAsync();
                throw;

            }finally{
                await connectionWithDB.CloseAsync();
            }
            return thisIdSale;
        }

        public async Task<IEnumerable<RegisteredSale>> GetRegisteredSalesByDayAsync(DateTime dateParameter){       
            
            if (dateParameter == DateTime.MinValue) throw new BussinessException($"{nameof(dateParameter)} es fecha no valida");

            List<RegisteredSale>  registeredSalesToday =new List<RegisteredSale>();                        
            //Para evitar el error de "MySQL Transaction is active" manejamos las instancias individualmente
            //https://mysqlconnector.net/troubleshooting/transaction-usage/
            MySqlConnection connectionWithDB = null;
            MySqlTransaction transaction = null;
            
            try{
                connectionWithDB = new MySqlConnection(DB_Connection.INIT_CONNECTION_DB());                
                await connectionWithDB.OpenAsync();
                transaction = await connectionWithDB.BeginTransactionAsync();
                                                    
                string selectSales = @"
                SELECT IdSale,PurchaseNum, Total,Subtotal, Direction, IdPayment,DateSale 
                FROM Sales                    
                WHERE DATE(DateSale) = DATE(@dateParameter);";                    

                using(MySqlCommand command = new MySqlCommand(selectSales,connectionWithDB)){
                    //Asociar siempre el comando de la consulta a la transaccion
                    //porque sino la consideraria activa siempre
                    command.Transaction = transaction;
                    //Definimos el parametro a comparar                                            
                    command.Parameters.AddWithValue("@dateParameter", dateParameter);
                    using (MySqlDataReader readerTable = await command.ExecuteReaderAsync()){
                        
                        while(await readerTable.ReadAsync()){
                            
                            var newRegisteredSale = new RegisteredSale();                                
                            newRegisteredSale.IdSale = Convert.ToInt32(readerTable["IdSale"]);
                            newRegisteredSale.PurchaseNum = readerTable["PurchaseNum"].ToString();
                            newRegisteredSale.Total = Convert.ToDecimal(readerTable["Total"]);
                            newRegisteredSale.SubTotal = Convert.ToDecimal(readerTable["Subtotal"]);
                            newRegisteredSale.Direction = readerTable["Direction"].ToString();

                            //Como obtenemos el id de un metodo de pago, debemos buscar dentro de la lista estatica de PaymentMethods si existe
                            //dicho metodo de pago
                            int paymentId = Convert.ToInt32(readerTable["IdPayment"]);                                                                
                            PaymentMethod paymentMethod = PaymentMethods.paymentMethodsList.FirstOrDefault(p => (int)p.payment == paymentId);
                            if (paymentMethod == null){
                                //Si obtenemos un Id de un metodo de pago que no existe actualmente, tratarlo mas arriba
                                //ya que ha podido ser desactivado o eliminado
                                //throw new BussinessException("El metodo de pago actual no es valido");
                                throw new BussinessException($"{nameof(paymentMethod)} no puede ser nulo");
                            }
                            newRegisteredSale.PaymentMethod = paymentMethod;
                            newRegisteredSale.DateTimeSale = (DateTime)readerTable["DateSale"];    
                            //Validamos los datos
                            newRegisteredSale.validateRegisteredSale();
  
                            registeredSalesToday.Add(newRegisteredSale);
                        }
                    }
                }
                await transaction.CommitAsync();                

            }catch (Exception ex){                                                
                await transaction.RollbackAsync();
                throw;

            }finally{
                await connectionWithDB.CloseAsync();
            }            
            return registeredSalesToday;
        }

        public async Task<IEnumerable<RegisteredSaleWeek>> GetRegisteredSalesByWeekAsync(DateTime dateParameter){

            if (dateParameter == DateTime.MinValue) throw new BussinessException($"{nameof(dateParameter)} es fecha no valida");
            
            List<RegisteredSaleWeek>  registeredSaleWeek =new List<RegisteredSaleWeek>();                                    
            MySqlConnection connectionWithDB = null;
            MySqlTransaction transaction = null;
            
            try{
                connectionWithDB = new MySqlConnection(DB_Connection.INIT_CONNECTION_DB());                
                await connectionWithDB.OpenAsync();
                transaction = await connectionWithDB.BeginTransactionAsync();

                string selectLastWeekSales = @"
                SELECT DAYNAME(DateSale) as Day, SUM(total) as Total
                FROM Sales                                    
                WHERE DateSale > DATE_SUB(@dateParameter, INTERVAL 7 DAY)
                GROUP BY DAYNAME(DateSale);";

                using(MySqlCommand command = new MySqlCommand(selectLastWeekSales,connectionWithDB)){
                    
                    command.Transaction = transaction;
                    command.Parameters.AddWithValue("@dateParameter", dateParameter);
                    using (MySqlDataReader readerTable = await command.ExecuteReaderAsync()){
                        
                        while(await readerTable.ReadAsync()){                            
                            //como la consulta solo devuevle el nombre de la semana y el total, no se ve necesario
                            //generar una nueva clase de formato
                            var salesByLastWeek = new RegisteredSaleWeek();                                                            
                            salesByLastWeek.dayOfWeek = readerTable["Day"].ToString();
                            salesByLastWeek.total = Convert.ToDecimal(readerTable["Total"]);
                            salesByLastWeek.validateRegisteredSaleWeek();
                            registeredSaleWeek.Add(salesByLastWeek);
                        }
                    }
                }
                await transaction.CommitAsync();


            }catch(Exception ex){
                
                await transaction.RollbackAsync();
                throw;

            }finally{
                await connectionWithDB.CloseAsync();
            }            
            return registeredSaleWeek;
        }                



        public int InsertSaleTest(string purchaseNum, DateTime dateTimeSale, Cart purchasedCart) {
            if (string.IsNullOrEmpty(purchaseNum)) throw new BussinessException($"{nameof(purchaseNum)} no puede ser nulo ni estar vacio");
            if (dateTimeSale == DateTime.MinValue) throw new BussinessException($"{nameof(dateTimeSale)} es fecha no valida");
            if (purchasedCart == null) throw new ArgumentException($"{nameof(purchasedCart)} no puede ser nulo");

            MySqlConnection connectionWithDB = null;
            MySqlTransaction transaction = null;
            int thisIdSale = 0; // id a devolver

            try {
                connectionWithDB = new MySqlConnection(DB_Connection.INIT_CONNECTION_DB());
                connectionWithDB.Open();
                transaction = connectionWithDB.BeginTransaction();                

                // Hacemos el insert
                string insertSale = @"
                    INSERT INTO Sales (Total,PurchaseNum, Subtotal, Direction, IdPayment,DateSale)
                    VALUES (@total, @purchaseNum, @subtotal, @direction, @idPayment,@dateSale);
                ";
                using (MySqlCommand command = new MySqlCommand(insertSale, connectionWithDB)) {
                    // Asociamos las acciones a realizar con una transaction
                    command.Transaction = transaction;

                    command.Parameters.AddWithValue("@total", purchasedCart.Total);
                    command.Parameters.AddWithValue("@purchaseNum", purchaseNum);
                    command.Parameters.AddWithValue("@subtotal", purchasedCart.Subtotal);
                    command.Parameters.AddWithValue("@direction", purchasedCart.Direction);
                    command.Parameters.AddWithValue("@idPayment", purchasedCart.PaymentMethod.payment);
                    command.Parameters.AddWithValue("@dateSale", dateTimeSale);
                    
                    command.ExecuteNonQuery();

                    // Devolver el id de la venta generada (porque es IDENTITY(1,1))
                    // Reutilizamos el comando para evitar crear otro solo por un SELECT
                    command.Parameters.Clear();
                    string selectThisId = "SELECT IdSale FROM Sales WHERE PurchaseNum = @purchaseNum";
                    command.CommandText = selectThisId;
                    command.Parameters.AddWithValue("@purchaseNum", purchaseNum);
                    thisIdSale = Convert.ToInt32(command.ExecuteScalar());                    
                }

                DB_SaleLine.InsertSalesLinesTest(connectionWithDB, transaction, thisIdSale, purchaseNum, purchasedCart);

                // Commitemos tanto la inserción en DB_Sale y DB_SaleLine
                transaction.Commit();
                
            } catch (Exception ex) {
                transaction.Rollback();

                // Log de error
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            } finally {
                connectionWithDB.Close();
            }

            return thisIdSale;
        }

    }    
}