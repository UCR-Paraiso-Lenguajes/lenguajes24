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
        
        public async Task<Product> insertProductAsync(Product newProduct){
            await db_product.InsertProductInDBAsync(newProduct);
            //Si no hay errores 
            onProductInserted?.Invoke(newProduct);
            return newProduct;
        }
    }
}
