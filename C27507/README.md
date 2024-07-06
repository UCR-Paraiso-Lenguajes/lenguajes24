<h1 style="center">Proyec: "MyStoreAPI"</h1>
<p>This file will display detailed illustrative information about the basic functionality of the project for the Commercial Applications Languages course of the Business Informatics program at UCR. This project consists of an online store capable of handling purchases of multiple products, which are processed and stored on a server.</p>
<p>The "MyStoreAPI" application is a comprehensive solution for an online store that allows users to make purchases of various products. Additionally, it features an administration panel where key elements such as payment methods, user notifications, products, and sales reports can be managed. Security is a fundamental aspect of the application, which uses JSON Web Tokens (JWT) for authentication and user account protection. The database used is MySQL, ensuring efficient and secure handling of information.</p>
<p>The technologies employed in the development of this project include:</p>

<ul>
  <li>
  <strong>Node.js:</strong> Used in the development of the server and to handle backend operations of the application. Node.js enables the creation of a robust and scalable API that communicates with the frontend and the database.
</li>
<li>
  <strong>.NET:</strong> Used to develop the administration panel and specific backend functionalities, providing efficient and secure integration with system services.
</li>
<li>
  <strong>React:</strong> Used to develop the user interface (UI) of the application, offering an interactive and dynamic user experience. React facilitates the creation of reusable components and state management.
</li>
<li>
  <strong>Bootstrap:</strong> Used for the design and styling of the application, ensuring an attractive and responsive interface. Bootstrap helps create a consistent layout adaptable to different screen sizes.
</li>
<li>
  <strong>C#:</strong> Used in the development of business logic and implementation of backend functionalities, leveraging its integration with .NET to ensure optimal performance and structured coding.
</li>
<li>
  <strong>MySQL:</strong> Used as the database management system (DBMS), storing all relevant information of the application such as user data, products, sales, and more. MySQL ensures data integrity and security.
</li>

</ul>



<h3>Project Structure</h3>

<strong>Classes</strong>
<img src="img/diagrama_clases.jpg">
<p>Diagram presenting the class structure and their relationships within the system. The main classes are detailed:</p>


<strong>Folder Group
</strong>
<img src="img/diagrama_paquetes.jpg">
<p>Diagram representing the different structures and relationships among the various project components.</p>

<strong>Basic Functionality (Product Purchase)</strong>
<img src="/img/diagrama_actividad.jpg">
<p>Flow diagram of the purchase process from start to confirmation.</p>





<h3>Functionality Diagrams</h3>
<p>The following illustrates both activity and UML diagrams that exemplify the structure of several of the many functionalities of the application.</p>



<h5>Activity Diagram - Carousel</h5>
<img src="img/diagrama_activiad_carrusel.png">
<p>An activity diagram representing the purchase process from a carousel on the main page. It is similar to the original process, the only difference being that the process goes through several new React components before reaching the API.</p>

<h5>Activity Diagram - Payment Methods</h5>
<img src="/img/diagrama_actividad_metodos_pago.png">
<p>An activity diagram representing the purchase process using a payment method by users; as well as how these payment methods are managed by the application administrators.</p>


<h5>UML Diagram - Carousel</h5>
<img src="img/uml_carrusel.png">
<p>Illustration of the different classes involved during the purchase process from the carousel.</p>


<h5>UML Diagram - Payment Methods
</h5>
<img src="img/uml_metodos_pago_ventas.png">
<p>Illustration of the different classes involved during the process of managing the store's payment methods.</p>


<p>
Below, a detailed explanation will be presented on the most outstanding and important functionalities of the application.
</p>

<h3>Security Management</h3>
<p>In the MyStoreAPI application, security has been managed using various technologies and practices to ensure secure interactions with the API and protect data. Here's a breakdown of how these security measures were implemented:</p>

<strong>1. Authentication and Authorization with JWT</strong>
<p>For authentication and authorization, JSON Web Token (JWT) is used. JWT is a standard for creating access tokens that verify the user's identity and permissions through an encrypted token, significantly enhancing security against any attackers who may have access to user resources.</p>

