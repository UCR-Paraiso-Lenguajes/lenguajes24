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
[PlantUml](Doc/plantuml/InvertedTreeNode.md)

## User Interaction in the Online Store

The flowchart describes how users, both administrators and clients, interact with the online store. Users enter the site, where administrators can authenticate using a username and password. The backend uses JWT for authentication, generating a token sent to the frontend and stored in a cookie for subsequent sessions. Administrators have privileges to create or delete ads.

On the other hand, clients access the store and can view the three most recent ads. If a new ad is created while a client is browsing, they will see four ads instead of three during that session. This flow ensures a dynamic experience for clients interacting with the latest available content in the online store.


## Diagrama de actividad WebSocket
![webSocketActivityDiagram](https://i.imgur.com/gnVba0j.png)
[PlantUml](Doc/plantuml/WebSocketActivityDiagram.md)

# UMLPaymentMethods

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

# UML Payment Methods

The diagram describes the flow of updating payment method availability in a CQRS and SignalR-based application.

1. **Administrator**: Logs in and accesses the payment methods page.
2. **Frontend**: Makes a change in the availability of a specific payment method.
3. **Backend**: Updates the availability in the database.
4. **SignalR (PaymentMethodsHub)**: Notifies the checkout frontend about the change.
5. **Checkout Frontend**: Hides the disabled payment method button and prevents the user from using unavailable payment methods.

This flow ensures a consistent and secure user experience during the payment process.

## UML Payment Methods
![UMLPaymentMethods](https://i.imgur.com/aHBIEV4.png)
[PlantUml](Doc/plantuml/UMLPaymentMethods.md)

## Activity Diagram: Payment Methods

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
[PlantUml](Doc/plantuml/DiagramaActividadPaymentMethods.md)

## System Structure and Data Flow

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
[PlantUml](Doc/plantuml/DiagramaUMLEditarCantidaddeProducts.md)

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
[PlantUml](Doc/plantuml/DiagramaActividadAgregarProductosAlCart.md)
