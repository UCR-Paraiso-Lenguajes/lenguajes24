namespace StoreAPI.models;


public sealed class Store
{
    public List<Product> Products { get; private set; }
    public List<Product> ProductsCarrusel { get; private set; }
    public int TaxPercentage { get; private set; }

    private Store( List<Product> products, List<Product> productsCarrusel, int TaxPercentage )
    {
        Products = products;
        ProductsCarrusel = productsCarrusel;
        this.TaxPercentage = TaxPercentage;
    }

    public readonly static Store Instance;
    // Static constructor
    static Store()
    {
        var products = new List<Product>();

           products.Add(new Product {
                Name = "Cinder",
                Author = "Marissa Meyer",
                ImgUrl = "https://www.libreriainternacional.com/media/catalog/product/9/7/9781250768889_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
                Price = 9500,
                Id= 1
            });

            products.Add(new Product {
                Name = "Scarlet",
                Author = "Marissa Meyer",
                ImgUrl = "https://www.libreriainternacional.com/media/catalog/product/9/7/9781250768896_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
                Price = 9500,
                Id= 2
            });

            products.Add(new Product    {
                Name = "Cress",
                Author = "Marissa Meyer",
                ImgUrl = "https://www.libreriainternacional.com/media/catalog/product/9/7/9781250768902_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
                Price = 9500,
                Id= 3
            });

            products.Add(new Product {
                Name = "Winter",
                Author = "Marissa Meyer",
                ImgUrl = "https://www.libreriainternacional.com/media/catalog/product/9/7/9781250768926_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
                Price = 11900,
                Id= 4
            });

            products.Add(new Product {
                Name = "Fairest",
                Author = "Marissa Meyer",
                ImgUrl = "https://www.libreriainternacional.com/media/catalog/product/9/7/9781250774057_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
                Price = 8700,
                Id= 5
            });

            products.Add(new Product {
                Name = "La Sociedad de la Nieve",
                Author = "Pablo Vierci",
                ImgUrl = "https://www.libreriainternacional.com/media/catalog/product/9/7/9786070794162_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
                Price = 12800,
                Id= 6
            });

            products.Add(new Product{
                Name = "En Agosto nos vemos",
                Author = "Gabriel García Márquez",
                ImgUrl = "https://www.libreriainternacional.com/media/catalog/product/9/7/9786073911290_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
                Price = 14900,
                Id= 7
            });

            products.Add(new Product {
                Name = "El estrecho sendero entre deseos",
                Author = "Patrick Rothfuss",
                ImgUrl = "https://www.libreriainternacional.com/media/catalog/product/9/7/9789585457935_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
                Price = 12800,
                Id= 8
            });

            products.Add(new Product{
                Name = "Alas de Sangre",
                Author = "Rebecca Yarros",
                ImgUrl = "https://www.libreriainternacional.com/media/catalog/product/9/7/9788408279990_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
                Price = 19800,
                Id= 9
            });

            products.Add(new Product   {
                Name = "Corona de Medianoche",
                Author = "Sarah J. Mass",
                ImgUrl = "https://www.libreriainternacional.com/media/catalog/product/9/7/9786073143691_1_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
                Price = 15800,
                Id= 10
            });

            products.Add(new Product {
                Name = "Carta de Amor a los Muertos",
                Author = "Ava Dellaira",
                ImgUrl = "https://m.media-amazon.com/images/I/41IETN4YxGL._SY445_SX342_.jpg",
                Price = 8900,
                Id= 11
            });

            products.Add(new Product   {
                Name = "Alicia en el país de las Maravillas",
                Author = "Lewis Carrol",
                ImgUrl = "https://www.libreriainternacional.com/media/catalog/product/9/7/9788415618713_1_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000",
                Price = 7900,
                Id= 0
            });

        var productsCarrusel = new List<Product>();

           productsCarrusel.Add(new Product {
                Name = "Bookmarks",
                Author = "Perfect for not to lose where your story goes",
                ImgUrl = "1.png",
                Price = 9500,
                Id= 0
            });

            productsCarrusel.Add(new Product {
                Name = "Pins",
                Author = "Adding a touch of literary flair to any outfit or accessory",
                ImgUrl = "2.png",
                Price = 9500,
                Id= 1
            });

            productsCarrusel.Add(new Product    {
                Name = "Necklace",
                Author = "A beautifull Necklace for all day wear",
                ImgUrl = "3.png",
                Price = 9500,
                Id= 2
            });

            Store.Instance = new Store(products,productsCarrusel, 13);


    }
    

     public Sale Purchase (Cart cart)
    {
        if (cart.ProductIds.Count == 0)  throw new ArgumentException("Cart must contain at least one product.");
        if (string.IsNullOrWhiteSpace(cart.Address))throw new ArgumentException("Address must be provided.");

        var products = Store.Instance.Products;
        var taxPercentage = Store.Instance.TaxPercentage;

         // Find matching products based on the product IDs in the cart
        IEnumerable<Product> matchingProducts = Products.Where(p => cart.ProductIds.Contains(p.Id.ToString())).ToList();

        // Create shadow copies of the matching products
        IEnumerable<Product> shadowCopyProducts = matchingProducts.Select(p => (Product)p.Clone()).ToList();

        // Calculate purchase amount by multiplying each product's price with the store's tax percentage
        decimal purchaseAmount = 0;
        foreach (var product in shadowCopyProducts)
        {
            product.Price *= (1 + (decimal)taxPercentage / 100);
            purchaseAmount += product.Price;
        }

        PaymentMethods paymentMethod = PaymentMethods.Find(cart.PaymentMethods);
        PaymentMethods.Type paymentMethodType = paymentMethod.PaymentType;

        // Create a sale object
        var sale = new Sale(shadowCopyProducts, cart.Address, purchaseAmount, paymentMethodType, Sale.GenerateNextPurchaseNumber());



        return sale;

    }
}