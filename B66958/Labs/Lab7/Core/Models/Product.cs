namespace ApiLab7;

public class Product : ICloneable
{
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public Guid Uuid { get; set; }
    public Category Category { get; set; }

    // Implementation of the ICloneable interface
    public object Clone()
    {
        return new Product
        {
            Uuid = this.Uuid,
            Name = this.Name,
            ImageUrl = this.ImageUrl,
            Price = this.Price,
            Description = this.Description,
            Category = this.Category
        };
    }
}

public class Products
{
    private static Products instance;
    private List<Product> products;
    private Dictionary<int, List<Product>> ProductsByCategory;

    private Products()
    {
        products = new List<Product>();
        ProductsByCategory = new Dictionary<int, List<Product>>();
    }

    public static Products Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Products();
                instance.FillProducts();
            }
            return instance;
        }
    }

    private void FillProducts()
    {
        List<int> categories = Categories
            .Instance.GetCategories()
            .Select(category => category.Id)
            .ToList();

        Random rand = new Random();

        var productsData = new[]
        {
            new
            {
                description = "Gaming Mouse",
                imageUrl = "https://cdn.mos.cms.futurecdn.net/rfphfWvEc3PL2wfPJvZGiP.jpg",
                price = 75.0
            },
            new
            {
                description = "Monitor",
                imageUrl = "https://images.samsung.com/is/image/samsung/assets/nz/members/article-assets/gaming-monitors/img-kv-2.jpg?$ORIGIN_JPG$",
                price = 700.0
            },
            new
            {
                description = "Mousepad",
                imageUrl = "https://media.steelseriescdn.com/blog/posts/how-to-choose-your-mousepad/38569118cb1443abb9b88cf9b3f10da0.jpg",
                price = 30.0
            },
            new
            {
                description = "Gaming keyboard",
                imageUrl = "https://png.pngtree.com/png-vector/20220728/ourmid/pngtree-gaming-keyboard-rgb-effect-png-image_6087818.png",
                price = 30.0
            }
        };

        Product product;

        for (int i = 1; i <= 40; i++)
        {
            var productData = productsData[(i - 1) % productsData.Length];
            int randomIndex = rand.Next(1, categories.Count + 1);

            product = new Product
            {
                Name = $"Product {i}",
                ImageUrl = productData.imageUrl,
                Price = Convert.ToDecimal(productData.price),
                Description = productData.description,
                Uuid = Guid.NewGuid(),
                Category = Categories.Instance.GetCategoryById(randomIndex)
            };

            AddProduct(product);
        }
    }

    public void AddNewProduct(Product product)
    {
        if(product == null) throw new ArgumentException("The product can't be null");
        if(String.IsNullOrWhiteSpace(product.Name)) throw new ArgumentException("The product's name should be provided");
        if(String.IsNullOrWhiteSpace(product.Description)) throw new ArgumentException("The product's description should be provided");
        if(String.IsNullOrWhiteSpace(product.ImageUrl)) throw new ArgumentException("The product's description should be provided");
        if(product.Price <= 0) throw new ArgumentException("The product's price should be above 0");
        if(product.Category.Id <= 0) throw new ArgumentException("The product's category should be above 0");
        AddProduct(product);
    }

    private void AddProduct(Product product)
    {
        products.Add(product);
        if (ProductsByCategory.ContainsKey(product.Category.Id))
        {
            ProductsByCategory[product.Category.Id].Add(product);
        }
        else
        {
            List<Product> productsForCategory = new List<Product>();
            productsForCategory.Add(product);
            ProductsByCategory.Add(product.Category.Id, productsForCategory);
        }
    }

    public IEnumerable<Product> GetProductsByCategory(List<int> categories)
    {
        if (categories == null || categories.Count() == 0)
            throw new ArgumentException(
                "The categories list should not be null and must contain at least one category"
            );

        List<Product> productsFound;
        List<Product> productsMatching = new List<Product>();

        foreach (int category in categories)
        {
            if (ProductsByCategory.TryGetValue(category, out productsFound))
            {
                productsMatching.AddRange(productsFound);
            }
        }

        return productsMatching;
    }

    public IEnumerable<Product> GetProductsByQuery(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            throw new ArgumentException("The query should not be null");

        ProductSearchTree tree = new ProductSearchTree();

        foreach (var productsFound in ProductsByCategory.Values)
        {
            foreach (var product in productsFound)
            {
                tree.AddProduct(product);
            }
        }

        List<Product> matchedProducts = tree.Search(query).ToList();

        if (matchedProducts.Count > 0)
        {
            return matchedProducts;
        }
        else
        {
            throw new EmptyException("There are no products matching the queried query");
        }
    }

    public IEnumerable<Product> GetProductsByCategoryAndQuery(List<int> categories, string query)
    {
        if (categories == null || categories.Count() == 0)
            throw new ArgumentException(
                "The categories list should not be null and must contain at least one category"
            );
        if (string.IsNullOrWhiteSpace(query))
            throw new ArgumentException("The query should not be null");

        List<Product> productsFound;
        ProductSearchTree tree = new ProductSearchTree();

        foreach (int category in categories)
        {
            if (ProductsByCategory.TryGetValue(category, out productsFound))
            {
                foreach (Product product in productsFound)
                {
                    tree.AddProduct(product);
                }
            }
        }

        List<Product> matchedProducts = tree.Search(query).ToList();

        if (matchedProducts.Count > 0)
        {
            return matchedProducts;
        }
        else
        {
            throw new EmptyException(
                "There are no products matching the queried categories and query"
            );
        }
    }

    public IEnumerable<Product> GetProducts()
    {
        return products;
    }
}
