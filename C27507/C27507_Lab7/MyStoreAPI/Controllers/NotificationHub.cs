using MyStoreAPI.Business;
using MyStoreAPI.DB;
using MyStoreAPI.Models;
using Core;
//Websockets
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace MyStoreAPI.Controllers{

    public class NotificationHub : Hub{

        //Funcion llamadas por los usuarios
        public async Task receiveNotificationsAsync(IEnumerable<Notification> listOfnotifications){

            //Fucnion de propia de SignalR, donde el primer parametro es el
            //codigo para reconocerse entre cliente y servidor;cy los demas son datos
            //para los clientes
            await Clients.All.SendAsync("Receive",listOfnotifications);
        }

    }

}