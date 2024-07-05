# Web Page: Book Store
**Developer:** Paula Chaves Rivera C02231

---

The project is built using ASP.NET Core and includes several components:

The project involves developing an online store using ASP.NET Core for backend operations, React and Next.js for frontend development, Node.js for server-side logic, MySQL for database management, and C# for programming tasks. It allows users the shopping experience of viewing products, adding them to a shopping cart, searching by category, name, and description, and checking out.

## Features
- Product catalog
- Shopping cart
- Checkout process
- Inventory management
- Payment method management
- Campaigns
- Security

---

## Diagrams:

### Activity Diagram

**Online Bookstore Purchase Process**

This flowchart details the steps a user takes to purchase a book from an online bookstore. The steps are as follows:

1. **Enter Homepage**: The user navigates to the main page of the online bookstore.
2. **Select Book**: The user selects a book they are interested in purchasing.
3. **Continue Shopping Decision**: The user decides whether to continue shopping or proceed to the cart.
   - If the user wants to continue shopping, they return to the book selection step.
   - If not, they proceed to the cart.
4. **View Cart**: The user reviews the items in their shopping cart.
5. **Proceed to Checkout**: The user moves forward to the checkout page.
6. **Enter Delivery Address**: The user inputs their delivery address.
7. **Select Payment Method**: The user chooses their preferred payment method.
   - If the user selects **Sinpe**, they are prompted to enter the receipt number.
   - If the user selects **Cash**, they proceed directly to the next step.
8. **Confirm Payment**: The user confirms the payment details.
   - The system displays the order number.
9. **Finalize Purchase**: The user completes the purchase process.
10. **End**: The purchase process is complete.

![Flujo de Compra](Images/DiagramaCompra.jpeg)

### Diagram Project Structure

<img src="Images/Paquetes.jpeg" alt="Paquetes" width="200" height="150">


This diagram represents the structure of the project, divided into `BackEnd` and `FrontEnd` components:

1. **BackEnd**:
   - **Core**:
     - **Business**: Contains the business logic of the application.
     - **Database**: Manages database connections and queries.
     - **Model**: Defines the data models used in the application.
   - **StoreApi**:
     - **Controller**: Handles incoming API requests and routes them to the appropriate business logic.
   - **UT (Unit Tests)**:
     - Contains unit tests to ensure the correctness of the backend components.

2. **FrontEnd**:
   - Built using `React`.
   - **HomePage**: The main page displaying products.
   - **Admin**: Contains pages and components for administrative tasks such as:
     - `init`
     - `payMethods`
     - `products`
     - `report`
   - **Cart**: Manages the shopping cart functionality.
   - **Payment**: Handles payment processing.
   - **Confirm**: Manages order confirmation.

---

# Front-end

The front-end of the application is built with React and consists of several components organized in different folders.

- `page.tsx`: This main component renders the application's main page.
- `admin`: This component represents the administration section of the application and contains subcomponents for different administrative tasks.
- `cart`: This component is responsible for displaying the user's shopping cart.
- `confirm`: This component is for confirming the user's order.
- `payment`: This component is for processing the user's order payment and entering purchase data.

---

# Back-end

The main package contains the core business logic of the application.

- `Core`: This subpackage contains the core classes and functions for the application.
- `Business`: This subpackage contains classes and functions related to the business rules of the application.
- `Database`: This subpackage contains classes and functions for interacting with the database.
- `Model`: This subpackage contains classes that represent the application's data models.
- `StoreApi`: This subpackage contains API controllers for sending and receiving data from the front-end.
- `Controller`: This subpackage contains classes that implement specific API controllers.
- `UT`: This subpackage contains unit tests for the back-end code.

---

# Dependencies

- The front-end depends on the back-end to fetch data and perform actions.
- Front-end components communicate with the back-end through APIs.
- The back-end depends on the database to store and retrieve data.

---

## Application Structure Diagram

![Fetch](Images/Fetch.jpeg){width=200px height=150px}

A[HomePage] --> B{Products}

B --> C{Database}

B --> D{Store}

