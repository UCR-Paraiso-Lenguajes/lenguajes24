//API
using Core;
using MyStoreAPI.DB;
using MyStoreAPI.Models;

namespace MyStoreAPI.Business
{

    public class CampaignLogic{

        public DB_Notification db_notification

        public void CampaignLogic(){

        }


        public async Task<bool> createNewNotificationAsync(Notification newNotify){

            if(newNotify == null)
                throw new BussinessException($"{nameof(newNotify)} no puede ser nulo");

            bool insertedNotificationStatus = await db_notification.InsertNotificationAsync(newNotify);
            return insertedNotificationStatus;
        }

        public async Task<bool> deleteNotificationAsync(Notification notifyToDelete){

            if(notifyToDelete == null)
                throw new BussinessException($"{nameof(notifyToDelete)} no puede ser nulo");

            bool deleteddNotificationStatus = await db_notification.DeleteNotificationAsync(notifyToDelete);
            return deleteddNotificationStatus;
        }
        
    }
}