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
    //cuando heredamos de ControllerBase, la clase ahora puede manejar solicitudes HTTP
    public class productsController : ControllerBase{                
        
        [HttpGet("product/category")]
        public IActionResult GetProductsByCategory(int category){
            try{
                ProductsLogic productsLogic = new ProductsLogic();
                IEnumerable<Product> filteredProducts = productsLogic.filterProductsByCategory(category);                
                return Ok(filteredProducts);
                
            //501 son para NotImplemented o Excepciones Propias
            }catch (BussinessException ex){                                
                return StatusCode(501, "Ha ocurrido un error al obtener los datos. Por favor inténtalo más tarde. ");
            }
            catch (Exception ex){                
                return StatusCode(500, "Ha ocurrido un error al obtener los datos. Por favor inténtalo más tarde.");
            }   
        }                

        [HttpGet("product/search/")]
        public IActionResult GetProductsBySearchAndCategory([FromQuery]string searchText, [FromQuery]int[] categoryIds){

            try{
                ProductsLogic productsLogic = new ProductsLogic();
                //Las validaciones se hacen en ProductsLogic
                IEnumerable<Product> filteredProducts = productsLogic.filterProductsBySearchTextAndCategory(searchText,categoryIds);
                return Ok(filteredProducts);
                
            //501 son para NotImplemented o Excepciones Propias
            }catch (BussinessException ex){                                
                return StatusCode(501, "Ha ocurrido un error al obtener los datos. Por favor inténtalo más tarde. ");
            }
            catch (Exception ex){                
                return StatusCode(500, "Ha ocurrido un error al obtener los datos. Por favor inténtalo más tarde.");
            }   
        }
    }            
}
