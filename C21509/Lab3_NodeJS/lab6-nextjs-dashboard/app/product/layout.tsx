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
  quantity: number;
  description: string;
  price: number;
}

export const products: ProductItem[] = [
  {
    id: 1,
    name: "Iphone",
    imageURL: "/img/Iphone.jpg",
    quantity: 0,
    description: "Producto nuevo",
    price: 200
  },

  {
    id: 2,
    name: "Audifono",
    imageURL: "/img/audifonos.jpg",
    quantity: 0,
    description: "Producto nuevo",
    price: 100
  },

  {
    id: 3,
    name: "Mouse",
    imageURL: "/img/mouse.jpg",
    quantity: 0,
    description: "Producto nuevo",
    price: 35
  },

  {
    id: 4,
    name: "Pantalla",
    imageURL: "/img/Pantalla.jpg",
    quantity: 0,
    description: "Producto nuevo",
    price: 68

  },

  {
    id: 5,
    name: "Headphone",
    imageURL: "/img/Headphone.jpg",
    quantity: 0,
    description: "Producto nuevo",
    price: 35

  },

  {
    id: 6,
    name: "Teclado",
    imageURL: "/img/teclado.jpg",
    quantity: 0,
    description: "Producto nuevo",
    price: 95

  },

  {
    id: 7,
    name: "Cable USB",
    imageURL: "/img/Cable.jpg",
    quantity: 0,
    description: "Producto nuevo",
    price: 10
  },

  {
    id: 8,
    name: "Chromecast",
    imageURL: "/img/Chromecast.jpg",
    quantity: 0,
    description: "Producto nuevo",
    price: 150
  }

];

export const Product = ({ product, addToCart }: { product: ProductItem, addToCart: (product: ProductItem) => void }) => {
  const { name, imageURL, description, price } = product;

  const renderDescription = (description: string) => {
    return { __html: description };
  };

  return (
    <div className="products col-sm-6">
      <div className="product col-sm-6"><img src={imageURL} alt={name} /></div>
      <p className="col-sm-6">{name}</p>
      <p className="col-sm-6" dangerouslySetInnerHTML={renderDescription(description)}></p>
      <p className="col-sm-6">Precio: ${price}</p>
      <button className="button" onClick={() => addToCart(product)}>Comprar</button>
    </div>
  );
};

