namespace storeVetApi;

public sealed class Store
{
    public List<Product> Products { get; private set; }
    public int TaxPercentage { get; private set; }

    private Store(List<Product> products, int TaxPercentage)
    {
        this.Products = products;
        this.TaxPercentage = TaxPercentage;
    }

    public readonly static Store Instance;
    // Static constructor
    static Store()
    {
        var products = new List<Product>();

        products.Add(new Product
        {
            Uuid = Guid.NewGuid(),//$ nos permite la cocatenación 
            Name = $"Taza Acero Inoxidable 350 ml",
            ImageUrl = $"https://manchascr.vtexassets.com/arquivos/ids/157968-800-auto?v=638442984629570000&width=800&height=auto&aspect=true",
            Price = 5300,
            Description = $"La Taza Elevada para Comida es la elección perfecta para consentir a tu perro de raza pequeña durante sus comidas diarias. Diseñada pensando en la comodidad y el estilo, esta taza elevada no solo facilita el acceso a la comida, sino que también agrega un toque moderno a su espacio de alimentación."
        
        });
 
        products.Add(new Product
        {
            Uuid = Guid.NewGuid(),
            Name = $"Bowl de aventura",
            ImageUrl = $"https://manchascr.vtexassets.com/arquivos/ids/157787-800-auto?v=638372464039270000&width=800&height=auto&aspect=true",
            Price = 2600,
            Description = $"El Cuenco Portátil ZippyPaws Adventure es tu compañero ideal para mantener a tu perro alimentado e hidratado en cualquier aventura. Resistente al agua con un forro interior a prueba de agua y fugas, asegurando una experiencia limpia y sin desorden. Gran capacidad: contiene hasta 22 onzas de agua o 3 tazas de alimento, perfecto tanto para perros pequeños como grandes. Fácil de guardar y transportar: una vez que hayas terminado, simplemente dóblalo y guárdalo en tu bolsillo o mochila."
          
        });

        products.Add(new Product
        {
             Uuid = Guid.NewGuid(),
            Name = $"Desparasitante Heartgard ",
            ImageUrl = $"https://manchascr.vtexassets.com/arquivos/ids/156969-800-auto?v=638218745252630000&width=800&height=auto&aspect=true",
            Price = 15920,
            Description = $"Heartgard Plus es un eficaz desparasitante y larvacida que previene también la infección del gusano del corazón. Contiene 6 tabletas masticables por caja. Es el único que se puede administrar a cachorros a partir de 6 semanas de edad. Amplio margen de seguridad para todas las razas (incluyendo animales sensibles a la Ivermectina como Collies y Husky Siberianos). Perros de 12 a 22 Kg. de peso: (cada tableta contiene 136 mcg de ivermectina y 114 mg. de pirantel en forma de sal de pamoato), administrar una tableta mensual (Caja Verde)"
           
        });

        products.Add(new Product
        {
            Uuid = Guid.NewGuid(),
            Name = $"Halo Alimento Vegano Holístico",
            ImageUrl = $"https://manchascr.vtexassets.com/arquivos/ids/156815-800-auto?v=638200473971270000&width=800&height=auto&aspect=true",
            Price = 19900,
            Description = $"Elaborados con ingredientes 100% veganos, incluyen proteína vegetal de alta calidad y sin ingredientes animales, lo que significa menores emisiones de gases de efecto invernadero. También usamos solo ingredientes no modificados genéticamente, lo que respalda la biodiversidad ecológica y el bienestar. Un sistema inmunológico fuerte comienza con un intestino saludable, y Halo Holistic es el único alimento para perros que contiene prebióticos, probióticos y posbióticos para una salud digestiva completa."
            
        });
        Store.Instance = new Store(products, 13);
    }

    public Sale Purchase(Cart cart)
    {
        if (cart.ProductIds.Count == 0) throw new ArgumentException("Cart must contain at least one product.");
        if (string.IsNullOrWhiteSpace(cart.Address)) throw new ArgumentException("Address must be provided.");

        // Find matching products based on the product IDs in the cart
        IEnumerable<Product> matchingProducts = Products.Where(p => cart.ProductIds.Contains(p.Uuid.ToString())).ToList();

        // Create shadow copies of the matching products
        IEnumerable<Product> shadowCopyProducts = matchingProducts.Select(p => (Product)p.Clone()).ToList();

        // Calculate purchase amount by multiplying each product's price with the store's tax percentage
        decimal purchaseAmount = 0;
        foreach (var product in shadowCopyProducts)
        {
            product.Price *= (1 + (decimal)TaxPercentage / 100);
            purchaseAmount += product.Price;
        }

        // Create a sale object
        var sale = new Sale(shadowCopyProducts, cart.Address, purchaseAmount);

        return sale;

    }
}