<p>To enable this, the `Program.cs` file should include the following configurations:</p>


```csharp
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configuración de Swagger para usar JWT
builder.Services.AddSwaggerGen(setup =>
{
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
    setup.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});

// Variables de ambiente para JWT
var jwtGlobal = Environment.GetEnvironmentVariable("JWT_GLOBAL");
if (string.IsNullOrEmpty(jwtGlobal))
{
    jwtGlobal = "https://localhost:7161";
}

// Configuración de la autenticación JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtGlobal,
            ValidAudience = jwtGlobal,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("TheSecretKeyNeedsToBePrettyLongSoWeNeedToAddSomeCharsHere"))
        };
    });

var app = builder.Build();

app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
```
<p>Explanation of the code:</p>

<li>Configuration of Swagger for JWT: Swagger is configured to use JWT on endpoints by adding a security scheme that expects a JWT token in the request header.</li>

<li>CORS Configuration: CORS (Cross-Origin Resource Sharing) policies are configured to allow requests from http://localhost:3000, enabling the frontend application to interact with the API.</li>

<li>JWT Authentication Configuration: Configuration is set up to validate JWT tokens, including validation of issuer, audience, token lifespan, and issuer's signing key.</li>

<p>

<strong>2. Controllers and Authorization</strong>
<p>The [Authorize] attribute is used in controllers to restrict access to certain endpoints based on user roles.</p>


```csharp
namespace MyStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductManagementController : ControllerBase
    {
        [HttpPost("product/insert")]
        [Authorize(Roles = "Admin, Operator")]
        public async Task<IActionResult> InsertNewProductInStoreAsync([FromBody] Product newProduct)
        {
            try
            {
                ProductManagementLogic productManagementLogic = new ProductManagementLogic();
                bool insertedProductStatus = await productManagementLogic.insertProductAsync(newProduct, Store.Instance.addNewProductInStore);
                return Ok(new { insertedProductStatus });
            }
            catch (BussinessException ex)
            {
                return StatusCode(501, "Ha ocurrido un error al agregar el nuevo producto. Por favor inténtalo más tarde.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ha ocurrido un error al agregar el nuevo producto. Por favor inténtalo más tarde.");
            }
        }

        [HttpPost("product/delete")]
        [Authorize(Roles = "Admin, Operator")]
        public async Task<IActionResult> DeleteProductInDBAsync([FromBody] Product newProduct)
        {
            try
            {
                string insertedProductStatus = "a";
                return Ok(new { insertedProductStatus });
            }
            catch (BussinessException ex)
            {
                return StatusCode(501, "Ha ocurrido un error al eliminar el producto. Por favor inténtalo más tarde.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ha ocurrido un error al eliminar el producto. Por favor inténtalo más tarde.");
            }
        }
    }
}
```

<p>[Authorize(Roles = "Admin, Operator")]: Restricts access to the InsertNewProductInStoreAsync and DeleteProductInDBAsync methods only to users with the roles Admin and Operator.</p>
<p>For store controllers or functions that do not require any authority, they are simply handled with "[AllowAnonymous]" on the associated method.</p>





<strong>Handling JWT in the Frontend</strong>
<p>In the frontend, JWT tokens are managed to authenticate requests to the API.</p>


```typescript

export async function insertNewProductInDBAsync(newProduct: ProductAPI): Promise<string | boolean | null> {
    let urlByReactEnviroment = process.env.NEXT_PUBLIC_NODE_ENV || 'https://localhost:7161';
    let directionAPI = `${urlByReactEnviroment}/api/ProductManagement/product/insert`;

    let loginToken = sessionStorage.getItem("loginToken");
    if (!loginToken) {            
        window.location.reload();
        return "Default Error";
    }
    let tokenFormat = jwtDecode(loginToken);

    let todayDate = Date.now() / 1000;
    let tokenLifeTime = tokenFormat.exp;
    if (tokenLifeTime && tokenLifeTime < todayDate) window.location.reload();

    let postConfig = {
        method: "POST",
        headers: {
            "Accept": "application/json",
            "Content-Type": "application/json",
            'Authorization': `Bearer ${loginToken}`
        },
        body: JSON.stringify(newProduct)
    }

    try {                 
        let responsePost = await fetch(directionAPI, postConfig);
        if (!responsePost.ok) {
            const errorMessage = await responsePost.text();
            return errorMessage;
        }        
        const insertedWithSuccess = await responsePost.json();
        return insertedWithSuccess;
    } catch (error) {
        throw new Error('Failed to POST data: ' + error);
    }        
}
```


