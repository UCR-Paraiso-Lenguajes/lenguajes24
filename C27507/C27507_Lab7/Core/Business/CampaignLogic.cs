//API
using Core;
using MyStoreAPI.DB;
using MyStoreAPI.Models;

namespace MyStoreAPI.Business
{

    public class CampaignLogic{

        public DB_Notification db_notification;

        public CampaignLogic(){}


        public async Task createNewNotificationAsync(Notification newNotify){

            if(newNotify == null)
                throw new BussinessException($"{nameof(newNotify)} no puede ser nulo");

            await db_notification.InsertNotificationAsync(newNotify);            
        }

        // public async Task deleteNotificationAsync(Notification notifyToDelete){

        //     if(notifyToDelete == null)
        //         throw new BussinessException($"{nameof(notifyToDelete)} no puede ser nulo");

        //     await db_notification.DeleteNotificationAsync(notifyToDelete);            
        // }
        
    }
}