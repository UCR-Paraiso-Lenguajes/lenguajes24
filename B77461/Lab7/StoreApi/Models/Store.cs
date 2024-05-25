using StoreApi.Models;

namespace StoreApi;

public sealed class Store
{
    public List<Product> Products { get; private set; }
    public int TaxPercentage { get; private set; }
    public List<PaymentMethods> paymentMethods{ get; private set; }

    private Store( List<Product> products, int TaxPercentage, List<PaymentMethods> paymentMethods )
    {
        this.Products = products;
        this.TaxPercentage = TaxPercentage;
        this.paymentMethods = paymentMethods;
    }

    public readonly static Store Instance;
    
    static Store()
    {
        var products = new List<Product>();

        var productsData = new[]
        {
            new Product
    {
        Name = "Audifonos",
        Description = "Audifonos RGB",
        ImageUrl = "https://tienda.starware.com.ar/wp-content/uploads/2021/05/auriculares-gamer-headset-eksa-e1000-v-surround-71-rgb-pc-ps4-verde-2331-3792.jpg",
        Price = 60.0m,
        Quantity = 5

    },
    new Product
    {
        Name = "Teclado",
        Description = "Teclado mecánico RGB",
        ImageUrl = "https://kuwait.gccgamers.com/razer-deathstalker-v2/assets/product.webp",
        Price = 75.0m,
        Quantity = 4

    },
    new Product
    {
        Name = "Mouse",
        Description = "Mouse inalámbrico",
        ImageUrl = "https://static3.tcdn.com.br/img/img_prod/374123/mouse_gamer_impact_rgb_12400_dpi_m908_redragon_29921_3_20190927170055.jpg",
        Price = 35.0m,
        Quantity = 3

    },
    new Product
    {
        Name = "Monitor",
        Description = "Monitor LCD",
        ImageUrl = "https://i5.walmartimages.ca/images/Large/956/188/6000199956188.jpg",
        Price = 200.0m,
        Quantity = 12

    },
    new Product
    {
        Name = "CASE",
        Description = "Case CPU",
        ImageUrl = "https://th.bing.com/th/id/OIP.mhKR13PBP5mQP85l2c4DWgHaHa?rs=1&pid=ImgDetMain",
        Price = 450.0m,
        Quantity = 11

    },
    new Product
    {
        Name = "MousePad",
        Description = "MousePad HYPER X",
        ImageUrl = "https://s3.amazonaws.com/static.spdigital.cl/img/products/new_web/1500590806008-36964857_0168832511.jpg",
        Price = 15.0m,
        Quantity = 10

    },
    new Product
    {
        Name = "Laptop",
        Description = "Laptop ASUS",
        ImageUrl = "https://resources.claroshop.com/medios-plazavip/s2/10252/1145258/5d13a10bac9b0-laptop-gamer-asus-rog-strix-scar-ii-i7-16gb-512gb-rtx-2070-1600x1600.jpg",
        Price = 1000.0m,
        Quantity = 9

    },
    new Product
    {
        Name = "Tarjeta de Video",
        Description = "Tarjeta Nvidia 4060",
        ImageUrl = "https://ddtech.mx/assets/uploads/861311bd60bf6ede94bfe7ab01e705a3.png",
        Price = 600.0m,
        Quantity = 8

    },
    new Product
    {
        Name = "Control",
        Description = "Control STEAM",
        ImageUrl = "https://th.bing.com/th/id/OIP.lNj-nw7kO0Q73XjkAvaQkwHaJJ?rs=1&pid=ImgDetMain",
        Price = 150.0m,
        Quantity = 7

    },
    new Product
    {
        Name = "Gafas VR",
        Description = "Gafas VR PS4",
        ImageUrl = "https://img.pccomponentes.com/articles/15/157238/a2.jpg",
        Price = 500.0m,
        Quantity = 6

    },
    new Product
    {
        Name = "Pantalla",
        Description = "Pantalla LG OLED",
        ImageUrl = "https://th.bing.com/th/id/OIP.nC89zBQSGxR8hyVnocBvlQHaGb?rs=1&pid=ImgDetMain",
        Price = 750.0m,
        Quantity = 4

    },
    new Product
    {
        Name = "Celular",
        Description = "ASUS ROG",
        ImageUrl = "https://www.latercera.com/resizer/E392-vfE0PVd1xTj8wEKR6Ud7Z0=/800x0/smart/cloudfront-us-east-1.images.arcpublishing.com/copesa/3QACWYB2FNENTINU4KTAXU2D2A.jpg",
        Price = 900.0m,
        Quantity = 13
    },
    

    };


        for (int i = 1; i <= 30; i++)
        {
            var productData = productsData[(i - 1) % productsData.Length];

            products.Add(new Product
            {
                Name = productData.Name,
                ImageUrl = productData.ImageUrl,
                Price = Convert.ToDecimal(productData.Price) * i,
                Description = $"{productData.Description} {i}",
                Uuid = Guid.NewGuid(),
                Quantity = Convert.ToInt16(productData.Quantity)
            });
        }

        var paymentMethods = new List<PaymentMethods>();
        paymentMethods.Add(new Sinpe());
        paymentMethods.Add(new Cash());

        Store.Instance = new Store(products, 13, paymentMethods);
    }
}
