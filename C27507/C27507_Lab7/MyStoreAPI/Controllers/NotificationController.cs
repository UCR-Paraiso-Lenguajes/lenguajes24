using Core;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

//API
using MyStoreAPI.Business;
using MyStoreAPI.DB;
using MyStoreAPI.Models;
//JWT Authentication
using Microsoft.AspNetCore.Authorization;
//Websockets
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;



namespace MyStoreAPI.Controllers
{    
    [Route("api/[controller]")]
    [ApiController]            
    public class NotificationController : Controller{        

        private IHubContext<NotificationHub> notificationHubContext;
        private NotificationLogic notificationLogic;

        //Con la instancia de IHuContext ahora se pueden manejar los metodos de NotificationHub
        public NotificationController(IHubContext<NotificationHub> notificationHubContext,NotificationLogic notificationLogic){
            this.notificationHubContext = notificationHubContext;
            this.notificationLogic = notificationLogic;
        }

        [HttpPost]
        [AllowAnonymous]
        [Consumes("application/json")]
        public async Task<IActionResult> getNotificationsAsync(){

            try{                
                IEnumerable<Notification> listOfnotifications = await notificationLogic.getNotificationFromDBAsync();
                await notificationHubContext.Clients.All.SendAsync("Receive",listOfnotifications);
                return Ok();
            }
            catch (BussinessException ex){                
                return StatusCode(501, "Por el momento no hay notificaciones que mostrar");
            }
            catch (Exception ex){                                             
                //Otros posibles errores
                return StatusCode(500, "Por el momento no hay notificaciones que mostrar");
            }
            

            // try{
            //     IEnumerable<Notification> listOfnotifications = await notificationLogic.getNotificationFromDBAsync();                

            // }
            // catch (BussinessException ex){                
            //     return StatusCode(501, "Por el momento no hay notificaciones");
            // }
            // catch (Exception ex){                                             
            //     //Otros posibles errores
            //     return StatusCode(500, "Por el momento no hay notificaciones");
            // }
        }
    }
}