<p>Code Explanation:</p>

<p>Handling of JWT Token: The token is retrieved from sessionStorage, decoded, and checked for expiration. If the token is valid, it is included in the Authorization header of the request.
With these configurations and practices, it ensures that the MyStoreAPI is protected through authentication and authorization, and that requests between the frontend and API are secure.
</p>

<h3>2. Product Cache</h3>
<p>To optimize the store and the application as a whole, a singleton development model is used to manage a single instance of the core application class, namely Store.</p>

<p>This manages several static immutable lists outside of its environment, functioning as a cache for key information in store elements, such as products, categories, payment methods, among others.</p>

```csharp
public sealed class Store {
    public IEnumerable<Product> Products { get; private set; }
    public IEnumerable<Category> AllProductCategories { get; private set; }
    public IEnumerable<PaymentMethod> AllPaymentMethods { get; private set; }
    public int TaxPercentage { get; private set; }
    public bool StoreConnectedWithDB { get; private set; }

    private Store() {
        // Inicialización de propiedades y conexión a la base de datos simulada
    }

    // Singleton Instance
    public static readonly Store Instance;

    static Store() {
        Instance = new Store();
    }

    // Métodos para actualizar el cache en memoria
    public void addNewProductInStore(Product newProduct) { ... }
    public void updatePaymentMethodInStore(int paymentMethodId, int newStatus) { ... }
}

// Controlador API para la tienda
[Route("api/[controller]")]
[ApiController]
public class StoreController : ControllerBase {
    // Lógica de la tienda
    private StoreLogic storeLogic = new StoreLogic();

    // Endpoint para obtener la tienda
    [HttpGet]
    [AllowAnonymous]
    public IActionResult getStore() {
        return Ok(Store.Instance);
    }
}
```
<strong>Functionality and Usage</strong>
<p>This code defines the Store class that manages a cache of products, categories, and payment methods to optimize the store's performance. The getStore method in the StoreController API returns the singleton instance of Store, providing access to store data from the user interface.

In the user interface, data is fetched using specific "fetch" queries. For example, this is how all products from the Store instance are retrieved to display on the main page where they can be added to the cart.</p>


```typescript
useEffect(() => {
    const loadDataProductAPI = async () => {
        try {
            let dataFromStore = await getAllProductsFromAPI();

            if (typeof dataFromStore === "object" && dataFromStore !== null) {
                setProducts(dataFromStore.productsFromStore);
                setCategoryListFromStore(dataFromStore.categoriesFromStore);
            }
        } catch (error) {
            throw new Error('Failed to fetch data: ' + error);
        }
    }
    loadDataProductAPI();
}, []);

// Función para obtener todos los productos desde la API
export async function getAllProductsFromAPI(): Promise<string | { productsFromStore: ProductAPI[], categoriesFromStore: CategoryAPI[] } | null> {
    let urlByReactEnvironment = process.env.NEXT_PUBLIC_NODE_ENV || 'https://localhost:7161';
    let directionAPI = `${urlByReactEnvironment}/api/Store`;

    try {
        const response = await fetch(directionAPI);
        if (!response.ok) {
            const errorMessage = await response.text();
            return errorMessage;
        }
        const dataStore = await response.json();
        return {
            productsFromStore: dataStore.products,
            categoriesFromStore: dataStore.allProductCategories
        };

    } catch (error) {
        throw new Error('Failed to fetch data');
    }
}
```

