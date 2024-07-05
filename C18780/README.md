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

Este repositorio contiene una aplicación web modularizada que utiliza CQRS (Command Query Responsibility Segregation) para separar las operaciones de escritura y lectura. A continuación se detalla la estructura principal del proyecto:

## Core

El espacio de nombres `Core` encapsula la lógica central del dominio. Incluye la clase `PaymentMethods`, que define los métodos de pago disponibles con propiedades como `PaymentType`, `Name`, y `IsEnabled`. Además, ofrece funcionalidades como la búsqueda por tipo de método de pago.

## StoreApi.Controllers

El espacio de nombres `StoreApi.Controllers` alberga los controladores de la API REST que gestionan las interacciones con el backend. Por ejemplo, `StoreController` maneja las operaciones relacionadas con la tienda, como la gestión de métodos de pago y la actualización de su estado de habilitación.

## StoreApi.Hubs

En `StoreApi.Hubs` se encuentra `PaymentMethodsHub`, un hub de SignalR que facilita la comunicación en tiempo real entre el backend y los clientes. Es utilizado para notificar cambios en los métodos de pago a través de la aplicación.

## ReactApp

El espacio de nombres `ReactApp` representa la aplicación front-end desarrollada con React. Contiene varios módulos:

### api

Ofrece funciones personalizadas como `useFetchInitialStore` para obtener el estado inicial de la tienda, incluyendo productos y métodos de pago, y `useFetchUpdatePaymentIsEnabled` para actualizar el estado de habilitación de métodos de pago.

### Admin

Incluye `PaymentMethodsPage`, una página administrativa que utiliza `useFetchInitialStore` para gestionar métodos de pago y productos.

### Abacaxi

Contiene `Checkout`, una interfaz de usuario que renderiza la página de pago.

La estructura modular y la separación clara de responsabilidades permiten una fácil expansión y mantenimiento del proyecto, asegurando un desarrollo eficiente y escalable.

# UML Payment Methods

El diagrama describe el flujo de actualización de la disponibilidad de métodos de pago en una aplicación basada en CQRS y eventos con SignalR.

1. **Administrador**: Inicia sesión y accede a la página de métodos de pago.
2. **Frontend**: Realiza un cambio en la disponibilidad de un método de pago específico.
3. **Backend**: Actualiza la disponibilidad en la base de datos.
4. **SignalR (PaymentMethodsHub)**: Notifica al frontend del checkout sobre el cambio.
5. **Frontend del Checkout**: Oculta el botón del método de pago deshabilitado y evita que el usuario utilice métodos de pago no disponibles.

Este flujo asegura una experiencia coherente y segura para los usuarios durante el proceso de pago.

