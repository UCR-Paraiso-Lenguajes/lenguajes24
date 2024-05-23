using System.Drawing;
using ShopApi.Models;

public class ProductsLogic
{
    public IEnumerable<Product> products { get; }
    private Dictionary<int, List<Product>> productsDictionary;

    private ProductsLogic(IEnumerable<Product> products, Dictionary<int, List<Product>> productsDictionary)
    {
        this.products = products;
        this.productsDictionary = productsDictionary;
    }

    public static ProductsLogic Instance;
    static ProductsLogic()
    {
        var products = ProductDB.getProducts();
        Dictionary<int, List<Product>> productsDictionary = new Dictionary<int, List<Product>>();

        foreach (var product in products)
        {
            if (!productsDictionary.TryGetValue(product.category, out var categoryProducts))
            {
                categoryProducts = new List<Product>();
                productsDictionary[product.category] = categoryProducts;
            }
            categoryProducts.Add(product);
        }

        ProductsLogic.Instance = new ProductsLogic(products, productsDictionary);

    }

    public IEnumerable<Product> searchProducts(IEnumerable<Product> productos, string search)
    {
        var productosOrdenados = productos.OrderBy(p => p.name).ToList();

        List<Product> resultados = new List<Product>();

        Product searchProduct = new Product{
            name = search,
            id = 1,
            imgSource = "",
            price = 0,
            category = 0
            };
        int index = productosOrdenados.BinarySearch(searchProduct, new ProductoComparer());

        if (index < 0)
        {
            index = ~index;
        }

        for (int i = index; i < productosOrdenados.Count && productosOrdenados[i].name.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0; i++)
        {
            if (productosOrdenados[i].name.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                resultados.Add(productosOrdenados[i]);
            }
        }

        for (int i = index - 1; i >= 0 && productosOrdenados[i].name.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0; i--)
        {
            if (productosOrdenados[i].name.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                resultados.Add(productosOrdenados[i]);
            }
        }

        return resultados;
    }

    public IEnumerable<Product> GetProductsCategory(List<int> categoryIds)
    {
        if (categoryIds.Count <= 0) throw new ArgumentException($"The {nameof(categoryIds)} number must be greater than 0");

        if(categoryIds.Contains(0)) return this.products;

        List<Product> products = new List<Product>();
        foreach (int item in categoryIds)
        {
            this.productsDictionary.TryGetValue(item, out var productsTmp);
            if (productsTmp == null) productsTmp = new List<Product>();
            products.AddRange(productsTmp);
            
        }
        return products;
    }

}

public class ProductoComparer : IComparer<Product>
    {
        public int Compare(Product? x, Product? y)
        {
            if (x == null && y == null)
            {
                return 0;
            }
            if (x == null)
            {
                return -1;
            }
            if (y == null)
            {
                return 1;
            }
            return string.Compare(x.name, y.name, StringComparison.OrdinalIgnoreCase);
        }
    }
