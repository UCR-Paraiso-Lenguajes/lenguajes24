import NavLinks from '@/app/ui/dashboard/nav-links';
import { FaShoppingCart } from 'react-icons/fa';
// export default function Navbar({ countCart = 0 }: { countCart: number }) {
export default function Navbar({ countCart = 0 }: { countCart: number }) {
  return (
    <nav className="navbar navbar-expand-lg navbar-light bg-custom-blue fixed-top">
      <div className="container">
        <div className="navbar w-22">
          {/* Navbar left content */}
          <div className="navbar-left vet-poveda">Vet Poveda</div>
        </div>
        <div className="navbar-center w-50" style={{ marginRight: '12px' }}>
          {/* Navbar center content */}
          <div className="input-group w-100">
            <input
              className="form-control w-75 mr-2"
              type="search"
              placeholder="Buscar productos"
              aria-label="Search"
            />
            <div className="input-group-append">
              <button className="btn btn-primary" type="submit">
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  width="16"
                  height="24"
                  fill="currentColor"
                  className="bi bi-search"
                  viewBox="0 0 16 16"
                >
                  <path
                    d="M11.742 10.344a6.5 6.5 0 1 0-1.397 1.398h-.001q.044.06.098.115l3.85 
                    3.85a1 1 0 0 0 1.415-1.414l-3.85-3.85a1 1 0 0 0-.115-.1zM12 6.5a5.5 5.5 0 1 
                    1-11 0 5.5 5.5 0 0 1 11 0"
                  />
                </svg>
              </button>
            </div>
          </div>
        </div>
        <div className="navbar-right w-25">
          {/* Navbar right content */}
          <div className="navbar-right d-flex align-items-center">
            <NavLinks countCart={countCart} />
            

          </div>
        </div>
      </div>
    </nav>
  );
}