## UML Payment Methods
![UMLPaymentMethods](https://i.imgur.com/aHBIEV4.png)
[PlantUml](Doc/plantuml/UMLPaymentMethods.md)

## Diagrama de Actividad: Payment Methods

El siguiente diagrama describe el flujo de comunicación y acciones entre varios componentes en un sistema de comercio electrónico:

1. **Login:** El administrador se autentica en la aplicación.
2. **Go to PaymentMethodsPage:** Accede a la página de métodos de pago.
3. **Frontend:** Desde la interfaz de usuario, el administrador puede cambiar la disponibilidad de un método de pago.
4. **Backend:** Se realiza la modificación correspondiente en la disponibilidad del método de pago.
5. **Notify frontend (Checkout):** Utilización de SignalR a través de PaymentMethodsHub para notificar al frontend acerca de los cambios durante el proceso de pago.

### Resultado
En la interfaz de usuario:
- Se ocultan los botones de método de pago deshabilitados.
- Se impide al usuario utilizar métodos de pago que han sido marcados como no disponibles.

Esto asegura una experiencia de compra coherente y actualizada en tiempo real.

## Diagrama Actividad Payment Methods
![DiagramaActividadPaymentMethods](https://i.imgur.com/cNop2zM.png)
[PlantUml](Doc/plantuml/DiagramaActividadPaymentMethods.md)

## Estructura del Sistema y Flujo de Datos

Este proyecto sigue una arquitectura CQRS (Command Query Responsibility Segregation), que separa las operaciones de lectura y escritura para optimizar el rendimiento y la escalabilidad del sistema. A continuación se describe cómo interactúan los principales componentes:

- **Usuario (Actor):** Actúa sobre el sistema a través de la interfaz de usuario. En el diagrama, se muestra interactuando directamente con el frontend para editar la cantidad de un producto en el carrito.

- **Frontend:** Utiliza peticiones HTTP para comunicarse con el backend y ejecutar operaciones como agregar y actualizar elementos en el carrito de compras.

- **CartController:** Este controlador maneja las operaciones relacionadas con el carrito de compras, como agregar y actualizar artículos. Utiliza el patrón Mediator para enviar comandos al sistema.

- **IMediator:** Interfaz utilizada para enviar comandos al sistema. En el diagrama, se observa cómo se envían comandos como UpdateSalesCommand y CreateSalesCommand para manejar la lógica de negocios asociada con las ventas y líneas de ventas.

- **ProductController y ProductRepository:** Se encargan de gestionar los productos, proporcionando detalles y soporte para las operaciones de ventas.

- **Sales y Commands:** Representan las ventas y los comandos asociados (CreateSalesCommand y UpdateSalesCommand), que actualizan información crucial como la fecha, método de pago, total y dirección asociada con una venta.

Este diseño no solo separa las responsabilidades de lectura y escritura, mejorando así la eficiencia y la escalabilidad, sino que también facilita la implementación de nuevas características y la gestión de datos de manera más estructurada y mantenible.

## Diagrama UML Editar Cantidad de Products
![DiagramaUMLEditarCantidaddeProducts](https://i.imgur.com/BNVtmBo.png)
[PlantUml](Doc/plantuml/DiagramaUMLEditarCantidaddeProducts.md)

# Flujo de Checkout en la Aplicación de Compras

Este repositorio contiene la implementación del flujo de checkout para una aplicación de compras, siguiendo el siguiente diagrama de proceso:

## Descripción del Flujo

El proceso de checkout se inicia desde la página de inicio de la aplicación de compras. Aquí se detallan las acciones principales que ocurren durante el flujo de checkout:

1. **Inicio de Checkout:**
   - El cliente inicia el proceso de checkout desde la aplicación.

2. **Mostrar Detalles de Productos:**
   - Si hay productos en el carrito, se muestran los detalles de los productos seleccionados.

3. **Mostrar Resumen del Pedido:**
   - Se presenta un resumen detallado de los productos seleccionados y el total del pedido.

4. **Seleccionar Dirección de Facturación:**
   - El cliente selecciona o ingresa la dirección de facturación para el pedido.

5. **Mostrar Método de Pago:**
   - Una vez seleccionada la dirección de facturación, se muestra al cliente la opción de seleccionar el método de pago.

6. **Procesar Pago:**
   - Dependiendo del método de pago seleccionado (efectivo o Sinpe), se procesa el pago correspondiente.

7. **Confirmación del Pago:**
   - Si se selecciona el pago en efectivo, se espera la confirmación del administrador antes de finalizar la compra.
   - Si se elige Sinpe, se ingresa el número de confirmación y se procede a confirmar el pago.

8. **Finalizar Compra:**
   - Una vez confirmado el pago, se completa el proceso de compra y se finaliza el checkout.

## Componentes Utilizados

### Backend:

- **Controllers:**
  - Implementan la lógica para cada paso del flujo de checkout.

- **MediatR:**
  - Utilizado para la gestión de comandos y consultas, facilitando la separación de responsabilidades y el mantenimiento del código limpio.

- **Repositories:**
  - Acceden a los datos de productos y ventas necesarios para completar el proceso.

### Frontend:

- Interactúa con los controladores a través de peticiones HTTP para actualizar el carrito, procesar el pago y obtener detalles de productos.

## Diagrama Actividad Agregar Productos Al Cart
![DiagramaActividadAgregarProductosAlCart](https://i.imgur.com/2C3quDt.png)
[PlantUml](Doc/plantuml/DiagramaActividadAgregarProductosAlCart.md)
