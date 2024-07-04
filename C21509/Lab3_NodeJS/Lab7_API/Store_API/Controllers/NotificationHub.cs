using Store_API.Business;
using Store_API.Database;
using Core.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Store_API.Controllers{

    public class NotificationHub : Hub{

        public async Task SendNotificationAsync(Notifications currentNotify){

            await Clients.All.SendAsync("Receive",currentNotify);
        }

        public async Task NotifyDeletionIdAsync(int notificationDeletedId){
 
            await Clients.All.SendAsync("Delete",notificationDeletedId);
        }
    }
}