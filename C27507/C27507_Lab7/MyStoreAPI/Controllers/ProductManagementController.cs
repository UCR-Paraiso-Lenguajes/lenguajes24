using Core;
using Microsoft.AspNetCore.Mvc;
//API
using MyStoreAPI.Business;
using MyStoreAPI.DB;
using MyStoreAPI.Models;
namespace MyStoreAPI.Controllers
{
    [Route("store/[controller]")]
    [ApiController]
    public class ProductManagementController : ControllerBase{

        [HttpPost("product/insert")]
        [Authorize(Roles = "Admin, Operator")]
        public IActionResult InsertNewProductInStore([FromBody] Product newProduct){
            try{
                ProductManagementLogic productManagementLogic = new ProductManagementLogic();
                //Hacemos que al delegate se le asigne la funcion de actualizar la lista en memoria
                productsLogic.OnProductInserted = Store.Instance.addNewProductInStore;
                //Dentro de insertProductAsync() se ejecutaria el OnProductInserted
                bool insertedProductStatus = productManagementLogic.insertProductAsync(newProduct);
                return Ok(insertedProductStatus);
                
            //501 son para NotImplemented o Excepciones Propias
            }catch (BussinessException ex){                                
                return StatusCode(501, "Ha ocurrido un error al agregar el nuevo carrito. Por favor inténtalo más tarde. ");
            }
            catch (Exception ex){                
                return StatusCode(500, "Ha ocurrido un error al agregar el nuevo carrito. Por favor inténtalo más tarde.");
            }   
        }
        
    }
}