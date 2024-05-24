import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap-icons/font/bootstrap-icons.css';

const NavBar = ({ productCount, toggleCart, searchFunction, setQuery }:
    { productCount: number, toggleCart: (action: boolean) => void, searchFunction: () => void, setQuery: (text: string) => void }) => {


    function handleQueryChange(event: React.ChangeEvent<HTMLInputElement>) {
        const inputValue = event.target.value;
        setQuery(inputValue);
    }

    return <>
        <nav className="navbar navbar-expand-lg sticky-top navbar-dark bg-dark">
            <div className="container">
                <div className="navbar-left">
                    <button className="navbar-brand btn btn-outline-success" type="submit" onClick={() => toggleCart(false)}>
                        Andromeda
                    </button>
                </div>
                <div className="navbar-center w-75">
                    <div className="input-group w-100">
                        <input className="form-control mr-2 w-25" type="search" placeholder="Buscar"
                            aria-label="Search" onChange={handleQueryChange} />
                        <div className="input-group-append">
                            <button className="btn btn-outline-success" type="submit" onClick={searchFunction}>
                                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="24" fill="white"
                                    className="bi bi-search" viewBox="0 0 16 16">
                                    <path
                                        d="M11.742 10.344a6.5 6.5 0 1 0-1.397 1.398h-.001q.044.06.098.115l3.85 
                                        3.85a1 1 0 0 0 1.415-1.414l-3.85-3.85a1 1 0 0 0-.115-.1zM12 6.5a5.5 5.5 0 1 
                                        1-11 0 5.5 5.5 0 0 1 11 0" />
                                </svg>
                            </button>
                        </div>
                    </div>
                </div>
                <div className="navbar-right d-flex align-items-center">
                    <button className="btn btn-outline-success" type="submit" onClick={() => toggleCart(true)}>
                        {productCount}
                        <svg xmlns="http://www.w3.org/2000/svg" width="15" height="30" fill="white"
                            className="bi bi-cart" viewBox="0 0 16 16">
                            <path
                                d="M0 1.5A.5.5 0 0 1 .5 1H2a.5.5 0 0 1 .485.379L2.89 3H14.5a.5.5 0 0 1 
                                .491.592l-1.5 8A.5.5 0 0 1 13 12H4a.5.5 0 0 1-.491-.408L2.01 3.607 
                                1.61 2H.5a.5.5 0 0 1-.5-.5M3.102 4l1.313 7h8.17l1.313-7zM5 12a2 2 0 1 0 0 4 2 2 0 0 0 
                                0-4m7 0a2 2 0 1 0 0 4 2 2 0 0 0 0-4m-7 1a1 1 0 1 1 0 2 1 1 0 0 1 0-2m7 0a1 1 0 1 1 0 2 1 1 0 0 1 0-2" />
                        </svg>
                    </button>
                </div>
            </div>
        </nav >
    </>
}

export default NavBar;