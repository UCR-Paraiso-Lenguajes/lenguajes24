
import React from 'react';
const CarouselBanner = ({banner}) =>  {
    const {name, imageUrl} = banner;
    let className = 'carousel-item';
    className = banner.id  === 1 ? className + ' active ' : className;
    return(
        <div className={className}>
            <img src={imageUrl} className="d-block w-90" alt={name}/>
        </div>
    );
  };
  export default CarouselBanner;