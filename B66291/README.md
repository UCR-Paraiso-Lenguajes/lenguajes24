# GeekStore WebApp

Welcome to the Online Geek Store project! This project aims to provide a platform for purchasing various technological products conveniently online. Users can explore a wide range of products, make purchases, and utilize several key features outlined below.

## Features

### • Product Catalog
Browse through an extensive catalog of technological products, including games, laptops, accessories, and more. Each product listing includes detailed information snd pricing.

### • Payment Options
Choose between two convenient payment methods: cash on delivery or mobile money (Sinpe Movil). Our platform ensures secure transactions for a seamless shopping experience.

### • Weekly / Daily Sales Reports
Stay informed about the store's performance with weekly sales reports. These reports provide valuable insights into sales trends, popular products, and revenue generation. Accessible to authorized personnel for strategic decision-making.

### • Category Search
Effortlessly find products by browsing through specific categories. Whether you're looking for entertaiment, laptops, or accessories, our category 
search functionality simplifies the shopping process.

### • Messages CRUD
Implementing websockets technology we have an option to provide campains visualization, in this part the user is able to recieve notifications in real time implementing, those messages are going to be delivered by the admin, every time an important update comes in the webapp.


## Internal design of the site

### ‣ Security management 

Security management is done through JSON web tokens, these web tokens are generated when the user logs in with their username and password, the token with a role is placed in the session storage section of the browser, through the session if the user wants to perform actions on the page these actions can be performed in the application depending on the role that has been assigned through their credentials.

### ‣ Making of cache

To manage the products, which in principle are stored in a database, a transactional query is performed the first time the page is loaded so that the products are extracted and stored in a cache, this cache is composed of a data dictionary that improves the performance of the web application.

### ‣ Product search

In the section related to the product search, a search by categories is implemented where the user enters the product name and by means of a query to the api and a search in logarithmic time the products that the user needs are returned. These products ara obtained from mysql database, stored in a memory list, and consulted from it the next time the user performs the search to improve the performance of the web application.

### ‣ Sales reports 

The implementation of the sales reports was done through google charts, there is a weekly sales report based on the transactional query made through the api to the database that is displayed in a pie chart and a daily sales report that is displayed in a table.

![Backend Diagram](https://raw.githubusercontent.com/jazielrs/Pagina_compra/master/Reportes_captura.png)

### Web arquitecture diagrams

####  ❖ Buying process UML

![Backend Diagram](https://raw.githubusercontent.com/jazielrs/Pagina_compra/master/Diagrama_Uml_Pagos.png)

#### ❖ Buying process activity

![Backend Diagram](https://raw.githubusercontent.com/jazielrs/Pagina_compra/master/Diagrama_actividad_Compra.png)
## API Reference

#### Post user information

```http
  POST /api/login
```

| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
|  |  | Provides login authorization |

#### Post campains

```http
  POST /api/campannas
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `campain`      | `Campain` | The campain that is going to be sent to the database to provide historical record  |

#### Post cart

```http
  POST /api/cart
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `cart`      | `Cart` | Manages the shopping experience process and returns a purchase number for the user to view |

#### Get payment methods

```http
  GET /api/pago
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| ----- | ----- | Obtains the payment methods stored in the database to provide them to the user in the purchase process |


#### Get products

```http
  GET /api/store
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| ----- | ----- | Retrieves the products stored in the database and stored them in a memory list |

### Get reports

```http
  GET /api/store
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| date | string | Retrieves the data needed to create weekly and daily reports and returns it in JSON format  |

### Post products

```http
  POST /api/product
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| product | ProductAlt | Saves the product obtained by parameter into dtabase |








## Color Reference

| Color             | Hex                                                                |
| ----------------- | ------------------------------------------------------------------ |
| Black | ![#000000](https://via.placeholder.com/10/000000?text=+) #000000 |
| White | ![#f8f8f8](https://via.placeholder.com/10/f8f8f8?text=+) #f8f8f8 |
| Green | ![#00d1a0](https://via.placeholder.com/10/00b48a?text=+) #00d1a0 |

