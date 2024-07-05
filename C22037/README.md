# Web Page

**Developer:** Aarón Chacón Céspedes

## Tabla de Contenidos
1. [Descripción](#descripción-del-proyecto)
    - [Funcionalidades Principales](#funcionalidades-principales)
2. [Diagramas](#diagramas)
   - [Diagrama de la página principal](#diagrama-de-la-página-principal)
   - [Diagrama de la página de pago](#diagrama-de-la-página-de-pago)
   - [Diagrama de la página de reportes](#diagrama-de-la-página-de-reportes)
   - [Diagrama de Paquetes](#diagrama-de-paquetes)
   - [Diagrama de Actividad](#diagrama-de-actividad)
   - [Diagrama del WebSocket](#diagrama-del-websocket)
   - [Diagrama de clases del Proyecto](#diagrama-de-clases-del-proyecto)
   - [Diagrama de actividad del Carrito](#diagrama-de-actividad-del-carrito)
   - [Diagrama de actividad del Pago](#diagrama-de-actividad-del-pago)
3. [Seguridad](#seguridad)
   - [Autenticación y Autorización](#autenticación-y-autorización)
   - [Configuración del JWT](#configuración-del-jwt)
   - [Autorización basada en roles](#autorización-basada-en-roles)
4. [Caché](#caché)
   - [Caché de productos](#caché-de-productos)
5. [Buscador de productos](#buscador-de-productos)
   - [Lógica de búsqueda](#lógica-de-búsqueda)
6. [Reportes de venta](#reportes-de-venta)
   - [Implementación de reportes](#implementación-de-reportes)


## Descripción del Proyecto
Este proyecto es una tienda web completa donde los usuarios pueden comprar productos, seleccionar la cantidad deseada, elegir la dirección de envío y método de pago, entre otras funcionalidades. Los administradores tienen la capacidad de enviar mensajes o campañas a los clientes, añadir nuevos productos, habilitar o deshabilitar métodos de pago, y revisar reportes de ventas diarias y semanales.

## Funcionalidades Principales
### Usuarios
- **Registro e inicio de sesión:** Los clientes pueden registrarse y acceder a su cuenta.
- **Buscar productos:** Los usuarios pueden buscar y explorar productos.
- **Carrito de compras:** Añadir productos al carrito, seleccionar la cantidad deseada.
- **Realizar compras:** Completar la compra seleccionando la dirección de envío y el método de pago.

### Administradores
- **Gestión de productos:**
  - Añadir nuevos productos.
  - Actualizar información existente de productos.
  - Eliminar productos.

- **Gestión de métodos de pago:**
  - Habilitar o deshabilitar métodos de pago según sea necesario.

- **Envío de campañas:**
  - Enviar mensajes o campañas promocionales a los clientes.

- **Reportes:**
  - Revisar reportes detallados de ventas diarias.
  - Revisar reportes detallados de ventas semanales.

## Requisitos
- **Frontend:**
  - HTML, CSS, JavaScript
  - Framework recomendado: React.js

- **Backend:**
  - Lenguaje: Node.js
  - Framework recomendado: Express.js
  - Base de datos: MongoDB

## Diagramas
### Diagrama de la página principal
![Diagrama de la página principal](Images/pagprincipal.jpeg)

### Store
**Propiedades:**
- **Products**: Una colección de productos disponibles en la tienda.
- **TaxPercentage**: El porcentaje de impuesto aplicado a los productos.
- **categoryLogic**: Una instancia de `CategoryLogic` que maneja la lógica relacionada con las categorías de productos.

**Métodos:**
- **Store(IEnumerable<Product> products, int taxPercentage)**: Constructor que inicializa la tienda con una colección de productos y un porcentaje de impuesto.
- **InstanceAsync()**: Método asincrónico que devuelve una instancia de `Store`.
- **GetProductsByCategoryAsync(int id)**: Método asincrónico que devuelve una instancia de `Store` filtrada por categoría.

### CategoryLogic
**Propiedades:**
- **productsByCategoryId**: Diccionario que mapea identificadores de categoría a listas de productos correspondientes.

**Métodos:**
- **CategoryLogic(categories: CategorySt[], products: IEnumerable<Product>)**: Constructor que inicializa la lógica de categoría con las categorías y productos dados.
- **GetProductsBySearchAsync(search: String, categories: String)**: Método asincrónico que devuelve productos que coinciden con la búsqueda y las categorías proporcionadas.
- **GetCategoriesByIdAsync(categoryIds: IEnumerable<int>)**: Método asincrónico que devuelve categorías por sus identificadores.
- **SearchProducts(products: List<Product>, search: String)**: Método que busca productos en una lista según el criterio de búsqueda.

### Product
**Propiedades:**
- **Name**: Nombre del producto.
- **ImageURL**: URL de la imagen del producto.
- **Price**: Precio del producto.
- **Description**: Descripción del producto.
- **Id**: Identificador del producto.
- **Category**: Categoría del producto representada por una instancia de `CategorySt`.

**Métodos:**
- **Product(name: String, imageURL: String, price: decimal, description: String, id: int, category: CategorySt)**: Constructor que inicializa el producto con los valores proporcionados.
- **Clone()**: Método que clona el producto.

### Category
**Propiedades:**
- **Id**: Identificador de la categoría.
- **Name**: Nombre de la categoría.
- **Categories**: Subcategorías representadas por una colección de instancias de `CategorySt`.

**Métodos:**
- **Category(id: int, name: String, categories: CategorySt[])**: Constructor que inicializa la categoría con los valores proporcionados.
- **GetType(id: int)**: Método que obtiene el tipo de categoría según su identificador.
- **SortCategories()**: Método que ordena las categorías.

### StoreDB
**Métodos:**
- **CreateMysql()**: Método que crea la base de datos MySQL para la tienda.
- **GetProductsAsync()**: Método asincrónico que obtiene una colección de productos desde la base de datos.

### StoreController
**Métodos:**
- **GetStoreAsync()**: Método asincrónico que devuelve la tienda.
- **GetProductsByCategoryAsync(categories: [FromQuery] string)**: Método asincrónico que devuelve productos filtrados por categorías.
- **SearchAsync(search: [FromQuery] string, categories: [FromQuery] string)**: Método asincrónico que busca productos según los criterios de búsqueda y categorías proporcionadas.


### Diagrama de la página de pago
![Diagrama de la página de pago](Images/pagpago.jpeg)

### SaleDB
**Métodos:**
- **SaveAsync(sale: Sale)**: Método asincrónico que guarda una venta en la base de datos.
- **GetSalesReportAsync(date: DateTime)**: Método asincrónico que obtiene un reporte de ventas para una fecha específica.
- **GetWeeklySalesAsync(date: DateTime)**: Método asincrónico que obtiene las ventas semanales para una fecha específica.
- **GetDailySalesAsync(date: DateTime)**: Método asincrónico que obtiene las ventas diarias para una fecha específica.

### PaymentMethod
**Propiedades:**
- **Type**: Enumeración que representa el tipo de método de pago.

**Métodos:**
- **Find(type: Type)**: Método que encuentra un método de pago según su tipo.

### Sale
**Propiedades:**
- **Products**: Colección de productos incluidos en la venta.
- **Address**: Dirección de envío.
- **Amount**: Monto total de la venta.
- **PaymentMethod**: Tipo de método de pago utilizado.
- **PurchaseNumber**: Número de la compra.

### StoreLogic
**Propiedades:**
- **saleDB**: Instancia de `SaleDB` para interactuar con la base de datos de ventas.

**Métodos:**
- **PurchaseAsync(cart: Cart)**: Método asincrónico que procesa la compra de un carrito.
- **GenerateNextPurchaseNumber()**: Método que genera el siguiente número de compra.

### CartController
**Propiedades:**
- **storeLogic**: Instancia de `StoreLogic` para manejar la lógica de la tienda.

**Métodos:**
- **CreateCartAsync(cart: Cart)**: Método asincrónico que crea un carrito.

### Diagrama de la página de reportes
![Diagrama de la página de reportes](Images/pagreportes.jpeg)

### SaleDB
**Métodos:**
- **SaveAsync(sale: Sale)**: Método asincrónico que guarda una venta en la base de datos.
- **GetSalesReportAsync(date: DateTime)**: Método asincrónico que obtiene un reporte de ventas para una fecha específica.
- **GetWeeklySalesAsync(date: DateTime)**: Método asincrónico que obtiene las ventas semanales para una fecha específica.
- **GetDailySalesAsync(date: DateTime)**: Método asincrónico que obtiene las ventas diarias para una fecha específica.

### DailyReport
**Propiedades:**
- **PurchaseDate**: Fecha de la compra.
- **PurchaseNumber**: Número de la compra.
- **Total**: Total de la compra.

### WeeklyReport
**Propiedades:**
- **Day**: Día de la semana.
- **Total**: Total de ventas del día.

### SaleController
**Métodos:**
- **GetReportAsync(date: [FromQuery] DateTime)**: Método asincrónico que obtiene un reporte de ventas basado en la fecha proporcionada.

## Diagrama de Paquetes
![Diagrama de Paquetes](Images/diagrama_paquetes.jpeg)

## Diagrama del WebSocket
![Diagrama del WebSocket](Images/ws.jpeg)

## Diagrama de clases de Cart
![Diagrama de clases de Cart](Images/DiagramaClasesProy.png)

### ApiController
**Métodos:**
- **CreateCartAsync(Cart cart): Task<IActionResult>**: Método asincrónico que crea un carrito.
- **GetPaymentMethodsAsync(): Task<List<PaymentMethod>>**: Método asincrónico que obtiene la lista de métodos de pago disponibles.

### StoreLogic
**Métodos:**
- **PurchaseAsync(Cart cart): Task<Purchase>**: Método asincrónico que procesa la compra de un carrito y devuelve una instancia de `Purchase`.

### Purchase
**Propiedades:**
- **PurchaseNumber**: Número de la compra.
- **Cart**: Instancia de `Cart` que contiene los productos comprados.
- **PaymentMethod**: Método de pago utilizado para la compra.
- **Address**: Dirección de envío.

### Cart
**Propiedades:**
- **Items**: Diccionario que mapea identificadores de productos a instancias de `CartItem`, representando los productos en el carrito.
- **Subtotal**: Subtotal del carrito.
- **Total**: Total del carrito.

### CartItem
**Propiedades:**
- **Product**: Instancia de `Product` que representa el producto en el carrito.
- **Quantity**: Cantidad del producto en el carrito.

### Product
**Propiedades:**
- **Id**: Identificador del producto.
- **Name**: Nombre del producto.
- **Description**: Descripción del producto.
- **Price**: Precio del producto.
- **ImageURL**: URL de la imagen del producto.

### PaymentMethod
**Propiedades:**
- **Id**: Identificador del método de pago.
- **Name**: Nombre del método de pago.
- **IsEnabled**: Indica si el método de pago está habilitado.

### Flujo de Trabajo
1. **ApiController**:
   - `CreateCartAsync` crea un nuevo carrito y lo devuelve.
   - `GetPaymentMethodsAsync` obtiene los métodos de pago disponibles.
2. **StoreLogic**:
   - `PurchaseAsync` procesa la compra de un carrito, creando una instancia de `Purchase` con los detalles de la compra.
3. **Purchase**:
   - Contiene los detalles de la compra, incluyendo el número de compra, el carrito, el método de pago y la dirección de envío.
4. **Cart**:
   - Contiene los ítems del carrito (productos y sus cantidades) y los totales (subtotal y total).
5. **CartItem**:
   - Representa un producto y su cantidad en el carrito.
6. **Product**:
   - Contiene la información del producto, como su identificador, nombre, descripción, precio e imagen.
7. **PaymentMethod**:
   - Contiene la información del método de pago, como su identificador, nombre y estado de habilitación.


## Diagrama de actividad de Cart
![Diagrama de actividad de Cart](Images/DiagramaActividadCart.png)

## Diagrama de actividad del Pago
![Diagrama de actividad del Pago](Images/DiagramaActividadPayment.png)

## Seguridad

### Autenticación y Autorización
La autenticación y autorización se manejan mediante JWT (JSON Web Tokens). El endpoint de autenticación se encuentra en el controlador `AuthController`, el cual genera un token JWT al validar las credenciales del usuario. Este token incluye roles y permisos, permitiendo el control de acceso basado en roles para diferentes partes de la aplicación.

El token JWT se valida en cada solicitud a través de middleware configurado en `Program.cs`, asegurando que solo usuarios autenticados puedan acceder a ciertos endpoints. Además, se utiliza el esquema de autorización de `Bearer` con `JwtBearerDefaults.AuthenticationScheme`.

### Configuración del JWT
En el archivo `Program.cs` se configura el esquema de autenticación JWT con las siguientes opciones:
```
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "http://localhost:5087",
            ValidAudience = "http://localhost:5087",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("TheSecretKeyNeedsToBePrettyLongSoWeNeedToAddSomeCharsHere"))
        };
    });
```

### Autorización basada en roles
El rol del usuario se extrae del token JWT y se utiliza para controlar el acceso a ciertas partes de la aplicación. Por ejemplo, solo los usuarios con el rol `Admin` pueden acceder a la interfaz de administración.

## Caché

### Caché de productos
Para mejorar la eficiencia y reducir las solicitudes a la base de datos, se implementa un mecanismo de caché para los productos en la tienda. Los productos y categorías se cargan inicialmente desde la base de datos y se almacenan en listas que tienen los models de estas clases, esto para que el acceso a la base de datos no afecte al rendimiento general de la aplicación.


## Buscador de productos

El buscador de productos permite a los usuarios buscar productos por nombre, descripción y precio. Utiliza una búsqueda binaria optimizada para encontrar coincidencias en la lista de productos.

### Lógica de búsqueda
En `CategoryLogic.cs`, se implementa la lógica de búsqueda:
```
public async Task<IEnumerable<Product>> GetProductsBySearchAsync(string search, string categories)
{
    List<Product> matchingProducts = new List<Product>();
    List<int> categoryIdsList = categories?.Split(',').Select(int.Parse).ToList();
    if (categoryIdsList == null || !!categoryIdsList.Any())
    {
        foreach (var id in productsByCategoryId)
        {
            matchingProducts.AddRange(SearchProducts(id.Value, search));
        }
    }
    else
    {
        foreach (var categoryId in categoryIdsList)
        {
            if (productsByCategoryId.TryGetValue(categoryId, out List<Product> categoryProducts))
            {
                matchingProducts.AddRange(SearchProducts(categoryProducts, search));
            }
        }
    }
    return matchingProducts;
}
```

## Reportes de venta

La aplicación proporciona reportes de ventas diarias y semanales. Los reportes se obtienen de la base de datos y se presentan en la interfaz de administración.

### Implementación de reportes
En el controlador `SaleController`, se implementa un endpoint para obtener los reportes de ventas:
```
[HttpGet]
public async Task<IActionResult> GetReportAsync([FromQuery] DateTime date)
{
    if (date == DateTime.MinValue)
    {
        return BadRequest("Date parameter is required.");
    }
    if (date > DateTime.Now)
    {
        return BadRequest("Date cannot be in the future.");
    }
    SaleDB saleDB = new SaleDB();
    SalesReport salesReport = saleDB.GetSalesReport(date);
    return Ok(salesReport);
}
```

La manera en que se generan los reportes en `SaleDB` es la siguiente:
```
public SalesReport GetSalesReport(DateTime date)
{
    if (date == DateTime.MinValue)
        throw new ArgumentException("Date cannot be:", nameof(date));

    Task<List<WeeklyReport>> weeklySalesTask = GetWeeklySalesAsync(date);
    Task<List<DailyReport>> dailySalesTask = GetDailySalesAsync(date);
    Task.WaitAll(weeklySalesTask, dailySalesTask);
    List<WeeklyReport> weeklySales = weeklySalesTask.Result;
    List<DailyReport> dailySales = dailySalesTask.Result;

    SalesReport salesReport = new SalesReport(dailySales, weeklySales);
    return salesReport;
}
```

En la función `GetWeeklySalesAsync` se obtienen los reportes semanales de ventas:
```
public async Task<List<WeeklyReport>> GetWeeklySalesAsync(DateTime date)
{
    List<WeeklyReport> weeklySales = new List<WeeklyReport>();

    using (MySqlConnection connection = new MySqlConnection(Storage.Instance.ConnectionString))
    {
        await connection.OpenAsync();

        string selectQuery = @"
            use store;
            SELECT DAYNAME(sale.purchase_date) AS day, SUM(sale.total) AS total
            FROM sales sale 
            WHERE YEARWEEK(sale.purchase_date) = YEARWEEK(@date)
            GROUP BY DAYNAME(sale.purchase_date);";

        using (var command = new MySqlCommand(selectQuery, connection))
        {
            command.Parameters.AddWithValue("@date", date);

            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    string day = reader.GetString("day");
                    decimal total = reader.GetDecimal("total");
                    WeeklyReport weeklyReport = new WeeklyReport(day, total);
                    weeklySales.Add(weeklyReport);
                }
            }
        }
    }
    return weeklySales;
}
```

En la función `GetDailySalesAsync` se obtienen los reportes diarios de ventas:
```
public async Task<List<DailyReport>> GetDailySalesAsync(DateTime date)
{
    List<DailyReport> dailySales = new List<DailyReport>();

    using (MySqlConnection connection = new MySqlConnection(Storage.Instance.ConnectionString))
    {
        await connection.OpenAsync();

        string selectQuery = @"
            use store;
            SELECT purchase_date, purchase_number, total
            FROM sales
            WHERE DATE(purchase_date) = DATE(@date);";

        using (var command = new MySqlCommand(selectQuery, connection))
        {
            command.Parameters.AddWithValue("@date", date);

            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    DateTime purchaseDate = reader.GetDateTime("purchase_date");
                    string purchaseNumber = reader.GetString("purchase_number");
                    decimal total = reader.GetDecimal("total");
                    DailyReport dailyReport = new DailyReport(purchaseDate, purchaseNumber, total);
                    dailySales.Add(dailyReport);
                }
            }
        }
    }
    return dailySales;
}
```