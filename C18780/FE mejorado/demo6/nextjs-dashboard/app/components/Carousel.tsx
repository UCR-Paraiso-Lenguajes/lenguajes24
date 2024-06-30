import { Product } from "../utils/data-definitions";
import { getInitialCartLocalStorage, saveInitialCartLocalStorage } from "../utils/utils";
import CardProduct from "./CardProduct";

const Carousel = ({ products, onAdd }: { products: Product[], onAdd: any }) => {
    const chunkSize = 4;
    const productChunks = [];

    if (products === undefined) {
        return (<></>);
    }

    for (let i = 0; i < products.length; i += chunkSize) {
        productChunks.push(products.slice(i, i + chunkSize));
    }
    return (
        <div className="container-products">
            <div id="carouselExampleDark" className="carousel carousel-dark slide" data-bs-ride="carousel"
                data-interval="5000" data-pause="hover">

                <div className="carousel-inner">
                    {productChunks.map((chunk, index) => (
                        <div key={index} className={`carousel-item ${index === 0 ? "active" : ""} `}>
                            <div className="row d-flex flex-row justify-content-center align-items-center">
                                {chunk.map((product, index) => (
                                    <CardProduct key={index} product={product} onAdd={onAdd} />
                                ))}
                            </div>
                        </div>
                    ))}
                </div>

                <button className="carousel-control-prev" type="button" data-bs-target="#carouselExampleDark"
                    data-bs-slide="prev">
                    <span className="carousel-control-prev-icon" aria-hidden="true"></span>
                    <span className="visually-hidden">Previous</span>
                </button>
                <button className="carousel-control-next" type="button" data-bs-target="#carouselExampleDark"
                    data-bs-slide="next">
                    <span className="carousel-control-next-icon" aria-hidden="true"></span>
                    <span className="visually-hidden">Next</span>
                </button>
            </div>
        </div>
    )
}

export default function Page({ products }: { products: Product[] }) {
    const initialCart = getInitialCartLocalStorage();
    const handleAddToCart = ({ product }: { product: Product }) => {
        initialCart.cart.products.push(product);
        initialCart.cart.subtotal += product.price;
        initialCart.cart.total = initialCart.cart.subtotal + initialCart.cart.subtotal * initialCart.cart.taxPercentage;
        saveInitialCartLocalStorage(initialCart);
    }
    return (<><Carousel products={products} onAdd={handleAddToCart} /></>);
}