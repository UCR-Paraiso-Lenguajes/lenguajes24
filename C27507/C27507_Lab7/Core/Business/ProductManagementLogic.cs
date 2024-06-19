//API
using Core;
using MyStoreAPI.DB;
using MyStoreAPI.Models;

namespace MyStoreAPI.Business
{
    public class ProductManagementLogic{

        public DB_Product db_product = new DB_Product();
        public delegate void ProductInsertedDelegate(Product newProduct);
        public ProductInsertedDelegate onProductInserted;
        
        public async Task<bool> insertProductAsync(Product newProduct,ProductInsertedDelegate onProductInserted){
            try{
                await db_product.InsertProductInDBAsync(newProduct);
                //Si no hay errores significa que la inserción en la bd fue exitosa, 
                onProductInserted?.Invoke(newProduct);
                return true;

            }catch(BussinessException ex){
                throw new BussinessException("La inserción en la base de datos no se puede realizar");
                return false;
            }
        }

        public async Task<bool> insertProductAsyncUT(Product newProduct,ProductInsertedDelegate onProductInserted){            
            onProductInserted?.Invoke(newProduct);
            return true;
        }
    }
}
