namespace ApiLab7;

public class ProductBusiness
{
    private ProductData productData;
    internal delegate void OnNewProduct(Product product);

    public ProductBusiness()
    {
        productData = new ProductData();
    }

    OnNewProduct onNewProduct = (product) =>
    {
        Products.Instance.AddNewProduct(product);
    };

    public async Task<Product> AddProduct(ProductDTO product)
    {
        ValidateProduct(product);
        Product productInsert = product.ToProduct();
        return await productData.InsertProductAsync(productInsert, onNewProduct);
    }

    private void ValidateProduct(ProductDTO product)
    {
        if(String.IsNullOrWhiteSpace(product.Name)) throw new ArgumentException("A product requires a name");
        if(String.IsNullOrWhiteSpace(product.ImageUrl)) throw new ArgumentException("At least one image URL is expected");
        if(String.IsNullOrWhiteSpace(product.Description)) throw new ArgumentException("A description is required");
        if(product.Price <= 0) throw new ArgumentException("The price of the product should be above 0");
        if(product.Category <= 0) throw new ArgumentException("The category of the product should be above 0");
    }
}