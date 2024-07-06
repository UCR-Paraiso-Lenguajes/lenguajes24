# KEStoreApi

This project was created by Kendall Sánchez Chinchilla. The KEStoreApi provides a comprehensive API for managing an online store, including user authentication, product management, sales processing, and campaign handling.

## Business Logic Classes

### AuthLogic.cs
**Description**: Handles authentication logic, including creating and validating tokens, and verifying user credentials.

```csharp
public string CreateToken(List<Claim> claims)
{
    var token = new JwtSecurityToken(
        issuer: _config["Jwt:Issuer"],
        audience: _config["Jwt:Audience"],
        claims: claims,
        expires: DateTime.Now.AddMinutes(30),
        signingCredentials: creds);
    return new JwtSecurityTokenHandler().WriteToken(token);
}
```

### CampaignService.cs
**Description**: Manages campaign-related operations such as creating, retrieving, and deleting campaigns.

```csharp
public async Task CreateCampaignAsync(CampaignMessage campaign)
{
    using var connection = new MySqlConnection(_connectionString);
    var query = "INSERT INTO Campaigns (Title, Content) VALUES (@Title, @Content)";
    await connection.ExecuteAsync(query, campaign);
}
```

### Categorias.cs
**Description**: Manages category-related operations, including creating, retrieving, and deleting product categories.

```csharp
public IEnumerable<Categoria> GetAllCategorias()
{
    using var connection = new MySqlConnection(_connectionString);
    return connection.Query<Categoria>("SELECT * FROM Categorias");
}
```

### SaleLogic.cs
**Description**: Manages sales-related operations, such as creating sales, generating sales reports, and retrieving sales data.

```csharp
public async Task<ReportSales> GetReportSalesAsync(DateTime date)
{
    var sales = await _database.GetSalesByDateAsync(date);
    var salesByWeek = await _database.GetSalesByWeekAsync(date);
    return new ReportSales { Date = date, Sales = sales, SalesByWeek = salesByWeek };
}
```

### StoreLogic.cs
**Description**: Manages store-related operations, including retrieving product listings, handling inventory, and managing store settings.

```csharp
public IEnumerable<Product> GetProductsByCategory(int categoryId)
{
    using var connection = new MySqlConnection(_connectionString);
    return connection.Query<Product>("SELECT * FROM Products WHERE CategoryId = @CategoryId", new { CategoryId = categoryId });
}
```

## Data Access Classes

### CampaignsDatabase.cs
**Description**: Handles database operations related to campaigns.

```csharp
public async Task<IEnumerable<CampaignMessage>> GetAllCampaignsAsync()
{
    using var connection = new MySqlConnection(_connectionString);
    return await connection.QueryAsync<CampaignMessage>("SELECT * FROM Campaigns");
}
```

### DatabaseConfiguration.cs
**Description**: Manages the configuration and initialization of the database.

```csharp
public static void Init(string connectionString)
{
    _connectionString = connectionString;
}
```

### DatabaseSale.cs
**Description**: Handles database operations related to sales.

```csharp
public async Task<IEnumerable<Sale>> GetSalesByDateAsync(DateTime date)
{
    using var connection = new MySqlConnection(_connectionString);
    return await connection.QueryAsync<Sale>("SELECT * FROM Sales WHERE Date = @Date", new { Date = date });
}
```

### DatabaseStore.cs
**Description**: Handles database operations related to the store, such as product and inventory management.

```csharp
public async Task<IEnumerable<Product>> GetAllProductsAsync()
{
    using var connection = new MySqlConnection(_connectionString);
    return await connection.QueryAsync<Product>("SELECT * FROM Products");
}
```

## Models

### Address.cs
**Description**: Represents an address in the system.

```csharp
public class Address
{
    public int Id { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string PostalCode { get; set; }
    public string Country { get; set; }
}
```

### CampaignMessage.cs
**Description**: Represents a campaign message.

```csharp
public class CampaignMessage
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
}
```

### Cart.cs
**Description**: Represents a shopping cart.

```csharp
public class Cart
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public List<CartItem> Items { get; set; }
}
```

### PaymentMethod.cs
**Description**: Represents a payment method.

```csharp
public class PaymentMethod
{
    public int Id { get; set; }
    public string MethodName { get; set; }
    public string Details { get; set; }
}
```

### Product.cs
**Description**: Represents a product.

```csharp
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int CategoryId { get; set; }
}
```

### Products.cs
**Description**: Represents a collection of products.

```csharp
public class Products
{
    public List<Product> Items { get; set; }
}
```

### Sale.cs
**Description**: Represents a sale.

```csharp
public class Sale
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime Date { get; set; }
    public List<SaleItem> Items { get; set; }
}
```

### Store.cs
**Description**: Represents a store.

```csharp
public class Store
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}
```

## UI Components

### Page.tsx
**Description**: The main page component of the store.

