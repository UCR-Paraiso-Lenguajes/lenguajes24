# Página Web: Tienda de Libros
Paula Chaves Rivera C02231

## Diagramas:
![Metoo](https://opi.ucr.ac.cr/sites/default/files/styles/large/public/descarga_9.jpg?itok=9l7H5Q4l)

## Diagrama de Actividad
**__Flujo de Compras__**
Flowchart TD
    A[Inicio] --> B[Usuario entra a la página]
    B --> C[Selecciona los libros]
    C --> D[Libros se ingresan al carrito]
    D --> E[Usuario hace clic en el botón del carrito]
    E --> F[Pestaña del carrito]
    F --> G[Muestra el precio total y botón "Continuar"]
    G --> H[Usuario hace clic en "Continuar"]
    H --> I[Ingresar dirección de entrega]
    I --> J[Seleccionar método de pago]
    J --> K{Método de pago}

    K --> |SINPE| L[Ingresar número de recibo SINPE]
    L --> M[Botón "Confirmar pago"]

    K --> |Cash| N[Botón "Confirmar pago"]

    M --> O[Página de confirmación]
    N --> O[Página de confirmación]
    O --> P[Muestra el número de compra]
    P --> Q[Botón "Finalizar compra"]
    Q --> R[Fin]

![Flujo de Compra](DiagramaCompra.jpeg)

## Diagrama Paquetes

![Paquetes](Paquetes.jpeg)

**Front-end:**

El front-end de la aplicación está construido con React y se compone de varios componentes organizados en diferentes carpetas.
<ul>
<li>page.tsx: Este componente principal renderiza la página principal de la aplicación.
<li>admin: Este componente representa la sección de administración de la aplicación y contiene subcomponentes para diferentes tareas administrativas:
<li>cart: Este componente se encarga de mostrar el carrito de compras del usuario.
<li>confirm: Este componente es para confirmar el pedido del usuario.
<li>payment: Este componente es para procesar el pago del pedido del usuario e ingresar datos de compra.
</ul>


**Back-end:**

El paquete principal contiene la lógica principal del negocio de la aplicación.
<ul>
<li>Core: Este subpaquete contine las clases y funciones centrales para la aplicación.
<li>Business: Este subpaquete contine clases y funciones relacionadas con las reglas de negocio de la aplicación.
<li>Database: Este subpaquete contine clases y funciones para interactuar con la base de datos.
<li>Model: Este subpaquete contine clases que representan los modelos de datos de la aplicación.
<li>StoreApi: Este subpaquete contine controladores de API para enviar y recibir datos de front-end.
<li>Controller: Este subpaquete contine clases que implementan los controladores de API específicos.
<li>UT: Este subpaquete contiene las pruebas unitarias para el código del back-end.
</ul>

**Dependencias:**
<ul>
<li>El front-end depende del back-end para obtener datos y realizar acciones.
<li>Los componentes del front-end se comunican con el back-end a través de API.
<li>El back-end depende de la base de datos para almacenar y recuperar datos.
</ul>


## Diagrama Estructura de la aplicación

![Fetch](Fetch.jpeg)

A[HomePage] --> B{Products}
B --> C{Database}
B --> D{Store}
C --> E{Cache}


<ul>
<li>
<li> 
</ul>