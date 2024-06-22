using System;
using System.Globalization;
using System.Transactions;
using System.Collections.Generic;//para usar list
//API
using MyStoreAPI.Models;
using MySqlConnector;
using System.Runtime.InteropServices.Marshalling;
using Core;
namespace MyStoreAPI.DB{    

    public class DB_Notification{

        public async Task<IEnumerable<Notification>> getNotificationsForUsersAsync(){


            List<Notification>  listOfNotifications =new List<Notification>();
            MySqlConnection connectionWithDB = null;
            MySqlTransaction transaction = null;
            
            try{
                connectionWithDB = new MySqlConnection(DB_Connection.INIT_CONNECTION_DB());                
                await connectionWithDB.OpenAsync();
                transaction = await connectionWithDB.BeginTransactionAsync();

                string selectNotifications = @"
                SELECT Id, Title, Message, Creation_Date
                FROM Notifications;";

                using(MySqlCommand command = new MySqlCommand(selectNotifications,connectionWithDB)){
                    
                    command.Transaction = transaction;                    
                    using (MySqlDataReader readerTable = await command.ExecuteReaderAsync()){
                        while(await readerTable.ReadAsync()){
                            var id = Convert.ToInt32(readerTable["Id"]);
                            var title = readerTable["Title"].ToString();
                            var message = readerTable["Message"].ToString();
                            var creationDate = (DateTime)readerTable["Creation_Date"];
                            var currentNotify = new Notification(id,title,message,creationDate);
                            listOfNotifications.Add(currentNotify);                            
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
            return listOfNotifications;
        }            

        public async Task InsertNotificationAsync(Notification newNotification){
            
            if(newNotification == null) throw new ArgumentException($"{nameof(newNotification)} no puede ser nulo");

            MySqlConnection connectionWithDB = null;
            MySqlTransaction transaction = null;

            try{
                connectionWithDB = new MySqlConnection(DB_Connection.INIT_CONNECTION_DB());                
                await connectionWithDB.OpenAsync();
                transaction = await connectionWithDB.BeginTransactionAsync();
                
                //Hacemos el insert
                string insertNotification = @"
                INSERT INTO Notifications (Title, Message, Creation_Date)
                VALUES (@title, @message, @creationDate);
                ";
                using(MySqlCommand command = new MySqlCommand(insertNotification,connectionWithDB)){

                    //asociamos las acciones a realizar con una transaction
                    command.Transaction = transaction;                        
                    command.Parameters.AddWithValue("@title", newNotification.notifyTitle);
                    command.Parameters.AddWithValue("@message", newNotification.notifyMessage);
                    command.Parameters.AddWithValue("@creationDate", newNotification.notifyCreationDate);
                    await command.ExecuteNonQueryAsync();

                    // Despues de la insercion, obtenemos el ID recién insertado, pero en la misma conexion (mismo bloqueo)
                    int thisIdNotification = 0;
                    string selectLastInsertedId = "SELECT LAST_INSERT_ID();";
                    using (MySqlCommand idCommand = new MySqlCommand(selectLastInsertedId, connectionWithDB)){
                        thisIdNotification = Convert.ToInt32(await idCommand.ExecuteScalarAsync());
                    }

                    //Devolvemos el id de la notificacion generada (porque es IDENTITY(1,1))                    
                    await InsertNotificationCopyAsync(connectionWithDB, transaction, thisIdNotification, newNotification);
                    await transaction.CommitAsync();
                }               
            }catch(Exception ex){
                await transaction.RollbackAsync();
                //Mandamos el eroro a NotificationLogic
                throw new Exception("Error al insertar notificación", ex);

            }finally{
                await connectionWithDB.CloseAsync();
            }            
        }

        public async Task InsertNotificationCopyAsync(MySqlConnection connectionWithDB, MySqlTransaction transaction,int thisIdNotification, Notification newNotification){

            if (connectionWithDB == null) throw new ArgumentException($"{nameof(connectionWithDB)} la conexión no puede ser nula");
            if (transaction == null) throw new ArgumentException($"{nameof(transaction)} la transactión no puede ser nula");            
            if (thisIdNotification <= 0) throw new ArgumentException($"{nameof(thisIdNotification)} debe ser un valor positivo");
            if (newNotification == null) throw new ArgumentException($"{nameof(newNotification)} no puede ser nulo");

            try{                

                //Hacemos el insert                                
                string insertNotification = @"
                INSERT INTO Notifications (Id,Title, Message, Creation_Date)
                VALUES (@id, @title, @message, @creationDate);
                ";
                using(MySqlCommand command = new MySqlCommand(insertNotification,connectionWithDB)){

                    //asociamos las acciones a realizar con una transaction
                    command.Transaction = transaction;
                    command.Parameters.AddWithValue("@id", thisIdNotification);
                    command.Parameters.AddWithValue("@title", newNotification.notifyTitle);
                    command.Parameters.AddWithValue("@message", newNotification.notifyMessage);
                    command.Parameters.AddWithValue("@creationDate", newNotification.notifyCreationDate);
                    await command.ExecuteNonQueryAsync();                    
                }
                //No es necesario manejar commit o rollback aqui, porque InsertNotificationAsync lo hace
            }catch(Exception ex){

                throw new Exception("Error al insertar copia de notificación", ex);
            }
        }        
    }
}