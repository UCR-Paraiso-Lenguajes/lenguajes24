# Web Pages: KEStoreAPi

**Estudiante:** Kendall Sánchez Chinchilla
**Carné:** C27227
---

## Descripción del Proyecto

El propósito de este proyecto es desarrollar una tienda en línea utilizando React y Next.js. La aplicación permite a los usuarios una experiencia de compra donde pueden ver productos, añadirlos al carrito de compras, buscar por categoría, nombre y descripción, y proceder a la compra.

## Características
- Catálogo de productos
- Carrito de compras
- Proceso de pago
- Gestión de inventario
- Gestión de métodos de pago

---

## Diagramas:

### Diagrama de Actividad

### Diagrama de Componentes

**Business**

![Business](./Images/Bussiness.jpeg)

**Controllers**

![Controllers](./Images/Controllers.jpeg)

**Data**

![Data](./Images/Data.jpeg)

**Models**

![Models](./Images/Models.jpeg)

**UT**

![UT](./Images/UT.jpeg)

---

## Front-end

El front-end de la aplicación está construido con React y Next.js, y consta de varios componentes organizados en diferentes carpetas.

#### Estructura de Carpetas
- `page.tsx`: Este componente principal renderiza la página principal de la aplicación.
- `app/admin`: Este componente representa la sección de administración de la aplicación y contiene subcomponentes para diferentes tareas administrativas.
- `app/cart`: Este componente es responsable de mostrar el carrito de compras del usuario.
- `app/confirm`: Este componente es para confirmar el pedido del usuario.
- `app/payment`: Este componente es para procesar el pago del pedido del usuario e ingresar los datos de compra.
- `app/hooks`: Contiene hooks personalizados para manejar lógica específica.
- `app/ui`: Componentes de la interfaz de usuario.

#### Dependencias del Front-end
- React
- Next.js

---

## Back-end

El back-end está construido con ASP.NET Core y contiene la lógica de negocio central de la aplicación.

#### Estructura de Carpetas
- `Core`: Contiene las clases y funciones principales de la aplicación.
  - `Business`: Contiene clases y funciones relacionadas con las reglas de negocio de la aplicación.
    - `AuthLogic.cs`
    - `Categorias.cs`
    - `SaleLogic.cs`
    - `StoreLogic.cs`
  - `Data`: Contiene clases y funciones para interactuar con la base de datos.
    - `DatabaseConfiguration.cs`
    - `DatabaseSale.cs`
    - `DatabaseStore.cs`
  - `Models`: Contiene las clases que representan los modelos de datos de la aplicación.
    - `Address.cs`
    - `Cart.cs`
    - `Categoria.cs`
    - `Order.cs`
    - `PaymentMethod.cs`
    - `Product.cs`
    - `Products.cs`
    - `Sale.cs`
    - `SaleDetails.cs`
    - `SalesbyDay.cs`
    - `Store.cs`
    - `Tree.cs`
    - `UserAuth.cs`
- `KEStoreApi`: Contiene los controladores API para enviar y recibir datos desde el front-end.
  - `Controllers`
    - `AddressController.cs`
    - `AuthController.cs`
    - `CartController.cs`
    - `ProductsController.cs`
    - `SaleController.cs`
    - `StoreController.cs`
- `UT`: Contiene las pruebas unitarias para el código del back-end.
- `Program.cs`: Configuración inicial y arranque de la aplicación.

#### Dependencias del Back-end
- ASP.NET Core
- Entity Framework Core

---

## Dependencias

- El front-end depende del back-end para obtener datos y realizar acciones.
- Los componentes del front-end se comunican con el back-end a través de APIs.
- El back-end depende de la base de datos para almacenar y recuperar datos.

---

## Diagramas

### Búsqueda de Producto por Categoría y Consulta

