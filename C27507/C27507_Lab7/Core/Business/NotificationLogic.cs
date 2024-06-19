//API
using Core;
using MyStoreAPI.DB;
using MyStoreAPI.Models;

namespace MyStoreAPI.Business
{
    public class NotificationLogic{

        private IEnumerable<Notification> someNotifications;
        private DB_Notification db_notify;
        
        public NotificationLogic(){
            this.db_notify = new DB_Notification();

        }

        public async Task<IEnumerable<Notification>> getNotificationFromDBAsync(){

            var notifications  = await db_notify.getNotificationsForUsersAsync();

            if(notifications  == null || !notifications .Any())
                throw new BussinessException($"{nameof(notifications )} no puede ser nulo o vacío");
            return notifications; 

        }
    }
}