<p>The function "getAllProductsFromAPI" is defined as an asynchronous function (async) that returns a Promise. Its purpose is to make an HTTP GET request to the store API (Store Controller) to retrieve the list of products and categories.</p>

<h3>3. Binary Product Search</h3>

<p>Another interesting feature of this application is its ability to search products by name and categories, allowing users to apply different search filters to find what best suits their needs.</p>

<p>This functionality is implemented on the BACKEND using structs and a native C# Data Dictionary. The code is detailed and explained below:</p>

<strong>Categories Class</strong>
<p>The Categories class manages a predefined list of product categories and ensures they are ordered alphabetically. It uses a singleton pattern to provide a single instance of categories throughout the application.</p>

```csharp
namespace MyStoreAPI.Models
{
    public sealed class Categories
    {
        public IEnumerable<Category> AllProductCategories { get; private set; }

        private Categories()
        {
            List<Category> orderedCategoryList = new List<Category>()
            {
                new Category(0, "Todos los productos"),
                new Category(1, "Redes"),
                new Category(2, "Celulares"),
                new Category(3, "Videojuegos"),
                new Category(4, "Entretenimiento"),
                new Category(5, "Música"),
                new Category(6, "Computadoras"),
                new Category(7, "Juguetes")
            };

            orderedCategoryList.Sort((x, y) => string.Compare(x.name, y.name));
            AllProductCategories = orderedCategoryList;
        }

        public static readonly Categories Instance;

        static Categories()
        {
            Categories.Instance = new Categories();
        }
    }
}
```
<strong>Category Class</strong>
<p>The Category class represents a product category with an ID and a name. It ensures that both ID and name are valid upon instantiation.</p>

```csharp
namespace MyStoreAPI.Models
{
    public class Category
    {
        public int id { get; private set; }
        public string name { get; private set; }

        public Category(int id, string name)
        {
            if (id < 0)
                throw new ArgumentException($"{nameof(id)} no puede ser igual ni menor a cero");
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException($"{nameof(name)} no puede ser nulo ni vacío");

            this.id = id;
            this.name = name;
        }
    }
}
```

<strong>Product Class</strong>
<p>The Product class represents a product in the store, including its properties such as name, image URL, price, quantity, description, ID, and category. It implements the ICloneable interface for creating a deep copy of a product object.</p>

```csharp
public class Product : ICloneable
{
    public string name { get; set; }
    public string imageUrl { get; set; }
    public decimal price { get; set; }
    public decimal quantity { get; set; }
    public string description { get; set; }
    public decimal id { get; set; }
    public Category category { get; set; }

    public object Clone()
    {
        return new Product
        {
            id = this.id,
            name = this.name,
            imageUrl = this.imageUrl,
            price = this.price,
            quantity = this.quantity,
            description = this.description,
            category = this.category
        };
    }
}
```


<strong>productsController Class</strong>
<p>The productsController class is a controller responsible for handling HTTP requests related to products in the store. It includes methods to fetch products filtered by category and search text. Users are responsible for specifying the search parameters, which they send through HTTP queries as mentioned before. Depending on the type of search parameters, the controller will determine whether to apply a simple search or an advanced one (such as binary search with a data dictionary).</p>


```csharp
namespace MyStoreAPI.Controllers
{
    [Route("store/[controller]")]
    [ApiController]
    public class productsController : ControllerBase
    {
        [HttpGet("product/category")]
        public IActionResult GetProductsByCategory(int category)
        {
            try
            {
                ProductsLogic productsLogic = new ProductsLogic();
                IEnumerable<Product> filteredProducts = productsLogic.filterProductsByCategory(category);
                return Ok(filteredProducts);
            }
            catch (BussinessException ex)
            {
                return StatusCode(501, "Ha ocurrido un error al obtener los datos. Por favor inténtalo más tarde.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ha ocurrido un error al obtener los datos. Por favor inténtalo más tarde.");
            }
        }

        [HttpGet("product/search/")]
        public IActionResult GetProductsBySearchAndCategory([FromQuery]string searchText, [FromQuery]int[] categoryIds)
        {
            try
            {
                ProductsLogic productsLogic = new ProductsLogic();
                IEnumerable<Product> filteredProducts = productsLogic.filterProductsBySearchTextAndCategory(searchText, categoryIds);
                return Ok(filteredProducts);
            }
            catch (BussinessException ex)
            {
                return StatusCode(501, "Ha ocurrido un error al obtener los datos. Por favor inténtalo más tarde.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ha ocurrido un error al obtener los datos. Por favor inténtalo más tarde.");
            }
        }
    }
}
```

