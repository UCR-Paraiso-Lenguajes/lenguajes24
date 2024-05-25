using MySqlConnector;

namespace storeApi
{
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
            var products = new List<Product>
            {
                new Product
                {
                    id = 1,
                    name = "Producto 1",
                    description = "Esta computadora es muy rapida",
                    price = 20000,
                    imageURL = "https://m.media-amazon.com/images/I/71Cco7OaVxL.__AC_SX300_SY300_QL70_FMwebp_.jpg"
                },
                new Product
                {
                    id = 2,
                    name = "Producto 2",
                    description = "Esta computadora es muy rapida",
                    price = 20000,
                    imageURL = "https://images-na.ssl-images-amazon.com/images/G/01/AmazonExports/Events/2023/EBF23/Fuji_Desktop_Single_image_EBF_1x_v1._SY304_CB573698005_.jpg"
                },
                new Product
                {
                    id = 3,
                    name = "Producto 3",
                    description = "Esta computadora es muy rapida",
                    price = 20000,
                    imageURL = "https://images-na.ssl-images-amazon.com/images/G/01/AmazonExports/Events/2023/EBF23/Fuji_Desktop_Single_image_EBF_1x_v1._SY304_CB573698005_.jpg"
                },
                new Product
                {
                    id = 4,
                    name = "Producto 4",
                    description = "Esta computadora es muy rapida",
                    price = 20000,
                    imageURL = "https://images-na.ssl-images-amazon.com/images/G/01/AmazonExports/Events/2023/EBF23/Fuji_Desktop_Single_image_EBF_1x_v1._SY304_CB573698005_.jpg"
                },
                new Product
                {
                    id = 5,
                    name = "Producto 5",
                    description = "Esta computadora es muy rapida",
                    price = 20000,
                    imageURL = "https://images-na.ssl-images-amazon.com/images/G/01/AmazonExports/Events/2023/EBF23/Fuji_Desktop_Single_image_EBF_1x_v1._SY304_CB573698005_.jpg"
                },
                new Product
                {
                    id = 6,
                    name = "Producto 6",
                    description = "Esta computadora es muy rapida",
                    price = 20000,
                    imageURL = "https://images-na.ssl-images-amazon.com/images/G/01/AmazonExports/Events/2023/EBF23/Fuji_Desktop_Single_image_EBF_1x_v1._SY304_CB573698005_.jpg"
                },
                new Product
                {
                    id = 7,
                    name = "Producto 7",
                    description = "Esta computadora es muy rapida",
                    price = 20000,
                    imageURL = "https://images-na.ssl-images-amazon.com/images/G/01/AmazonExports/Events/2023/EBF23/Fuji_Desktop_Single_image_EBF_1x_v1._SY304_CB573698005_.jpg"
                },
                new Product
                {
                    id = 8,
                    name = "Producto 8",
                    description = "Esta computadora es muy rapida",
                    price = 20000,
                    imageURL = "https://images-na.ssl-images-amazon.com/images/G/01/AmazonExports/Events/2023/EBF23/Fuji_Desktop_Single_image_EBF_1x_v1._SY304_CB573698005_.jpg"
                },
                new Product
                {
                    id = 9,
                    name = "Producto 9",
                    description = "Esta computadora es muy rapida",
                    price = 20000,
                    imageURL = "https://images-na.ssl-images-amazon.com/images/G/01/AmazonExports/Events/2023/EBF23/Fuji_Desktop_Single_image_EBF_1x_v1._SY304_CB573698005_.jpg"
                },
                new Product
                {
                    id = 10,
                    name = "Producto 10",
                    description = "Esta computadora es muy rapida",
                    price = 20000,
                    imageURL = "https://images-na.ssl-images-amazon.com/images/G/01/AmazonExports/Events/2023/EBF23/Fuji_Desktop_Single_image_EBF_1x_v1._SY304_CB573698005_.jpg"
                },
                new Product
                {
                    id = 11,
                    name = "Producto 11",
                    description = "Esta computadora es muy rapida",
                    price = 20000,
                    imageURL = "https://images-na.ssl-images-amazon.com/images/G/01/AmazonExports/Events/2023/EBF23/Fuji_Desktop_Single_image_EBF_1x_v1._SY304_CB573698005_.jpg"
                },
                new Product
                {
                    id = 12,
                    name = "Producto 12",
                    description = "Esta computadora es muy rapida",
                    price = 20000,
                    imageURL = "https://images-na.ssl-images-amazon.com/images/G/01/AmazonExports/Events/2023/EBF23/Fuji_Desktop_Single_image_EBF_1x_v1._SY304_CB573698005_.jpg"
                }
            };

            Store.Instance = new Store(products, 13);

        }

    }
}