![Búsqueda de Producto](https://www.plantuml.com/plantuml/png/hLDTwjD047_VKmp_9mLxWJn8QQjOK2ce1vZkJkl2PYVEJ0ezZWVm0hxsOdRJT3MDYA1lfVo-Tx9KGx6-JSvZPv4ph2xVzKjF0I4Gx8H3xusXOu4r6XrPBjyGph3ch7qoT0wziS0dxL4Ykmx5eeyTPeEdtUNhCMQyoiHS_ajFW0flkAKT7ccAlGgQSCB0KfmJmiT8EU10-M0iD28IoPIvGgL2sQ2n5dZvXNjKScwC1OkNrRQ1RRQXLnJmOut2xyQoxeuih0Vd1bxbKqG1vHHzj3uKvhvkzYnpknUwx_O2_6sqypReS-SmsIypAf1svECXUWoe3W0U_QuHQfkcZYurgmuluwjaqQW4MFokmDkeqxuAXvXCCCpLA2d1UuRFU0R_Ft8R5vV4-jFzhoDxRhj43zaTvU3S7zrVbtQppMuFvVvfgweo3p0vk0sTE9r88CtrA7lEDyN9uOsNsy0xrY6-KDcmu2OdvvQKG_c0Vm00)

### Habilitar y Deshabilitar Métodos de Pago

![Habilitar y Deshabilitar Métodos de Pago](https://www.plantuml.com/plantuml/png/XLCxRjmm4ErvYYdNLNgxiG4dI6CWW4_mH5AxWr0SOKXjuHkbIEKZxCKof5REPhD1L0Yay3xl3Qd7Kg5rcPBpc2jBfcV6NZ61GgGdN_d80O5cAEWh1aviX4_FiHos3RsSVbG9KXhZ6_RYN1TrzqzXuiobAe8eOT18CT5qUWzBaeQUBfYpCH_S_Oiql58o7cvZ78-qUuqfUi-IeHjaGn3ey94qN2Izetg8cNRbzFFEaJrN2Sk7vhNammh_d8yCmqbYptK9X6GaFRAnrn7TDZxJNpSeV5sG-0rj7j6ATgVqLn4Bcq03IcoV0ggKQwaibKVs852IGZvnp3NQGdBqJRXqPOzfu2wysgqo_JMAVk-BTI9RD3NhVvy21IwpP1vIBEUORBuwugr1bh90sLms0y8Ud6_RIFclV6-WohLNy3BDYbxiJwUOm-ywBgOIQPVbVDPVE3To0krj1dnwpxUlbudmsEwmpExHDktd-0K0)

### Diagrama para Flujo de Compra

![Diagrama para Flujo de Compra](https://www.plantuml.com/plantuml/png/ZLInRXix4Epv5GjNzY_eYcV1VWvI631W9Fs6N5tMu74FI_AA_8tB5Ado2VgnW7VdeoWfG2e1uipip71vzp6XfZnuOt5CB04U6Nl50S4Hx6tYYHqw654HoCEJpszIZXva61Nd7qz9Z9c5pEvBpAWi472Y7W718up7zvx3hCY1BKjdTiyIawB2gEAoJHA1GXbWKJL7yfqnV83R3xb8dgnb2RWnpWZ_tK7wJW5ku_7NdG40s5qXvx3nkmMqxvNw4oE0YoPNX4qPFVz0BVCe9dIbWWrMZ7Ynw5-JAFQqqBQ40uM8RrJIMma7rW4j7t-7pf2F1BT1LhiF4XA7N2WpDJXf6w7W-61gnuuiEohEBJdInlasVkAOyDHks-pc2Unye_zMuytBDXFy1_Lc3VwLb0ziKS6naft3blGKKgdwh9w3w93zckU4qPaPLwm-Lfdf-ExPNN6vJCPosCor4c3_ydaPsjxByMSI9n5AXTXBXLnBlA8lmSNWStkukfHBbzk4_yF4g135i-MKNQ4kBPPRZAGJEz6k9RskVoyt7a-gFmD-ogXp3vJGUoeHAtPULuHl66bk79F4XbyhQlQrjPYNYYgnNUOBAL8LNPxitxjS5lowsgMInsPTga0bDxSvPd84aVhCkdwVoXCJ8CS95UeJgK_jYKjklpPUB6-DnoIZkQVWojVp3m00)

---

## Funciones Principales del Sistema

- **StoreDB**: Representa la base de datos donde se almacena la información del sistema.
- **SaleDB**: Representa la base de datos donde se almacenan las compras.
- **SaleReport**: Esta clase es responsable de generar informes de ventas.
- **SaleReportLogic**: Esta clase proporciona la lógica para generar informes de ventas.
- **StoreLogic**: Esta clase proporciona la lógica para interactuar con la base de datos de la tienda.

### Flujo de Datos

1. El usuario realiza una solicitud a la página principal.
2. Los controladores reciben la solicitud y la procesan.
3. Los controladores interactúan con los modelos para recuperar o modificar datos en la base de datos.
4. Los controladores interactúan con las vistas para generar la respuesta al usuario.
5. Las vistas envían la respuesta al usuario.
