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
    public class PaymentMethodManagementController : Controller{        

        private IHubContext<NotificationHub> notificationHubContext;
                
        [HttpGet("payment/select")]        
        public async Task<IActionResult> GetAllPaymentMethodsToAdminAsync(){

            try{
                // PaymentMethodManagementLogic paymentLogic = new PaymentMethodManagementLogic();
                // var listOfPaymentMethod = await paymentLogic.GetAllPaymentMethodsFromDBAsync();
                // return Ok(listOfPaymentMethod);
                return Ok(Store.Instance.AllPaymentMethods);
            }
            catch (BussinessException ex){                
                return StatusCode(501, "Ha ocurrido un error al obtener los medios de pago. Por favor inténtalo más tarde." + ex);
            }
            catch (Exception ex){                                             
                //Otros posibles errores
                return StatusCode(500, "Ha ocurrido un error al obtener los medios de pago. Por favor inténtalo más tarde." + ex);
            }
        }        

        [HttpPost("payment/update")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdatePaymentMethodAsync([FromBody] UpdatePaymentMethodModel model){
            
             try{
                
                PaymentMethodManagementLogic paymentLogic = new PaymentMethodManagementLogic();
                await paymentLogic.UpdatePaymentMethodStatusInBDAsync(model.PaymentMethodId, model.NewStatus,Store.Instance.updatePaymentMethodInStore);
                                                            
                return Ok();
            }
            catch (BussinessException ex)
            {
                return StatusCode(501, "Ha ocurrido un error al actualizar el método de pago. Por favor inténtalo más tarde. " + ex.Message);
            }
            catch (Exception ex)
            {
                // Otros posibles errores
                return StatusCode(500, "Ha ocurrido un error al actualizar el método de pago. Por favor inténtalo más tarde. " + ex.Message);
            }            
        }
    }

    public class UpdatePaymentMethodModel{
        public int PaymentMethodId { get; set; }
        public int NewStatus { get; set; }
    }
}