import Image from "next/image";
import '../ProductGrid.css'; // Archivo CSS para estilos
import { Products } from '../app/components/Products.jsx';

export default function Home() {
// Lista de productos de ejemplo
const products = [
  { id: 1, name: 'Producto 1', image: 'url_de_la_imagen_1', description: 'Descripción del Producto 1'  },
  { id: 2, name: 'Producto 2', image: 'url_de_la_imagen_1', description: 'Descripción del Producto 1'  },
  { id: 3, name: 'Producto 3', image: 'url_de_la_imagen_1', description: 'Descripción del Producto 1'  },
  { id: 4, name: 'Producto 4', image: 'url_de_la_imagen_1', description: 'Descripción del Producto 1'  },
  { id: 5, name: 'Producto 5', image: 'url_de_la_imagen_1', description: 'Descripción del Producto 1'  },
  { id: 6, name: 'Producto 6', image: 'url_de_la_imagen_1', description: 'Descripción del Producto 1'  },
  { id: 7, name: 'Producto 7', image: 'url_de_la_imagen_1', description: 'Descripción del Producto 1'  },
  { id: 8, name: 'Producto 8', image: 'url_de_la_imagen_1', description: 'Descripción del Producto 1'  },
  { id: 9, name: 'Producto 9', image: 'url_de_la_imagen_1', description: 'Descripción del Producto 1'  },
  { id: 10, name: 'Producto 10', image: 'url_de_la_imagen_1', description: 'Descripción del Producto 1'  }
];

return (
  <div className="product-grid">
  {products.map(product => (
    <div key={product.id} className="product-item">
      <img src={product.image} alt={product.name} className="product-image" />
      <h3 className="product-name">{product.name}</h3>
      <p className="product-description">{product.description}</p>
      <button className="buy-button">Comprar</button>
    </div>
  ))}
</div>
);
}
