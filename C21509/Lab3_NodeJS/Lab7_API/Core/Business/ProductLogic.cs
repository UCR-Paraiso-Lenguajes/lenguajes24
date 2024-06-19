using Store_API.Database;
using Store_API.Models;

namespace Core.Business
{
    public class ProductLogic
    {
        public DB_API db_product = new DB_API();
        public delegate void InsertedProductDelegate(Product newProduct);
        public InsertedProductDelegate referenceProductInserted;

        public async Task<bool> insertProductAsync(Product newProduct, InsertedProductDelegate  referenceProductInserted)
        {
            try
            {
                await db_product.InsertionProductInDBAsync(newProduct);
                referenceProductInserted?.Invoke(newProduct);
                return true;
            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentNullException("Product cannot be null: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("There was an error: " + ex.Message, ex);
            }
        }

         public async Task<bool> deleteProductAsync(int productId)
        {
            try
            {
                bool deleteStatus = await db_product.DeleteProductFromDBAsync(productId);
                return deleteStatus;
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException("Invalid product ID: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("There was an error: " + ex.Message, ex);
            }
        }

        public async Task<bool> insertionProductAsyncUT(Product newProduct, InsertedProductDelegate  referenceProductInserted)
        {
            referenceProductInserted?.Invoke(newProduct);
            return true;
        }
    }
}