<strong>ProductsLogic Class</strong>

<p>The ProductsLogic class encapsulates the business logic for managing products within a store application. It utilizes efficient data structures and algorithms to handle product filtering operations. 

Here an immutable copy of all store products is obtained using the static methods of Store.</p>

```csharp
public Products dataFromStore { get; }

public ProductsLogic()
{
    this.dataFromStore = new Products();

    if (dataFromStore == null)
        throw new BussinessException($"{nameof(dataFromStore)} cannot be null");
}
```

<p>Filtering Products by Category:
The method filterProductsByCategory(int categoryId) filters products based on a specified category ID. It checks if the category ID is valid and exists within the store's categories. If categoryId is 0, indicating all products, it returns all products from the store. Otherwise, it filters products by matching categoryId with the product's category ID
</p>

```csharp
public IEnumerable<Product> filterProductsByCategory(int categoryId)
{
    if (categoryId < 0)
        throw new BussinessException($"{nameof(categoryId)} invalid category ID");

    var thisCategoryExist = dataFromStore.categoriesFromStore.Any(c => c.id == categoryId);
    if (!thisCategoryExist)
        throw new BussinessException($"{nameof(categoryId)} does not exist");

    if (categoryId == 0)
    {
        return dataFromStore.productsFromStore.ToList();
    }

    var filteredProducts = dataFromStore.productsFromStore.Where(p => p.category.id == categoryId).ToList();
    if (filteredProducts == null)
        throw new BussinessException($"{nameof(filteredProducts)} cannot be null");

    return filteredProducts;
}
```

<p>iltering Products by Search Text and Category

The method filterProductsBySearchTextAndCategory(string searchText, int[] categoryIds) filters products based on a search text and optional category IDs. It retrieves products from a dictionary structure (dictionaryOfProducts) based on matching categories. If no specific categories are provided (categoryIds.Length == 0), it retrieves all products. It then sorts the filtered products by name and applies a binary search to efficiently find products matching the search text
</p>

```csharp
public IEnumerable<Product> filterProductsBySearchTextAndCategory(string searchText, int[] categoryIds)
{
    if (searchText == null)
        throw new BussinessException($"{nameof(searchText)} cannot be null");
    if (categoryIds == null)
        throw new BussinessException($"{nameof(categoryIds)} cannot be null");

    var filteredProducts = new List<Product>();

    if (categoryIds.Length == 0)
    {
        foreach (var productList in dataFromStore.dictionaryOfProducts.Values)
        {
            filteredProducts.AddRange(productList);
        }
    }
    else
    {
        foreach (var id in categoryIds)
        {
            if (dataFromStore.dictionaryOfProducts.ContainsKey(id))
            {
                filteredProducts.AddRange(dataFromStore.dictionaryOfProducts[id]);
            }
        }
    }

    filteredProducts.Sort((x, y) => string.Compare(x.name, y.name));

    if (string.IsNullOrEmpty(searchText) || searchText == "default")
    {
        return filteredProducts;
    }

    return filterByBinarySearch(searchText, filteredProducts);
}
```

<p>Binary Search Implementation
The private method filterByBinarySearch(string searchText, List<Product> filteredProducts) performs a binary search on the sorted list of products (filteredProducts). It efficiently finds products whose names contain the searchText, ignoring case sensitivity</p>


