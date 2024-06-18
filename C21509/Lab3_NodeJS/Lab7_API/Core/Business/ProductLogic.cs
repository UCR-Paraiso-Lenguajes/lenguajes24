using Store_API.Database;
using Store_API.Models;

namespace Core.Business
{
    public class ProductLogic
    {
        public DB_API db_product = new DB_API();
        public delegate void InsertedProductDelegate(Product newProduct);
        public InsertedProductDelegate referenceProductInserted;

        public async Task<bool> insertProductAsync(Product newProduct)
        {
            try
            {
                await db_product.InsertionProductInDBAsync(newProduct);
                referenceProductInserted?.Invoke(newProduct);
                return true;
            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentNullException("El producto no puede ser nulo: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error inesperado: " + ex.Message, ex);
            }
        }

        public async Task<bool> insertionProductAsyncUT(Product newProduct)
        {
            referenceProductInserted?.Invoke(newProduct);
            return true;
        }
    }
}