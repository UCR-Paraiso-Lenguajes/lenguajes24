using TodoApi.Models;

namespace TodoApi.Models;
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
    
    static Store()
    {
       var products = new List<Product>
{
    new Product {
        id = 1,
        name = "Accesorios Gaming",
        price = 30M,
        image = "https://sm.mashable.com/t/mashable_in/photo/default/gaming-gadgets-copy_xeq6.1248.jpg",
        description = "¡Entra aquí si estás buscando los mejores accesorios de gaming para llevar tu experiencia al siguiente nivel!"
    },
    new Product {
        id = 2,
        name = "Oferta de calzado",
        price = 20M,
        image = "https://img.michollo.com/app/deal/355387-1586342518814.png",
        description = "Descubre las últimas tendencias en moda que te harán destacar en cualquier ocasión."
    },
    new Product {
        id = 3,
        name = "Vestido de fiesta elegante",
        price = 50M,
        image = "https://silviafernandez.com/wp-content/uploads/2023/12/Vestidos_de_fiesta_2024_Silvia_Fernandez_PROMESA_FRONTAL.jpg",
        description = "Compra tus looks para los próximos eventos por mucho menor coste, diseños que no pasan de moda."
    },
    new Product {
        id = 4,
        name = "Lámpara de luz LED",
        price = 20M,
        image = "https://www.ofertasb.com/upload/f28564.jpg",
        description = "OFERTA."
    },
    new Product {
        id = 5,
        name = "SkinCare",
        price = 30M,
        image = "https://karunaskin.com/cdn/shop/files/K-FEB-13_1024x1024.jpg?v=1697073575",
        description = "OFERTA."
    },
    new Product {
        id = 6,
        name = "Kit de Esmaltes semipermanentes",
        price = 15M,
        image = "https://www.dd2.com.ar/image/cache/ML/MLA1382471373_0_15e8dfc851b9244816120f22f362bb93-550x550.jpg",
        description = "OFERTA."
    },
    new Product {
        id = 7,
        name = "Lazos para el cabello",
        price = 10M,
        image = "https://calalunacr.com/wp-content/uploads/2024/02/7306E711-DEE7-44B2-B3CE-2873B8FE366E-600x791.jpeg",
        description = "OFERTA."
    },
    new Product {
        id = 8,
        name = "Maquillaje",
        price = 40M,
        image = "https://us.123rf.com/450wm/belchonock/belchonock1711/belchonock171101715/90496402-concepto-de-venta-de-maquillaje-y-belleza-cosm%C3%A9ticos-en-el-fondo-blanco.jpg",
        description = "OFERTA."
    }};

        Store.Instance = new Store(products, 13);
    }


}