```csharp
private IEnumerable<Product> filterByBinarySearch(string searchText, List<Product> filteredProducts)
{
    var foundProducts = new List<Product>();
    int n = filteredProducts.Count;
    int step = (int)Math.Floor(Math.Sqrt(n));
    int prev = 0;

    while (prev < n)
    {
        int currentStep = Math.Min(step, n) - 1;
        Product currentProduct = filteredProducts[currentStep];

        if (currentProduct.name.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
        {
            for (int i = currentStep; i >= prev; i--)
            {
                Product product = filteredProducts[i];
                if (product.name.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    foundProducts.Add(product);
                }
                else break;
            }
            break;
        }

        prev = step;
        step += (int)Math.Floor(Math.Sqrt(n));
    }

    return foundProducts;
}

```


<h3>4. Sales Report</h3>

<p>There is also functionality for administrators or operators of the application/platform to generate daily and weekly sales reports to monitor and keep a history of the different sales that occur.
Below is an illustration and explanation of how the sales reports of the application were generated using Google Charts:</p>


<strong>UI Admin</strong>
For administrators, this useEffect hook triggers when 'eventDate' changes.
It initiates an asynchronous fetchData function to retrieve sales reports from the API based on the selected date (eventDate). Upon clicking a button, HTTP queries are sent to the API to fetch either daily or weekly sales records, depending on the 'eventDate' provided.

The retrieved data is validated:
<li>If the response is a string, it indicates an error (e.g., HTTP error codes 504/501),
    triggering 'setValidateData(false)' to handle the error state.</li>
<li>If the response is an object, it assumes successful data retrieval and calls
    'validateRegisteredSaleReport(registeredSalesReport)' to process the data for display.</li>
<li>Any other response type sets 'validateData' to false, indicating an unexpected error.</li>

Error handling is in place to alert the user in case of data retrieval failure,
displaying an error message via 'callAlertShop'.

This useEffect hook runs whenever 'eventDate' changes, ensuring that data fetching 
and validation processes are triggered accordingly.
</p>

```typescript
    useEffect(() => {
        const fetchData = async () => {
            try {
                console.log(eventDate);
                const registeredSalesReport = await getRegisteredSalesFromAPI(eventDate);

                //Validar el tipo de informacion recibida (string = error 504/501...etc)
                if (typeof registeredSalesReport === "string") {                    
                    setValidateData(false);
                    //Si es un objeto usamos los useState para cada Chart
                }else if (typeof registeredSalesReport  === "object") {                        
                    validateRegisteredSaleReport(registeredSalesReport);                                                       
                } else{                    
                    setValidateData(false);
                }
            } catch (error) {
                callAlertShop("Error","Error al obtener datos","Al parecer los datos no pueden ser mostrados. Por favor intentalo de nuevo");
            }            
        };    
        if (eventDate) fetchData();
    }, [eventDate]);
```

<strong>class SaleController</strong>

<p>This controller provides the advantage of retrieving both types of sales reports simultaneously, saving time for administrators who need to view sales reports for a specific day or the current day up to a week ago. Additionally, it simplifies usage by requiring only a single parameter, the specific day.

The SaleController is designed to efficiently handle requests for sales reports by allowing administrators to fetch both daily and weekly sales reports in a single request. This capability is beneficial because it reduces the time administrators spend querying data, whether they need sales data for a specific day or for the past week leading up to the current day.
</p>

```csharp
public class SaleController: ControllerBase{
        [HttpPost, Authorize(Roles = "Admin")]        
        public async Task<IActionResult> GetSaleAsync([FromBody] DateTime dateFormat){
            try{                                
                SaleLogic saleLogic = new SaleLogic();
                RegisteredSaleReport specificListOfRegisteredSales = await saleLogic.getSalesByDayAndWeekAsync(dateFormat);                
                return Ok(new { specificListOfRegisteredSales });
            }
            //501 son para NotImplemented o Excepciones Propias
            catch (BussinessException ex){                                
                return StatusCode(501, "Ha ocurrido un error al obtener los datos. Por favor inténtalo más tarde. ");
            }
            catch (Exception ex){                
                return StatusCode(500, "Ha ocurrido un error al obtener los datos. Por favor inténtalo más tarde.");
            }
            
        }

    }    
```