C --> E{Cache}

### Explanation of the Diagrams

#### Diagram 1: Data Fetching and Caching

This diagram illustrates the process of fetching product data for the homepage of an online store:

1. **HomePage**:
   - Displays a list of products to the user.
   - Makes a `FETCH` request to retrieve product data.

2. **Fetch Request**:
   - Sent to an API endpoint (e.g., `http://localhost...` with query parameters).

3. **Cache Store**:
   - The fetched data is first checked in the cache.
   - If the data is available in the cache, it is served from there to reduce database load and improve performance.

4. **Database**:
   - If the data is not in the cache, a query is made to the database.
   - The database contains tables such as `products`, `saleLines`, and `sales`.
   - The fetched data is then stored in the cache for future requests.

---

## Class Diagram

![Clases](Images/Clases.jpeg){width=200px height=150px}

---

**Main Page**
Refers to the main page of the website.

**Controllers**
The controllers are responsible for handling user requests and processing them. They interact with models and views to complete tasks.

**StoreDB**
Represents the database where the system's information is stored.

**SaleDB**
Represents the database where purchases are stored.

**SaleReport**
This class is responsible for generating sales reports.

**SaleReportLogic**
This class provides the logic for generating sales reports.

**StoreLogic**
This class provides the logic for interacting with the store database.

**Data Flow**
1. The user makes a request to the main page.
2. The controllers receive the request and process it.
3. The controllers interact with the models to retrieve or modify data in the database.
4. The controllers interact with the views to generate the response to the user.
5. The views send the response to the user.

**Main Functions of the System**
- Store and retrieve product information.
- Generate sales reports.
- Manage user purchases.

---

# Activity Diagram

## Campaign Activity Diagram

![Campaign](Images/Campaign.png){width=200px height=150px}

### 1. Admin

#### Start
- The admin opens the campaigns module (`/campannas`).

#### Create Message
- The admin fills out the message form.
- The admin submits the form.
- The server saves the message in the database.
- The server notifies connected clients via WebSockets.

#### Delete Message
- The admin selects a message to delete.
- The server deletes the message from the database.
- The server notifies connected clients via WebSockets.

#### End

### 2. Client

#### Start
- The client connects to the store.
- The store connects to the WebSocket server.

#### Receive Message
- The server sends a notification of a new message.
- The client increments the message counter.

#### View Messages
- The client clicks on the message icon.
- The store requests the last 3 messages from the server.
- The server sends the messages.
- The store displays the messages.

#### End

## Payment Method Management and Validation System

This system manages and validates payment methods, allowing administrators to activate or deactivate payment methods and clients to select available payment methods to complete their purchases. Below are the activity diagrams and class diagrams illustrating the functionality and structure of the system.

## Activity Diagrams

### Payment Method Management and Validation

![Paymenth Methods](Images/Paymenth.png){width=200px height=150px}

This diagram shows two main activity flows:

1. **Admin:**
    - Opens the admin module.
    - Logs in.
    - Opens the payment method management page.
    - Activates or deactivates a payment method.
    - End.

2. **Client:**
    - Opens the purchase page.
    - Selects an available payment method.
    - Completes the purchase.
    - End.


**Purchase from the Carousel**

This flowchart outlines the process a user follows when making a purchase from a product carousel on the homepage of an online store. The steps are as follows:

1. **User Interaction Start**: The user begins their interaction with the carousel.
2. **Opens Product Page**: The user clicks on a product in the carousel, which opens the product's detail page.
3. **Display Products in the Carousel**: The carousel continues to display various products to the user.
4. **User Proceeds to Checkout**: After selecting a product, the user moves forward to the checkout process.
5. **User Completes the Purchase**: The user finalizes their purchase by providing necessary payment and shipping information.
6. **End**: The purchase process is complete.

![Carusel](Images/Carusel.png){width=200px height=150px}


### Class Diagram

**Payment Method**

![Paymenth Method](Images/PaymenthUML.png)

The Payment Methods Class Diagram is designed to illustrate the structure and interaction of classes responsible for managing payment methods within the system. The main purposes are:

