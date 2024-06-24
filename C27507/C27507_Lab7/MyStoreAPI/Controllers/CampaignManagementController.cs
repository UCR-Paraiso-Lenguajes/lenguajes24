using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
//API
using MyStoreAPI.Business;
using MyStoreAPI.DB;
using MyStoreAPI.Models;
using Core;

//JWT Authentication
using Microsoft.AspNetCore.Authorization;

namespace MyStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampaignManagementController : ControllerBase{
        
        [HttpPost("campaign/insert")]
        //[Authorize(Roles = "Admin, Operator")]

        public async Task<IActionResult> CreateNotificationAsync([FromBody] Notification newNotify){

            try{
                CampaignLogic campaignLogic = new CampaignLogic();                                
                await campaignLogic.createNewNotificationAsync(newNotify);
                return Ok(true);
            }
            catch (BussinessException ex){                
                return StatusCode(501, "Ha ocurrido un error al generar la notificación. Por favor inténtalo más tarde.");
            }
            catch (Exception ex){                                             
                //Otros posibles errores
                return StatusCode(500, "Ha ocurrido un error al generar la notificación. Por favor inténtalo más tarde.");
            }
        }

        [HttpDelete("campaign/delete/{idNotification}")]
        //[Authorize(Roles = "Admin, Operator")]

        public async Task<IActionResult> DeleteNotificationAsync( int idNotification){
            
            try{
                CampaignLogic campaignLogic = new CampaignLogic();
                
                await campaignLogic.deleteNotificationAsync(idNotification);
                return Ok(true);
            }
            catch (BussinessException ex){                
                return StatusCode(501, "Ha ocurrido un error al borrar la notificación. Por favor inténtalo más tarde." + ex);
            }
            catch (Exception ex){                                             
                //Otros posibles errores
                return StatusCode(500, "Ha ocurrido un error al borrar la notificación. Por favor inténtalo más tarde." + ex);
            }
        }

        [HttpGet("campaign/select")]
        //[Authorize(Roles = "Admin, Operator")]

        public async Task<IActionResult> SelectNotificationAsync(){

            try{
                CampaignLogic campaignLogic = new CampaignLogic();
                
                var allNotificacionsForAdmin = await campaignLogic.selectAllNotifications();
                return Ok(allNotificacionsForAdmin);
            }
            catch (BussinessException ex){                
                return StatusCode(501, "Ha ocurrido un error al borrar la notificación. Por favor inténtalo más tarde." + ex);
            }
            catch (Exception ex){                                             
                //Otros posibles errores
                return StatusCode(500, "Ha ocurrido un error al borrar la notificación. Por favor inténtalo más tarde."  + ex);
            }
        }
        
    }
}