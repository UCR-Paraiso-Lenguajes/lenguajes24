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

        public async Task<IEnumerable<Notification>> getNotificationsForAdminsAsync(){

            List<Notification>  listOfNotifications =new List<Notification>();
            MySqlConnection connectionWithDB = null;
            MySqlTransaction transaction = null;
            
            try{
                connectionWithDB = new MySqlConnection(DB_Connection.INIT_CONNECTION_DB());                
                await connectionWithDB.OpenAsync();
                transaction = await connectionWithDB.BeginTransactionAsync();

                string selectNotifications = @"
                    SELECT Id, Title, Message, Creation_Date, Status
                    FROM Notifications
                    ORDER BY Status DESC, Creation_Date DESC;
                ";

                using(MySqlCommand command = new MySqlCommand(selectNotifications,connectionWithDB)){
                    
                    command.Transaction = transaction;                    
                    using (MySqlDataReader readerTable = await command.ExecuteReaderAsync()){
                        while(await readerTable.ReadAsync()){
                            var id = Convert.ToInt32(readerTable["Id"]);
                            var title = readerTable["Title"].ToString();
                            var message = readerTable["Message"].ToString();
                            var creationDate = (DateTime)readerTable["Creation_Date"];
                            var status = Convert.ToInt32(readerTable["Status"]);                            
                            var currentNotify = new Notification(id, title, message, creationDate, status);
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

        public async Task<IEnumerable<Notification>> getNotificationsForUsersAsync(){

            List<Notification>  listOfNotifications =new List<Notification>();
            MySqlConnection connectionWithDB = null;
            MySqlTransaction transaction = null;
            
            try{
                connectionWithDB = new MySqlConnection(DB_Connection.INIT_CONNECTION_DB());                
                await connectionWithDB.OpenAsync();
                transaction = await connectionWithDB.BeginTransactionAsync();

                string selectNotifications = @"
                    SELECT Id, Title, Message, Creation_Date, Status
                    FROM Notifications
                    WHERE Status <> 0
                    ORDER BY Creation_Date DESC
                    LIMIT 3;
                ";

                using(MySqlCommand command = new MySqlCommand(selectNotifications,connectionWithDB)){
                    
                    command.Transaction = transaction;                    
                    using (MySqlDataReader readerTable = await command.ExecuteReaderAsync()){
                        while(await readerTable.ReadAsync()){
                            var id = Convert.ToInt32(readerTable["Id"]);
                            var title = readerTable["Title"].ToString();
                            var message = readerTable["Message"].ToString();
                            var creationDate = (DateTime)readerTable["Creation_Date"];
                            var status = Convert.ToInt32(readerTable["Status"]);                            
                            var currentNotify = new Notification(id, title, message, creationDate, status);
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


        public async Task DeleteOneNotificationAsync(int notifyToDelete){
            if(notifyToDelete <= 0 ) throw new ArgumentException($"{nameof(notifyToDelete)} no puede ser nulo");
            MySqlConnection connectionWithDB = null;
            MySqlTransaction transaction = null;

            try{
                connectionWithDB = new MySqlConnection(DB_Connection.INIT_CONNECTION_DB());                
                await connectionWithDB.OpenAsync();
                transaction = await connectionWithDB.BeginTransactionAsync();

                string updateNotification = @"
                    UPDATE Notifications
                    SET Status = 0
                    WHERE Id = @notificationId;
                ";

                using (MySqlCommand command = new MySqlCommand(updateNotification, connectionWithDB)){
                    command.Transaction = transaction;
                    command.Parameters.AddWithValue("@notificationId", notifyToDelete);
                    await command.ExecuteNonQueryAsync();                                        
                    await transaction.CommitAsync();
                }
            }catch(Exception ex){
                await transaction.RollbackAsync();
                //Mandamos el eroro a NotificationLogic
                throw new Exception("Error al borrar notificación", ex);

            }finally{
                await connectionWithDB.CloseAsync();
            }            
        }

        public async Task<Notification> InsertNotificationAsync(Notification newNotification){
            
            if(newNotification == null) throw new ArgumentException($"{nameof(newNotification)} no puede ser nulo");

            Notification insertedNotification = null;

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
                        idCommand.Transaction = transaction;
                        object result = await idCommand.ExecuteScalarAsync();
                        thisIdNotification = Convert.ToInt32(await idCommand.ExecuteScalarAsync());

                        // Creamos una nueva instancia de Notification con el ID asignado
                        var notifyId = thisIdNotification;
                        var notifyTitle = newNotification.notifyTitle;
                        var  notifyMessage = newNotification.notifyMessage;
                        var notifyCreationDate = newNotification.notifyCreationDate;
                        insertedNotification = new Notification(notifyId,notifyTitle,notifyMessage,notifyCreationDate,1);
                    }                    

                    await transaction.CommitAsync();
                }               
            }catch(Exception ex){
                await transaction.RollbackAsync();
                //Mandamos el eroro a NotificationLogic
                throw new Exception("Error al insertar notificación", ex);

            }finally{
                await connectionWithDB.CloseAsync();
            }
            return newNotification;
        }
    }
}