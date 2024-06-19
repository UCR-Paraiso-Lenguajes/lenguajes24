using StoreAPI.models;
using StoreAPI.Database;
using StoreAPI;

namespace StoreAPI.Business
{
    public class ProductLogic
    {
        private ProductBD productBD;
        internal delegate void OnNewProduct(Product product);

        public ProductLogic()
        {
            productBD = new ProductBD();
        }

        // Delegate para manejar la actualización de la lista de productos
        OnNewProduct onNewProduct = (product) =>
        {
            Products.Instance.AddNewProduct(product);
            int nextId = Store.Instance.Value.Result.Products.Count + 1; // Obtener próximo ID disponible
            product.Id = nextId;
            Store.Instance.Value.Result.Products.Add(product);// Asegurar que la lista de productos en Store también se actualice
        };

        public async Task<Product> AddProduct(Product product)
        {
            if (product == null) throw new ("The product cannot be null.");
            Product productInsert = product;
            return await productBD.InsertProductAsync(productInsert, onNewProduct);
        }


    }

}