```tsx
import React from 'react';
import Header from '../components/Header';
import ProductList from '../components/ProductList';

const Page: React.FC = () => (
  <div>
    <Header />
    <ProductList />
  </div>
);

export default Page;
```

### jwtHooks.ts
**Description**: Custom hooks for handling JWT tokens.

```ts
import { useState, useEffect } from 'react';

export const useJwt = () => {
  const [token, setToken] = useState<string | null>(null);

  useEffect(() => {
    const storedToken = localStorage.getItem('jwtToken');
    if (storedToken) {
      setToken(storedToken);
    }
  }, []);

  const saveToken = (userToken: string) => {
    localStorage.setItem('jwtToken', userToken);
    setToken(userToken);
  };

  return {
    setToken: saveToken,
    token
  };
};
```

### WebSocketContext.tsx
**Description**: Context provider for WebSocket connections.

```tsx
import React, { createContext, useContext, useEffect, useState, ReactNode } from 'react';
import * as signalR from '@microsoft/signalr';

interface CampaignMessage {
  id: number;
  title: string;
  content: string;
}

interface WebSocketContextProps {
  connection: signalR.HubConnection | null;
  messages: CampaignMessage[];
}

const WebSocketContext = createContext<WebSocketContextProps | undefined>(undefined);

export const useWebSocket = (): WebSocketContextProps => {
  const context = useContext(WebSocketContext);
  if (!context) {
    throw new Error('useWebSocket must be used within a WebSocketProvider');
  }
  return context;
};

interface WebSocketProviderProps {
  children: ReactNode;
}

export const WebSocketProvider: React.FC<WebSocketProviderProps> = ({ children }) => {
  const [connection, setConnection] = useState<signalR.HubConnection | null>(null);
  const [messages, setMessages] = useState<CampaignMessage[]>([]);

  useEffect(() => {
    const connect = new signalR.HubConnectionBuilder()
      .withUrl('/hub/campaignsHub')
      .withAutomaticReconnect()
      .build();

    connect.start()
      .then(() => {

        connect.on('ReceiveNewCampaign', (content: string, title: string, id: number) => {
          setMessages(prevMessages => [...prevMessages, { id, title, content }]);
        });

        connect.on('ReceiveDeletedCampaign', (id: number) => {
          setMessages(prevMessages => prevMessages.filter(message => message.id !== id));
        });
      })
      .catch(err => set.error('Connection to SignalR hub failed: ', err));

    setConnection(connect);

    return () => {
      connect.stop();
    };
  }, []);

  return (
    <WebSocketContext.Provider value={{ connection, messages }}>
      {children}
    </WebSocketContext.Provider>
  );
};
```

## Diagrams

The following diagrams illustrate the architecture and workflows of the KEStoreApi project. These images are stored in the `C27227/Images` directory.

### Package Diagrams

#### Core
![Core Package Diagram](../Images/DiagramaPaquetesCore.png)

#### KEStoreApi
![KEStoreApi Package Diagram](../Images/DiagramaPaquetesApi.png)

#### UT
![UT Package Diagram](../Images/DiagramaPaquetesUT.png)

### Sequence

 Diagrams

#### Add Product
![Add Product Sequence Diagram](../Images/DiagramaSecuenciaProduct.png)

#### Delete Product
![Delete Product Sequence Diagram](../Images/DiagramaSecuenciaDelete.png)

#### View Sales Report
![View Sales Report Sequence Diagram](../Images/ReportSales.png)

#### Payment Method
![Payment Method Sequence Diagram](../Images/PaymentMethod.png)

#### Delete Payment Method
![Delete Payment Method Sequence Diagram](../Images/PaymentMethodDelete.png)

### Activity Diagrams

#### Campaign Management
![Campaign Management Activity Diagram](../Images/diagramaActividadCampañas.png)

#### Purchase Process
![Purchase Process Activity Diagram](../Images/DiagramaActividadCompra.png)

## Security Management

The KEStoreApi uses JWT (JSON Web Tokens) for user authentication and authorization. When a user logs in, a JWT token is generated and sent to the client. This token is included in the headers of subsequent API requests to authenticate the user. The server validates the token using the secret key to ensure the token's integrity and authenticity.

## Product Caching

Product data is cached to improve performance and reduce database load. The caching mechanism stores product data in memory and serves requests from the cache. When product data is updated, the cache is invalidated, and fresh data is fetched from the database.

## Product Search

The product search feature allows users to search for products based on keywords. The search functionality queries the database for products matching the keywords in their name or description. The search results are then returned to the client for display.

## Sales Reports

Sales reports are generated by querying the database for sales data within a specified date range. The report includes details such as total sales, sales by product, and sales by category. The server processes the data and formats it into a report, which is then sent to the client for display.

This README provides an overview of the KEStoreApi project, including its business logic, data access classes, models, UI components, and diagrams illustrating the architecture and workflows. For detailed information on each component, please refer to the respective source files and documentation.
```

This README now includes all the requested information and properly references the image paths.
