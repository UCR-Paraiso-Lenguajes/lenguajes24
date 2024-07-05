
using System;
using System.Transactions;
using System.Collections.Generic;//para usar list
//API
using MyStoreAPI.Models;
using MySqlConnector;
namespace MyStoreAPI.DB
{
    public class DB_PaymentMethod{

        public DB_PaymentMethod(){}


        //Metodo Estatico y Sincrono
        //Se usa una sola vez en Store en caso de que no existan metodo de pagos al crear o cargar la base de datos
        public static void InsertPaymentMethod(){
            try{
                using (TransactionScope scopeTransaction = new TransactionScope()){
                    using (MySqlConnection connectionWithDB = new MySqlConnection(DB_Connection.INIT_CONNECTION_DB())){
                        connectionWithDB.Open();                                        
                        string insertIntoPaymentMethodTable =
                            @"INSERT INTO PaymentMethod(IdPayment, Description,Verification) 
                            VALUES(@idPayment,@description,@verification)";                    
                        
                        using (MySqlCommand command = new MySqlCommand(insertIntoPaymentMethodTable,connectionWithDB)){
                            
                            string paymentDescription = "";
                            foreach (var actualPaymentMethod in PaymentMethods.paymentMethodsList){

                                command.Parameters.Clear();
                                command.Parameters.AddWithValue("@idPayment",actualPaymentMethod.payment);
                                //obtenemos la descripcion del Enum                                                        
                                paymentDescription = Enum.GetName(typeof(PaymentMethodNumber), actualPaymentMethod.payment);
                                command.Parameters.AddWithValue("@description",paymentDescription);
                                command.Parameters.AddWithValue("@verification",actualPaymentMethod.verify);                                                        
                                command.ExecuteNonQuery();                            
                            }
                        }                    
                    }             
                    scopeTransaction.Complete();
                }

            }catch (Exception ex){
                throw;
            }                    
        }   

        //Metodo Estatico y Sincrono
        //Se usa una sola vez en Store en caso de que no existan metodo de pagos al crear o cargar la base de datos
        public static bool PaymentMethodsInTableExist(){
            try{
                using (MySqlConnection connectionWithDB = new MySqlConnection(DB_Connection.INIT_CONNECTION_DB()))
                {
                    connectionWithDB.Open();
                    string numberOfPaymentMethods = "SELECT COUNT(*) FROM PaymentMethod";
                    using (MySqlCommand command = new MySqlCommand(numberOfPaymentMethods, connectionWithDB)){
                        int count = Convert.ToInt32(command.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
            catch (Exception ex){                
                throw;
            }
        }

        public static IEnumerable<PaymentMethod> GetAllPaymentMethods(){
            List<PaymentMethod> paymentMethodsList = new List<PaymentMethod>();
            try{
                using (MySqlConnection connectionWithDB = new MySqlConnection(DB_Connection.INIT_CONNECTION_DB())){
                    connectionWithDB.Open();
                    string query = "SELECT IdPayment, Description, Verification FROM PaymentMethod";
                    using (MySqlCommand command = new MySqlCommand(query, connectionWithDB)){
                        using (MySqlDataReader reader = command.ExecuteReader()){
                            while (reader.Read()){
                                PaymentMethod paymentMethod = new PaymentMethod(
                                    (PaymentMethodNumber)reader.GetInt32("IdPayment"),
                                    reader.GetBoolean("Verification")
                                );
                                paymentMethodsList.Add(paymentMethod);
                            }
                        }
                    }
                }
            }
            catch (Exception ex){
                throw;
            }
            return paymentMethodsList;
        }


        public async Task<IEnumerable<PaymentMethod>> GetAllPaymentMethodsAsync(){
            List<PaymentMethod> paymentMethodsList = new List<PaymentMethod>();
            try{
                using (MySqlConnection connectionWithDB = new MySqlConnection(DB_Connection.INIT_CONNECTION_DB())){
                    await connectionWithDB.OpenAsync();

                    string query = "SELECT IdPayment, Description, Verification FROM PaymentMethod";

                    using (MySqlCommand command = new MySqlCommand(query, connectionWithDB)){
                        using (MySqlDataReader reader = await command.ExecuteReaderAsync()){
                            while (await reader.ReadAsync()){
                                PaymentMethod paymentMethod = new PaymentMethod(
                                    (PaymentMethodNumber)reader.GetInt32("IdPayment"),
                                    reader.GetBoolean("Verification")
                                );

                                paymentMethodsList.Add(paymentMethod);
                            }
                        }
                    }
                }
            }
            catch (Exception ex){
                throw new ApplicationException("Error al obtener los métodos de pago desde la base de datos.", ex);
            }
            return paymentMethodsList;
        }

        public async Task UpdatePaymentMethodStatusAsync(int idPayment, int newStatus){
            try{
                string connectionString = DB_Connection.INIT_CONNECTION_DB();
                
                using (MySqlConnection connectionWithDB = new MySqlConnection(connectionString)){
                    await connectionWithDB.OpenAsync();

                    string query = "UPDATE PaymentMethod SET Verification = @verification WHERE IdPayment = @idPayment";

                    bool verification = false;   
                    if(newStatus == 1){
                        verification = true;
                    }else{
                        verification = false;
                    }
                    
                    using (MySqlCommand command = new MySqlCommand(query, connectionWithDB)){
                        command.Parameters.AddWithValue("@idPayment", idPayment);
                        command.Parameters.AddWithValue("@verification", verification);
                        
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex){
                throw new Exception("Error al actualizar el estado del método de pago.", ex);
            }
        }
    }
}