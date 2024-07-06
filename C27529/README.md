# Sistema de Gestión de Tienda Online

## Descripción del Proyecto

## Tabla de Contenidos
- [Instalación](#instalación)
- [Uso](#uso)
- [Características](#características)
- [Documentación Técnica](#documentación-técnica)
- [Contribución](#contribución)
- [Pruebas](#pruebas)
- [Despliegue](#despliegue)
- [Roadmap](#roadmap)
- [Autores](#autores)
- [Agradecimientos](#agradecimientos)


## Instalación
### Requisitos Previos
- Docker y Docker Compose
- .NET 6 SDK
- Node.js v14+ (para el cliente React)
- npm v6+ (para el cliente React)

---
---
#### Docker y Docker Compose
**En Ubuntu/Debian:**
```bash
# Instalar Docker
sudo apt update
sudo apt install -y apt-transport-https ca-certificates curl software-properties-common
curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo apt-key add -
sudo add-apt-repository "deb [arch=amd64] https://download.docker.com/linux/ubuntu $(lsb_release -cs) stable"
sudo apt update
sudo apt install -y docker-ce

# Instalar Docker Compose
sudo curl -L "https://github.com/docker/compose/releases/download/$(curl -s https://api.github.com/repos/docker/compose/releases/latest | grep tag_name | cut -d '"' -f 4)/docker-compose-$(uname -s)-$(uname -m)" -o /usr/local/bin/docker-compose
sudo chmod +x /usr/local/bin/docker-compose

# Verificar instalación
docker --version
docker-compose --version

```
**En macOS:**
```bash
# Instalar Docker y Docker Compose
brew install --cask docker
open /Applications/Docker.app

# Verificar instalación
docker --version
docker-compose --version

```
**En Windows:**
- Descargar e instalar Docker Desktop desde la página Docker.
---
---

#### .NET 6 SDK

**En Ubuntu/Debian:**
```bash
# Añadir la clave Microsoft
wget https://packages.microsoft.com/config/ubuntu/$(lsb_release -rs)/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb

# Instalar .NET SDK
sudo apt update
sudo apt install -y dotnet-sdk-6.0

# Verificar instalación
dotnet --version

```
**En macOS:**
```bash
# Usar Homebrew para instalar .NET SDK
brew install --cask dotnet-sdk

# Verificar instalación
dotnet --version


```
**En Windows:**
- Descargar e instalar .NET SDK desde la página oficial.
---
---


#### Node.js v14+ y npm v6+

**En Ubuntu/Debian:**
```bash
# Instalar Node.js y npm desde el repositorio NodeSource
curl -fsSL https://deb.nodesource.com/setup_14.x | sudo -E bash -
sudo apt install -y nodejs

# Verificar instalación
node -v
npm -v

```
**En macOS:**
```bash
# Usar Homebrew para instalar Node.js y npm
brew install node@14

# Verificar instalación
node -v
npm -v
```
**En Windows:**
- Descargar e instalar Node.js desde la página oficial.
---
---


## Uso

### Ejemplos de Uso

1. Inicia el servidor:
   ```bash
   git clone https://github.com/chmod2314/ecommerceNode.git

    ```

2. Inicia el servidor:
   ```bash
   cd ./storeApi
   dotnet run --launch-profile http

    ```
3. Inicia el cliente:
   ```bash
   cd .next/nextjs-dashboard
   npm run dev

    ```
## Características

### Funcionalidades Principales

- **Búsqueda de productos**: Permite a los usuarios buscar productos por nombre o categoría utilizando parámetros de consulta.
- **Carrusel de productos**: Muestra productos destacados en un carrusel interactivo en la página principal.
- **Carrito de compras dinámico**: Los administradores pueden agregar y eliminar productos del carrito de compras en tiempo real.
- **Notificaciones en tiempo real**: Los usuarios reciben notificaciones sobre comunicaciones importantes dadas por el administrador mediante el uso de Web Sockets.
- **Autenticación JWT**: La aplicación utiliza tokens JWT para la autenticación segura de los usuarios y de la base de datos.
- **Panel de administración**: Los administradores pueden agregar productos, desactivar métodos de pago, enviar campañas y ver reportes de ventas.
- **Soporte para múltiples métodos de pago**: La aplicación soporta varios métodos de pago que pueden ser habilitados o deshabilitados por el administrador.
- **Interfaz responsiva**: La aplicación está optimizada para funcionar en dispositivos móviles, tabletas y computadoras de escritorio.
- **Documentación API con Swagger**: La API está documentada utilizando Swagger, proporcionando una interfaz interactiva para probar los endpoints.


### Características Técnicas

- **Desarrollado con ASP.NET Core**: El backend de la aplicación está construido con ASP.NET Core, proporcionando un rendimiento y escalabilidad óptimos.
- **Cliente React**: La interfaz de usuario está desarrollada con React, ofreciendo una experiencia de usuario dinámica y reactiva.
- **Docker**: La aplicación está preparada para ejecutarse en contenedores Docker, facilitando la implementación y la gestión de dependencias.
- **SignalR**: Utiliza SignalR para la comunicación en tiempo real, permitiendo notificaciones instantáneas y actualizaciones de estado.
- **Base de datos MySQL**: Utiliza MySQL como base de datos para almacenar productos, ventas, y mensajes.
- **Pruebas Unitarias**: Se realizaron pruebas unitarias para validar las funcionalidades y garantizar la calidad del código.

---




## Documentación-técnica
**Diagramas de Secuencia De Tienda**

![Diagramas de Secuencia De Tienda](https://www.plantuml.com/plantuml/png/ZPDFgXf14CRtSugwW1UO1uydWuW1WoXa6yhkGbfwkhIwMvBWXRB8AhjivsAfLdGKHvwhORf_N_tvlfcFc51IRdrrRBoZaEXOrGs7c7r2s0dRR1B7jwewVgKVwKlyViQKceH3zmSXidV6fMp1qhrWl9WfDgMqE9_guUtRwBrXeHfMvm88tkambakTn9OZU8wt5Ul2ZzvNUxyYEP3KCDcIoQhSPvAVptLNnUZISqc99I3C9Y6t9BZsfFA7Pihh4Y_XoYbzvz4hkaTRvnoJeFGGK80TXeJUOn7qEOqtGXky8T63GH6N-ErwQvsGCQxx6qfhLPRJ_hhjVXVJJcFXfkYMXDxzGl08XjkTuD2K2vsoQ9eheGRq7mMljvGO6fIa5SlZulmtBMxHE0uq6EdOjsWetBKV271AWGGX1mZTFxNsj82GSMj-aXtv8VHniy_utlvv9etg6Vb1mPR_gleF)

Este diagrama muestra la secuencia de la tienda en un panorama general, desde el momento en el que el cliente envía la solicitud hasta el momento de la compra pasando por los todos los componentes del proyecto (Cliente, Aplicación y API).

**Diagramas de Flujo Campañas**

![Diagrama de Flujo de Campañas ](https://www.plantuml.com/plantuml/png/TLF1RXD13BtlL_YOI2WHGbms1qeJ19Ke1qh5sPbnag4JppBsH8M_wGTmG7m04lanFDakQHMQGxJQPx_dz-mz4yMiPHkRmqFJBWhjVoRm11510x578DQCWB1HxThfT3APJ1rkEsH6cJNjPHB3Cym_NYzguvPOy0iPr8GLZFxydcSoc0ljqW2-6eDkY64a-n_Z1kpNNl0was0UIc2qtT_TJk3b3JZCw9GCVWPT8TC8x6896Tvzkdm_dlK4Rmjc_w2Vk7euNnXHqfI1Li45OYLhe2WcmEbU-ueYEHSIFso_3QArlAVlhPodR5QVTd5WUrm4dlevUXvY7rPD_z-q7v866ykg4OgOWP04N69oYZxLT2_OPJ8gnLo1-Ze_JhbzxRHW3D-Fcc9ao3x1PrfU9NT3AZPaOCYqoYHLhzlVhGFZ-D-MhdjvsA0ZS36u6Y15Y6cTgc2_SfHJqyC57DKn3cvEefwRXy260UEErkZ2_XS3yV7UPaCwmp5skT0IwonILxB56WIy0CMbWAV_1GVVu7d5A-mFB9RJ6zw5uvHMigDyd_xNGY0MQg7g1B1eikjJmshq_IqbETMq-4LJrvmH-_eL_GK0)

Se muestra el flujo que sigue cuando el administrador gestiona las campañas, incluyendo creacion y eliminación de las mismas asi como su notificacion haciendo uso de los WebSockets.


![Diagrama de Flujo de Campañas ](https://www.plantuml.com/plantuml/png/TL71JW913BtFL_G85xZl7YI0KHFn01BFPRR0wEvqCzFrjB_amVW19l9ZTXY89cWFcqtxNkUzrqaK3D8rjUdd7HtU6Ig26a4S-GfxSnoRujfBG43OYxJ5U3mQZSOMcnQznrYQOhPSpnEn8H_ncRJbjZ2O1S80lgDNFa-kXY1xyZ28XxUX0QtYpky2HGnd20nk7nVtq59edBYAXsN6tNGOAeKfUpel3TLHePvFMrPKaxME_S-Yp4j2lZwd70A6_rMaAZ8e1w1lPBx6uBOcVui75hTrLa5TL3cE8rZsP0KhLjFz-hBRcs99rcsS9d5YMtVuyALHvpQGIjA84o71mqMAAdvMEvjirxmxdYSdZ_6FC1SSvPVHg3hHLmnFj5cnVI7HfTbB56xDHF_JxRy1)

Este es una version extendida del diagrama anterior mostrando como se refleja en el usuario las campañas.

**Diagramas de Flujo Habilitar Metodos de Pago**
![Diagramas de Flujo Habilitar Metodos de Pago](https://www.plantuml.com/plantuml/png/RPAzJiCm4CTtFyMDxIia0sLWgCI0G3p0jRwqbfnpiCyCl4r71fu1YRmO5xk1LaqKHP7p__ZvuaqIZ99xRygBQUvjxnWifIFkd7U2a1dwyLE23ReC0nv2gxAdBU1UCdhtWU3rTXgWWbcRPl5bbYwtnhWELZ_VZmZ3U3euBgvzw8U8G4d64wYQek3T6kH83AkNyMjjGAz6JQzR23jnnFHV-qTY5n7QOgwr3tx-l0nuHj3QdC8jGQ5u9YsythvI3HWHthAcHA2Rbif0R5rdwjCqoZBDyLrJx6Ib1HzmW4IeD0MEeSkI8wRpn59RSx3yjZgMpAXM5lGU2zNb3kF2T4oIC9YDOaq7uHS0)

El diagrama muestra el flujo que se sigue cuando el administrador actualiza el estado de los metodos de pago desde la pagina de administrador.


**Diagramas de Flujo Compra Carrusel**
![Diagramas de Flujo Compra Carrusel](https://www.plantuml.com/plantuml/png/JP11JiD034NtSmelqoiq2xfX2K927C1Dt39eOeziJ-_30LXn02HoCHmAJMPbpRnvNzy7SrHlKqc_Gqhx5skeMU12G0K6L6qMmqXGLSOsk5Y_KbN9iZ0kx55y_kI5nBFI6HNmAiWkG0oDRug0LdUVKZv1z_rrNrQ0pETt6BDLuNmiTBS3Vmr9zpn_x1B4sJ_zizUuKJPvVQ9Y1DsZ_C4FOgweC14Rlj6s3x1iWk8dZ_cKDXqR1Jq8U-Qu0SPBn77aoU8OubK1eM0mKrMCFkPIqo54osP_0000)

Como el usuario puede agregar productos al carrito desde el carrusel de productos actualizando el carrito.

**Diagramas de Secuencia de Autenticación**
![Diagramas de Secuencia de Autenticación ](https://www.plantuml.com/plantuml/png/PP91JiCm44NtFiMegrO2PH-XgY5223P4rC1w50-jXUk9nauNt8OpS8HU32SoKQ9kP_uR_MTv5o9Ax4zEOHjPu2tqA9PLbwQsjHtw2AlQMVAHLe01wkMgwkEnPX-5dICP4zKoeVcB_9xaR5iQztgvppGQbaOuqi4Q7dEDKhaDt6wXBk799mWXKB2NNm_hLiYGJoD7OQFg8LMLqBpiDHJOsG9JjyBnmLvbgo6hItX7PmqApBTAvqFrR0ZdoyyG3HFwI9uaeN7GW-SFFN5wcbIv-2k5hgSG4LhsCsBfbsAfpdSIP9T4Os-Wom-2i9weRDga2W-w66ndmFf8Q4YWIkeiwLYqx4ku9vGqlHkFR3Ug-VSdiP-slTPeiaRsvXleI4usS7g9p1WEghaIH70p0RKZRuRFfTGV)

Este diagrama describe el proceso de autenticacion y autorizacion completa utilizando JWT. Desde el inicio de sesion hasta el uso del token en solicitudes.

## Estructura del Código

![Diagrama de Flujo de Campañas ](https://www.plantuml.com/plantuml/png/VPHDRXin38NtFeNX0_WCQUjrksZt1KTw7QlLZq1oWWI5xnv7I2QogOm1R_ZnIUIR1ps84kkIugwI-qSpzWUuaXBOuNlnEEp_x_RxbRd2RwMkE2qY8KCQQBy_8LxheeKHonpSqScXeSkjqfFILnBSAtzIn6bgghxyhMGu3IMVfdQsdpuxE1MFQ0Pon6eETQIO2pyRKEav8Mk2delVfgTVFsm35xyuUvzGX2csTVfbshztFkD9ZVDKw-sEhNJyAq_lntTVBjMIaNLrfTDupv1tcN5iKCSZiY85LV2Lt-Cpo8DNeRLjHTOmwTbbz779fv3lzwvND5myvA33bN-_NJZUErNAcMG4FI8K8_Lubd5cHWsCMlZ30PCGnuiVshEs7P4V3uFuDFcN6ao4NCdA9KQmc2WjUlx0BPt6ev3dpHRM3PmWGZDat68jiuqcFHiTGX5RM0jRtfpiBvcPKcFMuwNbpBui2j4ckPt5JmFGfZXPqh-cdy7NkgopNNOFoFxrPNy1)

## Roadmap

El proyecto tiene varias funcionalidades planificadas para futuras versiones. A continuación se detalla el roadmap con las próximas características y mejoras a implementar:

### Versión Actual

- Implementación básica de la tienda online.
- Autenticación y autorización con JWT.
- Gestión de productos y métodos de pago.
- Funcionalidad de carrito de compras.
- Notificaciones en tiempo real mediante WebSockets.
- Administración de campañas y mensajes.

### Próximas Implementaciones

#### V1.1 - Implementación de Funcionalidades Adicionales

1. **Compra de Múltiples Artículos del Mismo Producto**
   - Permitir a los usuarios seleccionar y comprar múltiples unidades de un mismo producto.
   - Actualizar la lógica del carrito de compras para manejar cantidades.
   - Modificar la interfaz de usuario para permitir la selección de cantidad.

   **Descripción del Proceso**:
   - El usuario selecciona un producto de la tienda.
   - El usuario selecciona la cantidad deseada del producto.
   - El usuario añade el producto al carrito con la cantidad seleccionada.
   - El carrito de compras se actualiza para reflejar la cantidad seleccionada.
   - El usuario puede ver la cantidad actualizada en el carrito y proceder a la compra.

2. **Inicio de Sesión de Usuario**
   - Permitir a los usuarios crear e iniciar sesión para una experiencia más personalizada.
   - Añadir formularios de registro y login en la interfaz de usuario.
   - Gestionar la autenticación de usuarios y el manejo de sesiones.

   **Descripción del Proceso**:
   - El usuario navega a la página de registro o login.
   - El usuario completa el formulario de registro con sus datos (nombre, correo electrónico, contraseña) y envía la información.
   - El sistema valida la información y crea una cuenta para el usuario.
   - El usuario inicia sesión ingresando su correo electrónico y contraseña en el formulario de login.
   - El sistema valida las credenciales del usuario.
   - Si las credenciales son correctas, el sistema genera un token JWT para la sesión del usuario.
   - El usuario es redirigido a la página principal con su sesión iniciada y puede acceder a funciones personalizadas.

#### V1.2 - Mejoras en la Experiencia de Usuario

1. **Revisión y Valoración de Productos**
   - Permitir a los usuarios dejar reseñas y valoraciones de los productos.
   - Mostrar reseñas y valoraciones en la página de cada producto.

2. **Historial de Pedidos**
   - Implementar una sección donde los usuarios puedan ver el historial de sus pedidos.
   - Mostrar detalles de cada pedido, incluyendo productos, cantidades y estado del pedido.



## Autores

- Jose Solano Araya [j14solano23@gmail.com](https://www.linkedin.com/in/jose-solano-araya-6b558b234/)
 




