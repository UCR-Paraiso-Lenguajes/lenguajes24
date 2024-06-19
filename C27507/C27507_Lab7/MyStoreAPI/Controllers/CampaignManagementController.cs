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
        [Authorize(Roles = "Admin, Operator")]

        public async Task<IActionResult> CreateCartAsync([FromBody] Notification newNotify){

            try{
                CampaignLogic campaignLogic = new CampaignLogic();
                
                var succesNotification = await campaignLogic.createNewNotificationAsync(newNotify);
                return Ok(new { succesNotification });
                 }
            catch (BussinessException ex){                
                return StatusCode(501, "Ha ocurrido un error al generar la notificación. Por favor inténtalo más tarde.");
            }
            catch (Exception ex){                                             
                //Otros posibles errores
                return StatusCode(500, "Ha ocurrido un error al generar la notificación. Por favor inténtalo más tarde.");
            }
        }
        
    }
}