- **PaymentMethods Class:** Represents individual payment methods available in the system. It includes attributes that define the payment method's identity and status, and a method for executing payments.
  
- **PaymentMethodDB Class:** Manages the database operations related to payment methods. It provides asynchronous methods for creating, activating, deactivating, and checking the status of payment methods in the database, ensuring efficient and reliable data management.
  
- **PaymentMethodsController Class:** Acts as an intermediary between the user interface and the database. It handles client requests related to payment methods by retrieving available methods, and managing their active/inactive status, thereby enforcing business logic and ensuring proper functionality of the payment methods within the system.

### Products and Sales Class Diagram

**Quantity**
![QuantityProducts](Images/QuantityUML.png)

The Products and Sales Class Diagram provides a comprehensive view of the classes involved in product management and sales processing within the system. The primary purposes are:

- **Product Class:** Defines the attributes and behaviors of products available for sale. It includes essential product details such as name, description, price, and category, and provides methods for creating and cloning product instances, ensuring that products are well-defined and manageable within the system.
  
- **ProductQuantity Class:** Manages the quantity of each product. It links products to their quantities, facilitating inventory management and ensuring that the system can track and update product availability accurately.
  
- **Sale Class:** Represents individual sales transactions. It includes details about the products sold, the total amount, payment method, order number, delivery address, and a unique sale identifier, thus encapsulating all necessary information for processing and tracking sales.
  
- **SaleBD Class:** Handles the database operations related to sales. It provides methods for saving sales records, retrieving daily and weekly sales reports, and inserting sale lines into the database. This ensures that sales data is accurately recorded, retrievable for reporting purposes, and integrated with the overall database structure for consistency and reliability.
---

# StoreAPI Security Implementation

This section outlines the security mechanisms implemented in the StoreAPI project. The system uses JWT (JSON Web Token) for authentication, ensuring secure access to resources based on user roles.

## Overview


## Security Implementation
Key Components:

1. **LoginModel**:

   - The `LoginModel` class represents the data model for user login.
   - It ensures that both the username and password fields are not null or empty, throwing exceptions if these conditions are violated. This helps prevent invalid data from being processed.

![Login Model](Images/LoginModel.png)



2. **UserAccount**:
   - The `UserAccount` class manages user accounts and their associated roles.
   - It ensures each user has at least one role and that the username and password fields are not null or empty, providing essential validation to maintain data integrity.
   - This class also maintains a static list of user accounts, which is useful for testing and development purposes.


3. **AuthController**:
   - The `AuthController` is responsible for handling login requests.
   - It verifies the provided user credentials against the stored user accounts.
   - If the credentials are valid, it generates a JWT. This token includes claims such as the username and roles, which are used for subsequent authorization checks.
   - The token is signed using a symmetric security key and has an expiration time to enhance security.

```csharp
        [HttpPost]
        [AllowAnonymous]
```
![AuthController](Images/AuthController.png)



4. **Program Configuration**:
   - In the main configuration file (`Program.cs`), JWT authentication and authorization are set up.
   - This configuration includes specifying the issuer, audience, and the signing key for the JWT, which are crucial for validating the token's authenticity.
   - Additionally, the configuration ensures that the token's lifetime is validated to prevent the use of expired tokens.

![ProgramSecurity1](Images/ProgramSecurity1.png)


![ProgramSecurity2](Images/ProgramSecurity2.png)


5. **Frontend (React)**:
   - The login page in the frontend sends the user's credentials to the backend.
   - Upon successful authentication, the backend returns a JWT.
   - This token is then stored in the browser's `sessionStorage`, allowing the frontend to include the token in subsequent requests to access protected resources.
   - The frontend also handles error messages and validation feedback to enhance the user experience.

Here is the way the token is stored in the browser.
![ReactSecurity1](Images/ReactSecurity1.png)

This is the way the page confirm and keeps constantly checking to see if the token is still valid
![ReactSecurity2](Images/ReactSecurity2.png)



### Security Flow

