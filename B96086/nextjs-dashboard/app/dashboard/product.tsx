
export default function Product({ product, onAdd }: { product: Product, onAdd: any }) {

    const { uuid, name, description, imageUrl, price } = product;
    return (
      <div className="col-sm-3">
        <div className="card">
          <img src={product.imageUrl}
            className="card-img-top" alt={product.name} />
          <div className="card-body">
            <div className="mb-3">
              <span className="float-start badge rounded-pill bg-primary">{product.name}</span>
              <span className="float-end price-hp">₡{product.price}</span>
            </div>
            <div className="specifications">
              <div className="specifications-content">
                <p>{product.description}</p>
              </div>
            </div>
            <div className="text-center my-4">
              {/* <button onClick={handleClick} className="btn btn-warning">Buy</button> */}
              <button onClick={()=> onAdd({product})} href="#" className="btn btn-warning" >Comprar</button>
              {/* Hiciste click {count} veces */}
            </div>
          </div>
        </div>
      </div>
    );
  };