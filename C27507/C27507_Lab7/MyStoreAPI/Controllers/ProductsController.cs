﻿using Core;
using Microsoft.AspNetCore.Mvc;
//API
using MyStoreAPI.Business;
using MyStoreAPI.DB;
using MyStoreAPI.Models;
namespace MyStoreAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    //cuando heredamos de ControllerBase, la clase ahora puede manejar solicitudes HTTP
    public class productsController : ControllerBase{                
        
        [HttpGet("store/product/")]
        public IActionResult GetProductsByCategory(int category){
            try{
                ProductsLogic productsLogic = new ProductsLogic();
                IEnumerable<Product> filteredProducts = productsLogic.filterProductsByCategory(category);
                int[] ids = new int[] {1,2,3};
                var filteredProducts2 = productsLogic.filterProductsBySearchTextAndCategory("ps",ids);
                return Ok(filteredProducts);
                
            //501 son para NotImplemented o Excepciones Propias
            }catch (BussinessException ex){                                
                return StatusCode(501, "Ha ocurrido un error al obtener los datos. Por favor inténtalo más tarde. ");
            }
            catch (Exception ex){                
                return StatusCode(500, "Ha ocurrido un error al obtener los datos. Por favor inténtalo más tarde.");
            }   
        }                

        [HttpGet("store/product/search/")]
        public IActionResult GetProductsBySearchAndCategory(string searchText){

            try{
                int[] arrayOfIds = new int[] {1,2,3};
                ProductsLogic productsLogic = new ProductsLogic();
                IEnumerable<Product> filteredProducts = productsLogic.filterProductsBySearchAndCategory(searchText,arrayOfIds);
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
