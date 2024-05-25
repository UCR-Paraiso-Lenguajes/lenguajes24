"use client"

export default function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {

  return (
    <html lang="en">
      <body>{children}</body>
    </html>
  );
}

export interface ProductItem {
  id: number;
  name: string;
  imageURL: string;
  price: number;
}

export const products: ProductItem[] = [
  {
    id: 1,
    name: "Iphone",
    imageURL: "/img/Iphone.jpg",
    price: 200
  },

  {
    id: 2,
    name: "Audifono",
    imageURL: "/img/audifonos.jpg",
    price: 100
  },

  {
    id: 3,
    name: "Mouse",
    imageURL: "/img/mouse.jpg",
    price: 35
  },

  {
    id: 4,
    name: "Pantalla",
    imageURL: "/img/Pantalla.jpg",
    price: 68

  },

  {
    id: 5,
    name: "Headphone",
    imageURL: "/img/Headphone.jpg",
    price: 35

  },

  {
    id: 6,
    name: "Teclado",
    imageURL: "/img/teclado.jpg",
    price: 95

  },

  {
    id: 7,
    name: "Cable USB",
    imageURL: "/img/Cable.jpg",
    price: 10
  },

  {
    id: 8,
    name: "Chromecast",
    imageURL: "/img/Chromecast.jpg",
    price: 150
  }

];

export const Product = ({ product, addToCart }: { product: ProductItem, addToCart: (product: ProductItem) => void }) => {
  const { name, imageURL, price } = product;
  return (

    <div className="products col-sm-6">
      <div className="product col-sm-6"><img src={imageURL} alt={name} /></div>
      <p className="col-sm-6">{name}</p>
      <p className="col-sm-6">Precio: ${price}</p>
      <button className="button" onClick={() => addToCart(product)}>Comprar</button>
    </div>

  );



};