1. **User Login**:
   - The user provides their username and password through the login form.
   - The frontend sends these credentials to the backend via a POST request.

2. **Credential Verification**:
   - The backend verifies the credentials against the stored user accounts.
   - If the credentials are invalid, the backend responds with an error.

3. **Token Generation**:
   - If the credentials are valid, the backend generates a JWT.
   - The token includes claims for the username and roles, signed with a secure key, and set to expire after a certain period.

4. **Token Storage and Usage**:
   - The frontend receives the token and stores it in `sessionStorage`.
   - For any subsequent requests to protected endpoints, the frontend includes the token in the request headers.

5. **Authorization**:
   - The backend checks the token on each request to protected endpoints.
   - It validates the token's signature, issuer, audience, and expiration time.
   - Based on the token's claims, the backend determines if the user has the necessary permissions to access the requested resource.

### Benefits

- **Secure Authentication**: Using JWT ensures that user credentials are securely transmitted and stored.
- **Statelessness**: JWT allows for a stateless authentication mechanism, reducing the need for server-side session storage.
- **Scalability**: This approach scales well with the application since the token contains all necessary information about the user.
- **Flexibility**: Roles and claims in the token provide fine-grained access control, making it easy to implement different levels of access for various users.

---

### Product cache in StoreAPI

The StoreAPI project includes an efficient caching mechanism to improve performance and reduce database load by caching product data. This approach ensures that product information is readily available for the application without frequent database queries.

### Key Components

1. **Database Retrieval (StoreDB)**:
   - The `StoreDB` class is responsible for retrieving product data from the database.
   - It establishes a connection to the database using `MySqlConnection` and executes a query to fetch all products.
   - The retrieved data is then processed and stored in a list of dictionaries, where each dictionary represents a row with column names as keys and their respective values.
![RetrieveDatabaseInfo](Images/ RetrieveDatabaseInfo.png)

2. **Product Loading and Caching (Store)**:
   - The `Store` class handles loading product data into the application's cache.
   - It includes a private constructor that initializes the product list, categories, and tax percentage.
   - The `InitializeAsync` method asynchronously loads products and categories, then creates an instance of the `Store` class.
   - This instance is stored using a `Lazy<Task<Store>>` to ensure it is initialized only once and shared across the application.
   - The `LoadProductsAsync` method converts the data retrieved by `StoreDB` into a list of `Product` objects.
   - It also includes validation to ensure the data is correctly parsed and that each product is associated with a valid category.

![LoadProducts](Images/LoadProducts.png)

### Caching Flow

1. **Database Connection**:
   - The `RetrieveDatabaseInfoAsync` method in `StoreDB` establishes a connection to the MySQL database.
   - It executes a query to fetch all rows from the `products` table and processes the results into a list of dictionaries.

2. **Data Loading**:
   - The `LoadProductsAsync` method in `Store` calls `RetrieveDatabaseInfoAsync` to get the raw product data.
   - It then iterates through the data, validating and converting each row into a `Product` object.
   - Each `Product` is associated with a `Category`, which is retrieved from the `Categories` singleton.

3. **Caching Initialization**:
   - The `InitializeAsync` method loads products and categories asynchronously and initializes a `Store` instance.
   - This instance is cached using a `Lazy<Task<Store>>` to ensure it is created only once and is reused throughout the application.

4. **Accessing Cached Data**:
   - The `Store.Instance` property provides access to the cached `Store` instance.
   - This ensures that all parts of the application use the same cached data, reducing redundant database queries and improving performance.

### Benefits

- **Performance Improvement**: By caching product data, the application reduces the number of database queries, leading to faster response times.
- **Reduced Database Load**: Caching helps in minimizing the load on the database, which can be especially beneficial for applications with a high number of read operations.
- **Consistency**: The use of a single cached instance ensures that all parts of the application have a consistent view of the product data.
- **Scalability**: The caching mechanism allows the application to scale more efficiently by serving cached data to multiple users without additional database overhead.


---

### Product Search Mechanism in StoreAPI

