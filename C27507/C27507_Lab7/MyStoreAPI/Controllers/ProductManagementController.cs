using Core;
using Microsoft.AspNetCore.Mvc;
//API
using MyStoreAPI.Business;
using MyStoreAPI.DB;
using MyStoreAPI.Models;

//JWT Authentication
using Microsoft.AspNetCore.Authorization;


namespace MyStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductManagementController : ControllerBase{

        [HttpPost("product/insert")]
        [Authorize(Roles = "Admin, Operator")]
        public async Task<IActionResult> InsertNewProductInStoreAsync([FromBody] Product newProduct){
            try{                
                ProductManagementLogic productManagementLogic = new ProductManagementLogic();                                
                bool insertedProductStatus = await productManagementLogic.insertProductAsync(newProduct, Store.Instance.addNewProductInStore);                
                return Ok( new {insertedProductStatus});
                
            //501 son para NotImplemented o Excepciones Propias
            }catch (BussinessException ex){                                
                return StatusCode(501, "Ha ocurrido un error al agregar el nuevo carrito. Por favor inténtalo más tarde. ");
            }
            catch (Exception ex){                
                return StatusCode(500, "Ha ocurrido un error al agregar el nuevo carrito. Por favor inténtalo más tarde.");
            }   
        }


        [HttpPost("product/delete")]
        [Authorize(Roles = "Admin, Operator")]
        public async Task<IActionResult> DeleteProductInDBAsync([FromBody] Product newProduct){
            try{
                string insertedProductStatus = "a";
                return Ok( new {insertedProductStatus});
                
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