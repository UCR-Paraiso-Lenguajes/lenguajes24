import type { Product } from '../lib/data-definitions';
import Image from 'next/image';

export default function Product({ product, onAdd }: { product: Product, onAdd: any }) {
  const { name, description, imageUrl, price } = product;

  return (
    <div className="col-sm-3">
      <div className="card">
        <img src={product.imageUrl}
          width={1000}
          height={760}
          className="card-img-top hidden md:block" alt={product.name} />
        <div className="card-body">
          <div className="mb-3">
            <span className="float-start badge rounded-pill bg-primary">{product.name}</span>
          </div>
          <div className="specifications">
            <div className="specifications-content" dangerouslySetInnerHTML={{ __html: product.description }} />
          </div>
          <span className="float price-hp">₡ {product.price}</span>
          <div className="text-center my-4">
            {/* Aquí llamamos a handleClick cuando se hace clic en el botón "Buy" */}
            <a onClick={() => onAdd({ product })} href="#" className="btn btn-warning">Buy</a>
          </div>
        </div>
      </div>
    </div>
  );
};
