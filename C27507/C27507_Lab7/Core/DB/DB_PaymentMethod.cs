
using System;
using System.Transactions;
using System.Collections.Generic;//para usar list
//API
using MyStoreAPI.Models;
using MySqlConnector;
namespace MyStoreAPI.DB
{
    public class DB_PaymentMethod{


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
    }
}