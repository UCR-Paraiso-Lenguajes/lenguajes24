'use client'
import { useState } from "react";
import '../styles/search.css';
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faMagnifyingGlass } from "@fortawesome/free-solid-svg-icons";

export default function Search({ onAddSearch }: { onAddSearch: any }) {
    const [newSearch, setNewSearch] = useState<string>();
    const handleClick = () => {
        onAddSearch(newSearch);
    };

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setNewSearch(e.target.value);
    };
    return (
        <>
            <div className="container-input">
                <input type="text" placeholder="Search" name="text" className="input" onChange={handleChange} />
                <a className="MagnifyingGlass">
                    <FontAwesomeIcon icon={faMagnifyingGlass} className="fa-icon me-2" onClick={handleClick} />
                </a>
            </div>

        </>
    );
}