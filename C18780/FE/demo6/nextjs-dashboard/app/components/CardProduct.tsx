'use client'
import { Product } from '../utils/data-definitions';
import BtnBuy from './Btn/BtnBuy';

export default function CartProduct({ product, onAdd }: { product: Product, onAdd: any }) {
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
            <a className='btn' onClick={() => onAdd({ product })}><BtnBuy price={product.price} /></a>
          </div>
        </div>
      </div>
    </div>
  );
};