<strong>Class SalesLogic</strong>


<strong>getSalesFromTodayAsync(DateTime dateFormat)</strong>
<p>This method retrieves all registered sales data for a specific day (dateFormat). It connects to the database (db_sale) to fetch sales records for the given date. After retrieving the data, it validates each sale to ensure data integrity, checking properties like IdSale, PurchaseNum, SubTotal, Tax, and Total. If any validation fails or no data is found, it throws a BussinessException. Finally, it returns an IEnumerable<RegisteredSale> containing validated sales records for the specified day.</p>

<strong>getSalesFromLastWeekAsync(DateTime dateFormat)</strong>
<p>This method retrieves registered sales data for the last week starting from a specified date (dateFormat). It also connects to the database (db_sale) to fetch weekly sales records. Similarly, it validates each weekly sale to ensure dayOfWeek is valid and total is not null and greater than zero. Any validation issues or absence of data results in a BussinessException being thrown. It returns an IEnumerable<RegisteredSaleWeek> containing validated weekly sales records for the specified date range.</p>


Both methods facilitate retrieving sales data from a database, ensuring that the retrieved data meets specified validation criteria before returning it.

```csharp
private async Task<IEnumerable<RegisteredSale>> getSalesFromTodayAsync(DateTime dateFormat){                                                            

            IEnumerable<RegisteredSale> allRegisteredSalesByDay = await db_sale.GetRegisteredSalesByDayAsync(dateFormat);                                                

            if (allRegisteredSalesByDay == null)throw new BussinessException($"{nameof(allRegisteredSalesByDay)} puede ser 0, pero no nula");
            foreach (var thisRegisteredSale in allRegisteredSalesByDay){

                if (thisRegisteredSale.IdSale <= 0)        
                    throw new BussinessException($"{nameof(thisRegisteredSale)} debe contener un IdSale entero positivo.");
        
                if (string.IsNullOrEmpty(thisRegisteredSale.PurchaseNum))        
                    throw new BussinessException($"{nameof(thisRegisteredSale)} no puede contener un codigo de compra nulo o vacío.");        
                
                if (thisRegisteredSale.SubTotal <= 0)
                    throw new BussinessException($"{nameof(thisRegisteredSale)} el Subtotal de la venta no puede ser menor ni igual que cero.");

                if (thisRegisteredSale.Tax < 0)
                    throw new BussinessException($"{nameof(thisRegisteredSale)} el impuesto de la venta no puede ser menor que cero.");
    

                if (thisRegisteredSale.Total <= 0)        
                    throw new BussinessException($"{nameof(thisRegisteredSale)} el total de la venta no puede ser menor ni igual que cero.");
            }
            return allRegisteredSalesByDay;
        }

        private async Task<IEnumerable<RegisteredSaleWeek>> getSalesFromLastWeekAsync(DateTime dateFormat){
            IEnumerable<RegisteredSaleWeek> allRegisteredSalesByWeek = await db_sale.GetRegisteredSalesByWeekAsync(dateFormat);                                                

            if (allRegisteredSalesByWeek == null)throw new BussinessException("La lista de ventas puede ser 0, pero no nula");

            foreach (var thisRegisteredSale in allRegisteredSalesByWeek){
                if (string.IsNullOrEmpty(thisRegisteredSale.dayOfWeek)) throw new BussinessException("Los dias de la semana no son validos");
                if (thisRegisteredSale.total == null) throw new BussinessException($"{nameof(thisRegisteredSale)} el total de ventas de un dia de la semana no puede ser nulo");
                if (thisRegisteredSale.total <= 0) throw new BussinessException($"{nameof(thisRegisteredSale)} el total de ventas de un dia de la semana no puede ser menor a cero");
            }
            return allRegisteredSalesByWeek;
        }        
    }
}
```


<h3>2.1 Diagramaas importantes de la estrcutura de la aplicaion</h3>

En esta seccion se mostraran dos diagramas UML sobre 

img #1



