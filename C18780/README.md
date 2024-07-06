[Video](https://youtu.be/49CrUmGoCSg)
# Index 
- [1. Authentication and Authorization](#authentication-and-authorization) 
- [2. Sales Report Generation in StoreApi](#sales-report-generation-in-storeapi) 
- [3. Product Caching](#product-caching) 
- [4. Product Search](#product-search) 
- [5. ProductSearch using Inverted Tree](#productsearch-using-inverted-tree)
- [6. User Interaction in the Online Store](#user-interaction-in-the-online-store)
- [7. UML Payment Methods](#uml-payment-methods)
- [8. Payment Methods](#payment-methods)
- [9. System Structure and Data Flow](#system-structure-and-data-flow)
- [10. Checkout Flow in the Shopping Application](#checkout-flow-in-the-shopping-application)

# Authentication and Authorization

- **JWT (JSON Web Token)**: Used for authentication. The `AuthController` handles JWT token generation after validating user credentials.

## Roles and Permissions

- Authorization attributes like `[Authorize(Roles = "Admin")]` and `[Authorize(Roles = "Operator")]` are used in controllers to restrict access to specific actions based on roles.

## API Security

- APIs are configured with CORS policies (`AddCors`) to allow requests from the frontend at `http://localhost:3000`. This controls which external domains can access the APIs.

## Exception Handling

- Availability of necessary services is checked to prevent null errors (`ArgumentNullException`) and ensure system integrity.

## Protection of Sensitive Data

- `SymmetricSecurityKey` is used to configure the JWT secret key, and `SigningCredentials` to sign the token, ensuring sensitive data like passwords is not transmitted in plaintext.

## Application Layer Security

- All critical operations (creation and deletion of sensitive resources like ads, categories, products, etc.) are protected with specific roles (`Admin`, `Operator`) to limit access according to the principle of least privilege.

This approach ensures the project adheres to recommended security practices, providing robust authentication, role-based authorization, and adequate protection of sensitive data through JWT.

# Sales Report Generation in StoreApi

In the `ReportsController` of the `StoreApi` application, the functionality for generating sales reports is implemented as follows:

1. **Endpoint and Authorization**: The `GetReportsByDateAsync` method is decorated with `[HttpGet("Date"), Authorize(Roles = "Admin")]`, restricting access to the endpoint to authenticated users with the "Admin" role.

2. **Query Execution**: Inside the method, two queries are asynchronously sent using MediatR:
   ```csharp
   var dailySalesTask = mediator.Send(new GetDailySalesQuery() { DateTime = dateTime });
   var weeklySalesTask = mediator.Send(new GetWeeklySalesByDateQuery() { DateTime = dateTime });

3. **Awaiting Asynchronous Tasks**: `Task.WhenAll` is used to await both daily and weekly sales queries concurrently.
`await Task.WhenAll(dailySalesTask, weeklySalesTask);`

4. **Fetching Results**: After completing the tasks, daily and weekly sales are retrieved from the completed tasks.
`var dailySales = await dailySalesTask;
var weeklySales = await weeklySalesTask;
`

5. **Creating Reports Object**: Finally, a `Reports` object is created encapsulating both the retrieved daily and weekly sales.
`return new Reports(dailySales, weeklySales);`

This process ensures efficient and structured retrieval and presentation of daily and weekly sales data, suitable for use in report generation within the StoreApi application.


# Product Cache

## ProductsCache Class

- **Singleton Implementation**: The `ProductsCache` class implements the Singleton pattern (`GetInstance()` returns a single instance) to manage the product cache.

## Initialization

- **StoreController Constructor**: In the constructor of the `StoreController`, the `ProductsCache` instance is initialized using `ProductsCache.GetInstance()`.

## Cache Methods

- **exists()**: Checks if the product cache exists.
- **setProduct(productsList)**: Stores a list of products in the cache.
- **getProduct(categoryUuid)**: Retrieves a list of products for a specific category from the cache.
- **getAll()**: Returns all products stored in the cache.

## Usage in Controller

- **GetStoreAsync Method**: In this method, `productsCache` is used to access and filter products in memory. This avoids repeated database queries if the cache is already populated, thus improving response speed.

In summary, the product cache is implemented using a Singleton class (`ProductsCache`) that manages storage and access to products in memory once retrieved from the database.

# Product Search

## Step 1: Data Structure and Initial Loading

### Data Structure in Cache

- **ProductsCache and CategoriesCache**: Store products and categories from the database in backend memory at application startup to enhance access speed, avoiding frequent database queries.

## Step 2: Creating the Inverted Tree for Search

### InvertedTreeNode Class

- Defines an inverted tree node with a dictionary of words (`Words`) and a set of products (`Products`). Each node contains references to other nodes through words found in the products.

### ProductSearch Class

#### Constructor

- Takes a collection of products and creates a new inverted tree (`root`) where keywords extracted from product names and descriptions will be stored.
- Uses the `BuildInvertedTree` method to construct the inverted tree from the received products.

#### `BuildInvertedTree` Method

- For each product, extracts keywords from the name and description using the `GetWordsFromText` method.
- Adds each keyword to the inverted tree using the `AddWordToInvertedTree` method.

#### Helper Methods (`GetWordsFromText` and `AddWordToInvertedTree`)

- **GetWordsFromText**: Uses regular expressions to split text into words, ignoring non-alphanumeric characters and empty whitespace.
- **AddWordToInvertedTree**: Inserts a word into the inverted tree, navigating through nodes based on whether the word exists in the current node.

#### `Search` Method

- Takes a search string.
- Splits the string into keywords using `GetWordsFromText`.
- Begins searching in the inverted tree (`root`) from the initial node.
- Iterates over each keyword and navigates through the inverted tree to find products associated with each keyword.
- Returns a set of products that match all search keywords.

## Step 3: Using the Search in the Controller

### `StoreController` Controller

#### `GetStoreAsync` Method

- Receives category parameters and a search string.
- Uses `ProductsCache` to retrieve products stored in memory.
- Creates an instance of `ProductSearch` using the retrieved products.
- Filters products based on specified categories.
- Uses the searcher (`ProductSearch`) to perform a search for products matching the search string.
- Returns a `Store` object containing the found products and available payment methods.

## Summary

The product searcher is implemented using an inverted tree to index keywords extracted from product names and descriptions. This approach allows efficient and quick product searching based on user-entered keywords, optimizing performance by storing product and category data in memory using caches.

# ProductSearch using Inverted Tree

The code provides an implementation of product search using an inverted tree to index keywords in product names and descriptions.

## InvertedTreeNode Class

The `InvertedTreeNode` class represents a node in the inverted tree. It contains two private fields:
- `Words`: A dictionary mapping keywords to child nodes in the tree.
- `Products`: A set storing products associated with keywords at this node.

Public methods provided include:
- Checking if a keyword is present in the node (`ContainsWord`).
- Getting the child node corresponding to a keyword (`GetNode`).
- Adding a keyword and its child node to the dictionary (`AddWord`).
- Checking if a product is associated with this node (`ContainsProduct`).
- Adding a product to the set of products (`AddProduct`).

## ProductSearch Class

The `ProductSearch` class is used to perform product searches using the inverted tree. When constructing a `ProductSearch` object, provided products are indexed, and the inverted tree is built.

Key methods include:
- `BuildInvertedTree`: Constructs the inverted tree by indexing keywords from product names and descriptions.
- `Search`: Performs a product search given a search term. It splits the search term into keywords, searches each keyword in the inverted tree, and returns products associated with all keywords.

Regular expressions are used to split text into keywords, and transformations are applied to normalize text to lowercase during indexing and searching.

The inverted tree and product search are designed to be efficient in terms of time and memory, utilizing data structures like dictionaries and sets for fast access and avoiding duplicates.


![invertedTreeNode](https://www.planttext.com/api/plantuml/png/ZPHDJiCm48NtESMeAoAW1-W2bHyLiQ12j19hAvw2XMD7umcbjyTsapPsMzf8aPE9lpVFRwHC7GlYjjP5urTLgKOOXj62BU6ZDKnMQwGH20TPBGNUoCgsuWKAghxYzgIIRdunFqYCSac8SoinxWyFHP4rWTkxL6W21jTfwQhPlhcoIEf7s8TMQJdiFc2rjGsiB04AptRr0lnrJlKP-SEIT3EozH9_kHSTpqMDSDrhZP_GLQGQLjQKKJXjkVOQiNoqPuXd0lTIXbPBcnmYa3TzctAd4P04ZIs0BKIx85KLl0ZQ_8Dqge7gF3bmk0q6ZIp2VS7Kkb6vk8IGpW_h8DOXd4RtZPjTUYUIcoaZmOJZ1Me82CtzXzR8WoWoL-_8BPa-5WShxoxYfqUNARmYVU9UG1aVBQ5hjT_HvQeTRYqPovcj0_Ki-vr3h1lUvCBTm2uMsUF5r8F-F_47)
Plantuml code diagram [here](Doc/plantuml/InvertedTreeNode.md)

# User Interaction in the Online Store

The flowchart describes how users, both administrators and clients, interact with the online store. Users enter the site, where administrators can authenticate using a username and password. The backend uses JWT for authentication, generating a token sent to the frontend and stored in a cookie for subsequent sessions. Administrators have privileges to create or delete ads.

On the other hand, clients access the store and can view the three most recent ads. If a new ad is created while a client is browsing, they will see four ads instead of three during that session. This flow ensures a dynamic experience for clients interacting with the latest available content in the online store.


## Diagrama de actividad WebSocket
![webSocketActivityDiagram](https://i.imgur.com/gnVba0j.png)
Plantuml code diagram [here](Doc/plantuml/WebSocketActivityDiagram.md)

# UML Payment Methods

This repository contains a modularized web application using CQRS (Command Query Responsibility Segregation) to separate write and read operations. Below is the main structure of the project:

## Core

The `Core` namespace encapsulates the core domain logic. It includes the `PaymentMethods` class, which defines available payment methods with properties such as `PaymentType`, `Name`, and `IsEnabled`. It also provides functionalities like searching by payment method type.

## StoreApi.Controllers

The `StoreApi.Controllers` namespace hosts REST API controllers that manage interactions with the backend. For example, `StoreController` handles operations related to the store, such as managing payment methods and updating their enablement status.

## StoreApi.Hubs

In `StoreApi.Hubs`, you'll find `PaymentMethodsHub`, a SignalR hub that facilitates real-time communication between the backend and clients. It is used to notify changes in payment methods throughout the application.

## ReactApp

The `ReactApp` namespace represents the frontend application developed with React. It contains several modules:

### api

Provides custom functions like `useFetchInitialStore` to fetch the initial store state, including products and payment methods, and `useFetchUpdatePaymentIsEnabled` to update the enablement status of payment methods.

### Admin

Includes `PaymentMethodsPage`, an administrative page using `useFetchInitialStore` to manage payment methods and products.

### Abacaxi

Contains `Checkout`, a user interface rendering the payment page.

The modular structure and clear separation of responsibilities enable easy expansion and maintenance of the project, ensuring efficient and scalable development.

## Diagram UML Payment Methods

The diagram describes the flow of updating payment method availability in a CQRS and SignalR-based application.

1. **Administrator**: Logs in and accesses the payment methods page.
2. **Frontend**: Makes a change in the availability of a specific payment method.
3. **Backend**: Updates the availability in the database.
4. **SignalR (PaymentMethodsHub)**: Notifies the checkout frontend about the change.
5. **Checkout Frontend**: Hides the disabled payment method button and prevents the user from using unavailable payment methods.

This flow ensures a consistent and secure user experience during the payment process.

![UMLPaymentMethods](https://i.imgur.com/aHBIEV4.png)
Plantuml code diagram [here](Doc/plantuml/UMLPaymentMethods.md)

# Payment Methods

The following diagram describes the communication flow and actions among various components in an e-commerce system:

1. **Login:** The administrator authenticates in the application.
2. **Go to PaymentMethodsPage:** Accesses the payment methods page.
3. **Frontend:** From the user interface, the administrator can change the availability of a payment method.
4. **Backend:** Corresponding modification is made to the payment method availability.
5. **Notify frontend (Checkout):** Uses SignalR via PaymentMethodsHub to notify the frontend about changes during the payment process.

### Outcome
In the user interface:
- Disabled payment method buttons are hidden.
- Users are prevented from using payment methods that have been marked as unavailable.

This ensures a consistent and real-time updated shopping experience.

## Diagrama Actividad Payment Methods
![DiagramaActividadPaymentMethods](https://i.imgur.com/cNop2zM.png)
Plantuml code diagram [here](Doc/plantuml/DiagramaActividadPaymentMethods.md)

# System Structure and Data Flow

This project follows a CQRS (Command Query Responsibility Segregation) architecture, which separates read and write operations to optimize system performance and scalability. Here's how the main components interact:

- **User (Actor):** Acts on the system through the user interface. In the diagram, it shows direct interaction with the frontend to edit the quantity of a product in the cart.

- **Frontend:** Uses HTTP requests to communicate with the backend and perform operations such as adding and updating items in the shopping cart.

- **CartController:** This controller handles operations related to the shopping cart, such as adding and updating items. It uses the Mediator pattern to send commands to the system.

- **IMediator:** Interface used to send commands to the system. The diagram illustrates commands like UpdateSalesCommand and CreateSalesCommand for handling business logic associated with sales and sales lines.

- **ProductController and ProductRepository:** Manage products, providing details and supporting sales operations.

- **Sales and Commands:** Represent sales and associated commands (CreateSalesCommand and UpdateSalesCommand), which update crucial information such as date, payment method, total, and associated address for a sale.

This design not only separates read and write responsibilities, improving efficiency and scalability, but also facilitates the implementation of new features and structured, maintainable data management.

## Diagrama UML Editar Cantidad de Products
![DiagramaUMLEditarCantidaddeProducts](https://i.imgur.com/BNVtmBo.png)
Plantuml code diagram [here](Doc/plantuml/DiagramaUMLEditarCantidaddeProducts.md)

# Checkout Flow in the Shopping Application

This repository contains the implementation of the checkout flow for a shopping application, following the process diagram below:

## Flow Description

The checkout process starts from the home page of the shopping application. Here are the main actions that occur during the checkout flow:

1. **Start Checkout:**
   - The customer initiates the checkout process from the application.

2. **Display Product Details:**
   - If there are items in the cart, details of the selected products are displayed.

3. **Show Order Summary:**
   - A detailed summary of the selected products and the total order amount is presented.

4. **Select Billing Address:**
   - The customer selects or enters the billing address for the order.

5. **Display Payment Method:**
   - Once the billing address is selected, the customer is presented with the option to select the payment method.

6. **Process Payment:**
   - Depending on the selected payment method (cash or Sinpe), the corresponding payment is processed.

7. **Payment Confirmation:**
   - If cash payment is selected, confirmation from the administrator is awaited before completing the purchase.
   - If Sinpe is chosen, the confirmation number is entered and the payment is confirmed.

8. **Complete Purchase:**
   - Once payment is confirmed, the purchase process is completed and checkout is finalized.

## Components Used

### Backend:

- **Controllers:**
  - Implement the logic for each step of the checkout flow.

- **MediatR:**
  - Used for command and query management, facilitating separation of concerns and maintaining clean code.

- **Repositories:**
  - Access necessary product and sales data to complete the process.

### Frontend:

- Interacts with controllers via HTTP requests to update the cart, process payment, and fetch product details.

## Diagrama Actividad Agregar Productos Al Cart
![DiagramaActividadAgregarProductosAlCart](https://i.imgur.com/2C3quDt.png)
Plantuml code diagram [here](Doc/plantuml/DiagramaActividadAgregarProductosAlCart.md)