The product search functionality in StoreAPI is designed to efficiently retrieve products based on specified criteria, including category IDs and search keywords. Here's how the search mechanism operates:

#### Search Criteria

1. **Category Filtering**:
   - Users can search for products within specific categories identified by their unique IDs (`categoryIdsList`).
   - If `categoryIdsList` contains multiple category IDs, the search retrieves products that belong to any of those categories.
   - Including a `0` in `categoryIdsList` triggers a search across all available categories, providing a comprehensive view of products.

2. **Keyword Matching**:
   - Products are searched based on user-provided keywords (`searchKeywords`), which can appear in either the product name or description.
   - The search algorithm ensures that partial matches are also considered, enhancing the likelihood of relevant product retrieval.

#### Search Process

1. **Initialization**:
   - Upon initialization, the `Products` class loads all products from the products cache.

2. **Category-based Search**:
   - Depending on the contents of `categoryIdsList`:
     - If specific category IDs are provided (`categoryIdsList` excluding `0`), the search retrieves products from those categories.
     - If `0` is included in `categoryIdsList`, the search spans all categories, ensuring a broad search scope.

3. **Keyword-based Filtering**:
   - Each product retrieved from the specified categories is evaluated against `searchKeywords`.
   - Products are filtered based on whether the `searchKeywords` match any part of their name or description.
   - The search algorithm efficiently traverses and filters products using techniques like binary search for optimal performance.

### Binary Search Algorithm Overview

Binary search is a classic algorithm for finding a sorted list of products. It works by repeatedly dividing the search interval in half. If the value of the search key is less than the item in the middle of the interval, the algorithm narrows the interval to the lower half. Otherwise, it narrows it to the upper half. This process continues until the algorithm finds the key or determines that it is not present.

### Implementation Details

1. **Input Requirements**:
   - **Sorted List**: The list of products (`products`) must be sorted beforehand, typically by product name to facilitate alphabetical search.
   - **Search Keyword**: The `searchKeywords` parameter specifies the string to search within the product names or descriptions.

2. **Method Signature**:
   The method `BinarySearchProducts` takes two parameters:
   - `products`: A sorted list of products to search within.
   - `searchKeywords`: The string of keywords to match against product names or descriptions.

3. **Algorithm Execution**:
   - **Initialization**: Begin with two pointers, `left` at the start of the list (`0`) and `right` at the end (`products.Count - 1`).
   - **Binary Search Logic**: 
     - Calculate the middle index (`mid`) of the current `left` and `right` pointers.
     - Check if the `Name` or `Description` of the product at `mid` contains the `searchKeywords` using a case-insensitive comparison (`Contains` method with `StringComparison.OrdinalIgnoreCase`).
     - If a match is found, add the product to `matchingProducts`.
     - Expand the search to the left (`temp--`) and right (`temp++`) of the `mid` index to gather all consecutive matches in the sorted list.
   - **Iteration and Convergence**: Continue adjusting the `left` and `right` pointers based on whether the `searchKeywords` precedes or follows the current middle index until the entire list is traversed.

4. **Output**:
   - **Matching Products**: Collect all products that contain the `searchKeywords` in their `Name` or `Description`.
   - **Efficiency**: Binary search operates in O(log n) time complexity, making it highly efficient for large sorted datasets compared to linear search algorithms.

![BinarySearchProduct](Images/BinarySearchProduct.png)
 
#### Benefits

- **Efficiency**: By organizing products into categorized and indexed data structures during initialization, the search operation achieves rapid response times even when querying large datasets.
  
- **Flexibility**: Users can tailor searches by combining category IDs and keywords, facilitating precise product discovery based on specific requirements.
  
- **Scalability**: As the product inventory grows, the search mechanism remains scalable due to its efficient data organization and retrieval strategies.

---


### Sale Reports

#### API Backend (C#)

#### Models

1. **DaySalesReports**:
   - Represents daily sales reports including purchase date, purchase number, total amount, products, and quantity.
   - Ensures data validity through constructor validation.

