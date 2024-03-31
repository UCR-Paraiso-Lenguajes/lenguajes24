import React from 'react';
import CarouselBanner from '@/app/ui/components/carrusel';
import CarruselDemo from '@/app/ui/dataCarrusel';
const CarouselStore = () => {
    return(
        <div>
            <div className= 'container'>
                <div id="carouselExampleIndicators" className="carousel slide" data-bs-ride="carousel">
                    <div className="carousel-indicators">
                        {CarruselDemo.map((banner, i) => (
                            <button key= {banner.id} type = "button "data-bs-target="#carouselExampleIndicators" data-bs-slide-to={i} className={i === 0 ? ' active' : ''} aria-current={i === 0 ? 'true' : 'false'} aria-label={`Slide ${i + 1}`} />                                    
                    ))}
                    </div>
                    <div className = "carousel-inner">
                        {CarruselDemo.map((banner) => (
                            <CarouselBanner key= {banner.id} banner = {banner} />                                    
                        ))}
                    </div>
                        <button className="carousel-control-prev" type="button" data-bs-target="#carouselExampleIndicators" data-bs-slide="prev">
                            <span className="carousel-control-prev-icon" aria-hidden="true"></span>
                            <span className="visually-hidden">Previous</span>
                        </button>
                        <button className="carousel-control-next" type="button" data-bs-target="#carouselExampleIndicators" data-bs-slide="next">
                            <span className="carousel-control-next-icon" aria-hidden="true"></span>
                            <span className="visually-hidden">Next</span>
                        </button>
                </div>
            </div>
        </div>
  
    );
  };
  export default CarouselStore;
