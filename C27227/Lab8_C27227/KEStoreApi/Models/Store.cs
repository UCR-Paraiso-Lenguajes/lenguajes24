namespace KEStoreApi;
public sealed class Store
{
    public List<Product> Products { get; private set; }
    public int TaxPercentage { get; private set; }

    private Store( List<Product> products, int TaxPercentage )
    {
        this.Products = products;
        this.TaxPercentage = TaxPercentage;
    }

    public readonly static Store Instance;
    // Static constructor
    static Store()
    {
        var products = new List<Product>
{
    new Product
    {
        Id = 1,
        Name = "Xbox",
        Price = 349,
        ImageUrl = "https://xxboxnews.blob.core.windows.net/prod/sites/2/05-23.jpg"
    },
    new Product
    {
        Id = 2,
        Name = "PlayStation 4",
        Price = 212,
        ImageUrl = "https://media.wired.com/photos/62eabb0e58719fe5c578ec7c/master/pass/What-To-Do-With-Old-PS4-Gear.jpg"
    },
    new Product
    {
        Id = 3,
        Name = "Hogwarts Legacy",
        Price = 47,
        ImageUrl = "https://assets.nintendo.com/image/upload/c_fill,w_1200/q_auto:best/f_auto/dpr_2.0/ncom/software/switch/70070000019147/8d6950111fb9a0ece31708dcd6ac893f93c012bc585a6a09dfd986d56ab483d1"
    },
    new Product
    {
        Id = 4,
        Name = "Keyboard Corsair",
        Price = 79,
        ImageUrl = "https://i.ytimg.com/vi/KhVg_7WqCaA/maxresdefault.jpg"
    },
    new Product
    {
        Id = 5,
        Name = "Xbox",
        Price = 349,
        ImageUrl = "https://xxboxnews.blob.core.windows.net/prod/sites/2/05-23.jpg"
    },
    new Product
    {
        Id = 7,
        Name = "Xbox",
        Price = 349,
        ImageUrl = "https://xxboxnews.blob.core.windows.net/prod/sites/2/05-23.jpg"
    },
    new Product
    {
        Id = 8,
        Name = "Xbox",
        Price = 349,
        ImageUrl = "https://xxboxnews.blob.core.windows.net/prod/sites/2/05-23.jpg"
    },
    new Product
    {
        Id = 9,
        Name = "Celular",
        Price = 100,
        ImageUrl = "https://d.newsweek.com/en/full/1995235/galaxy-s22-series-phones.jpg"
    },
    new Product
    {
        Id = 10,
        Name = "Celular",
        Price = 100,
        ImageUrl = "https://d.newsweek.com/en/full/1995235/galaxy-s22-series-phones.jpg"
    },
    new Product
    {
        Id = 11,
        Name = "Celular",
        Price = 100,
        ImageUrl = "https://d.newsweek.com/en/full/1995235/galaxy-s22-series-phones.jpg"
    },
    new Product
    {
        Id = 12,
        Name = "Celular",
        Price = 100,
        ImageUrl = "https://d.newsweek.com/en/full/1995235/galaxy-s22-series-phones.jpg"
    },
    new Product
    {
        Id = 13,
        Name = "Logitech Superlight",
        Price = 126,
        ImageUrl = "https://cdn.mos.cms.futurecdn.net/uFsmnUYaPrVA48v9ubwhCV.jpg"
    },
    new Product
    {
        Id = 14,
        Name = "AMD RYZEN 9 7950X3D",
        Price = 626,
        ImageUrl = "https://www.techspot.com/articles-info/2636/images/2023-02-27-image-14.jpg"
    },
     new Product
    {
        Id = 15,
        Name = "GIGABYTE GeForce RTX 4060 AERO OC 8G Graphics Card",
        Price = 319,
        ImageUrl = "https://static.tweaktown.com/news/9/0/90778_02_gigabyte-accidentally-confirms-geforce-rtx-4070-12gb-and-4060-8gb_full.jpg"
    }
};
        Store.Instance = new Store(products, 13);
    }
}