2. **WeekSalesReport**:
   - Represents weekly sales reports including the day of the week and total sales.
   - Constructor validates data input to ensure consistency.

#### SaleBD Class

- **SaveAsync Method**:
  - Saves a sale transaction to the database using MySQL.
  - Begins by opening a database connection and initiating a transaction to ensure data integrity.
  - Executes an `INSERT` query to store basic sale information (date, total, payment method, etc.).
  - Retrieves the last inserted ID to associate sale lines with the main sale entry.
  - Calls `InsertSaleLinesAsync` to store individual product details related to the sale.
  - Commits the transaction upon successful completion or rolls back if an error occurs.

- **InsertSaleLinesAsync Method**:
  - Inserts detailed sale line information (product ID, quantity, final price) associated with a specific sale ID into the database.
  - Uses a loop to iterate through each product in the sale and executes an `INSERT` query for each product.
  - Rolls back the transaction and throws an exception if an error occurs during insertion.

- **GetDailySalesAsync Method**:
  - Retrieves daily sales reports based on a specified date.
  - Uses a SQL query to join `sales`, `saleLines`, and `products` tables to fetch relevant data (purchase date, purchase number, total, quantity, products).
  - Constructs `DaySalesReports` objects from the retrieved data and returns a list of daily sales reports.

- **GetWeeklySalesAsync Method**:
  - Retrieves weekly sales reports based on a specified date.
  - Uses a SQL query to aggregate sales data by day of the week (`DAYNAME` function) and calculate total sales.
  - Constructs `WeekSalesReport` objects from the retrieved data and returns a list of weekly sales reports.

#### SaleReportLogic Class

- **GetSalesReportAsync Method**:
  - Orchestrates the retrieval of both daily and weekly sales reports.
  - Utilizes `GetDailySalesAsync` and `GetWeeklySalesAsync` methods from `SaleBD` to fetch respective reports asynchronously.
  - Constructs a `SalesReport` object containing both daily and weekly sales data and returns it.

### Frontend (React)

#### ReportPage Component

- **Initialization**:
  - Initializes state variables (`selectedDate`, `weeklySalesData`, `dailySalesData`, `isAuthenticated`) using React hooks (`useState` and `useEffect`).
  - Manages authentication using JWT tokens retrieved from session storage to ensure secure access to the sales report page.

- **Fetching Data**:
  - Implements `fetchData` function to fetch sales data from the API based on the selected date.
  - Uses `fetch` API to send a GET request to the API endpoint (`/api/Sale`) with appropriate headers including the JWT token for authorization.
  - Parses JSON response and updates `dailySalesData` and `weeklySalesData` states to populate charts.

- **User Interaction**:
  - Utilizes `DatePicker` from `react-datepicker` to allow users to select a specific date for fetching sales data.
  - Implements `handleDayChange` to update `selectedDate` state upon user interaction with the date picker.

- **Rendering**:
  - Renders two charts using `react-google-charts` (`Table` for daily sales and `PieChart` for weekly sales) to visualize sales data fetched from the API.
  - Includes error handling to redirect unauthorized users back to the login page (`/admin`).

  **Examples of the reports:**
  - Day Reports:
![DayReports](Images/DayReports.png)
 
 - Week Reports:
![WeeksReports](Images/WeeksReports.png)
 
 - Complete Reports:
![Reports](Images/Reports.png)
 

### Importance of `react-google-charts`

1. **Ease of Use and Customization**:
   - `react-google-charts` simplifies the process of integrating Google Charts into React applications. It provides components like `Table` and `PieChart` that are easy to configure and use with React's component-based architecture.

2. **Integration with React Ecosystem**:
   - As a widely used library within the React ecosystem, `react-google-charts` benefits from community support, regular updates, and compatibility with other React libraries and tools. This makes it a reliable choice for developers building data visualization features.

3. **Data Security and Privacy**:
   - When displaying sensitive data like sales figures, ensuring data security and privacy is paramount. This involves implementing secure data transmission (e.g., HTTPS), validating user permissions (as mentioned in the error handling), and adhering to data protection regulations (e.g., GDPR).
