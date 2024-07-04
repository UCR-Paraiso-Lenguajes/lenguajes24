'use client'
import { Product } from "../utils/data-definitions";
import CardProduct from "./CardProduct";

const Carousel = ({ products, onAdd }: { products: Product[], onAdd: (product: Product) => void }) => {
  const chunkSize = 4;
  const productChunks: Product[][] = [];

  if (!products) {
    return null;
  }

  for (let i = 0; i < products.length; i += chunkSize) {
    productChunks.push(products.slice(i, i + chunkSize));
  }

  return (
    <div className="container-products">
      <div id="carouselExampleDark" className="carousel carousel-dark slide" data-bs-ride="carousel"
        data-bs-interval="5000" data-bs-pause="hover">
        <div className="carousel-inner">
          {productChunks.map((chunk, index) => (
            <div key={index} className={`carousel-item ${index === 0 ? "active" : ""}`}>
              <div className="row d-flex flex-row justify-content-center align-items-center">
                {chunk.map((product) => (
                  <CardProduct key={product.uuid} product={product} onAdd={onAdd} />
                ))}
              </div>
            </div>
          ))}
        </div>
        <button className="carousel-control-prev" type="button" data-bs-target="#carouselExampleDark" data-bs-slide="prev">
          <span className="carousel-control-prev-icon" aria-hidden="true"></span>
          <span className="visually-hidden">Previous</span>
        </button>
        <button className="carousel-control-next" type="button" data-bs-target="#carouselExampleDark" data-bs-slide="next">
          <span className="carousel-control-next-icon" aria-hidden="true"></span>
          <span className="visually-hidden">Next</span>
        </button>
      </div>
    </div>
  );
}

export default Carousel;
