//API
using Core;
using MyStoreAPI.DB;
using MyStoreAPI.Models;

namespace MyStoreAPI.Business
{

    public class CampaignLogic{

        public DB_Notification db_notification;

        public CampaignLogic(){
            db_notification = new DB_Notification(); // Asegúrate de inicializar db_notification aquí o pásalo por dependencia
        }


        public async Task<Notification> createNewNotificationAsync(Notification newNotify){
                        
            if(newNotify == null)
                throw new BussinessException($"{nameof(newNotify)} no puede ser nulo");

            if (newNotify.notifyCreationDate == DateTime.MinValue) 
                throw new BussinessException($"{nameof(newNotify.notifyCreationDate)} es fecha no valida");
            
            Notification insertedNotification = await db_notification.InsertNotificationAsync(newNotify);
            if(insertedNotification == null)
                throw new BussinessException($"{nameof(insertedNotification)} no puede ser nulo");

            return insertedNotification;
        }

        public async Task deleteNotificationAsync(int notifyToDelete){

            if(notifyToDelete == null)
                throw new BussinessException($"{nameof(notifyToDelete)} no puede ser nulo");

            if(notifyToDelete <= 0)
                throw new BussinessException($"{nameof(notifyToDelete)} no puede ser nulo");

            await db_notification.DeleteOneNotificationAsync(notifyToDelete);            
        }

        public async Task<IEnumerable<Notification>> selectAllNotifications(){

            IEnumerable<Notification> allNotifications = await db_notification.getNotificationsForAdminsAsync();
            if (allNotifications == null)
                throw new BussinessException($"{nameof(allNotifications)} puede ser 0, pero no nula");

            foreach (var notification in allNotifications){
                if (notification.notifyId <= 0)
                    throw new BussinessException($"{nameof(notification)} debe contener un notifyId entero positivo.");

                if (string.IsNullOrEmpty(notification.notifyTitle))
                    throw new BussinessException($"{nameof(notification)} no puede contener un título nulo o vacío.");

                if (string.IsNullOrEmpty(notification.notifyMessage))
                    throw new BussinessException($"{nameof(notification)} no puede contener un mensaje nulo o vacío.");

                if (notification.notifyCreationDate == DateTime.MinValue)
                    throw new BussinessException($"{nameof(notification)} debe contener una fecha de creación válida.");
            }
            return allNotifications;
        }
        
    }
}