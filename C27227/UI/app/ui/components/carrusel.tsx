import React from 'react';

const CarouselBanner = ({ banner, handleAddToCart }) => {
  const { id, name, imageUrl } = banner;
  let className = 'carousel-item';
  className = id === 1 ? className + ' active' : className;

  return (
    <div className={className}>
      <img src={imageUrl} className="d-block w-90" alt={name} />
      <div className="carousel-caption d-none d-md-block">
        <h5>{name}</h5>
        <button className="btn btn-primary" onClick={() => handleAddToCart(banner)}>
          Add to Cart
        </button>
      </div>
    </div>
  );
};

export default CarouselBanner;
