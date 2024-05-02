using System;
using System.Globalization;
using System.Transactions;
using System.Collections.Generic;//para usar list
//API
using MyStoreAPI.Models;
using MySqlConnector;
using System.Runtime.InteropServices.Marshalling;
using Core;
namespace MyStoreAPI.DB
{
    public class DB_Sale{
        
        public static (string, int) InsertSale(Cart purchasedCart){
            try{                                        

                using (TransactionScope scopeTransaction = new TransactionScope()){
                    
                    string purchaseNum = "";
                    int thisIdSale = 0;

                    //El bloque using(MySql....) es una buena practica ya que conecta y desconecta de la bd, liberando recursos
                    //y evitar dejando conexiones abiertas
                    using (MySqlConnection connectionWithDB = new MySqlConnection(DB_Connection.INIT_CONNECTION_DB())){
                        connectionWithDB.Open();                    

                        //Hacemos el insert
                        string insertSale = @"
                        INSERT INTO Sales (Total,PurchaseNum, Subtotal, Direction, IdPayment,DateSale)
                        VALUES (@total, @purchaseNum, @subtotal, @direction, @idPayment,@dateSale);
                        ";

                        //id global unico para el comprobante                    
                        purchaseNum = generateRandomPurchaseNum();
                        MySqlCommand command = new MySqlCommand(insertSale, connectionWithDB);
                        command.Parameters.AddWithValue("@total", purchasedCart.Total);
                        command.Parameters.AddWithValue("@purchaseNum", purchaseNum);
                        command.Parameters.AddWithValue("@subtotal", purchasedCart.Subtotal);
                        command.Parameters.AddWithValue("@direction", purchasedCart.Direction);                  
                        command.Parameters.AddWithValue("@idPayment", purchasedCart.PaymentMethod.payment);
                        command.Parameters.AddWithValue("@dateSale", DateTime.Now);
                        command.ExecuteNonQuery();

                         //Devolver el id de la venta generada (porque es IDENTITY(1,1))
                        string selectThisId = "SELECT IdSale FROM Sales WHERE PurchaseNum = @purchaseNum";
                        command = new MySqlCommand(selectThisId, connectionWithDB);
                        command.Parameters.AddWithValue("@purchaseNum", purchaseNum);
                        thisIdSale = Convert.ToInt32(command.ExecuteScalar());
                                            
                        DB_SaleLine.InsertSalesLine(connectionWithDB,purchaseNum,purchasedCart);
                    }     
                    //encapsula los metodos rollback y commit de Transaction
                    scopeTransaction.Complete();
                    Console.WriteLine("Exito al realizar la compra, guadado en Sales: ");    

                    //Si la transaccion se cumple con exito, devolvemos el codigo y el id para la instancia de Sale   
                    return (purchaseNum, thisIdSale);
                }
            }catch (Exception ex){
                Console.WriteLine("Error al generar InsertSale: " + ex);    
                throw;                
            }            
        }

        public static async Task<List<RegisteredSale>> GetRegisteredSalesByDayAsync(DateTime dateParameter){       

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

                            registeredSalesToday.Add(newRegisteredSale);
                        }
                    }
                }
                await transaction.CommitAsync();                

            }catch (Exception ex){                                
                Console.WriteLine("Mensaje desde DB_Sale: " + ex);
                transaction.Rollback();
                throw;

            }finally{
                connectionWithDB.Close();
            }            
            return registeredSalesToday;
        }

        public static async Task<List<RegisteredSaleWeek>> GetRegisteredSalesByWeekAsync(DateTime dateParameter){
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
                              
                            registeredSaleWeek.Add(salesByLastWeek);
                        }
                    }
                }
                await transaction.CommitAsync(); 


            }catch(Exception ex){

                Console.WriteLine("Mensaje desde DB_Sale: " + ex);
                transaction.Rollback();
                throw;

            }finally{
                connectionWithDB.Close();
            }            
            return registeredSaleWeek;
        }

        public static string generateRandomPurchaseNum(){            
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
    }    
}