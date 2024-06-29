'use client'
import React, { useState } from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faChartLine, faCartFlatbed, faPowerOff, faHome, faBars, faMessage } from '@fortawesome/free-solid-svg-icons';
import Link from 'next/link';
import { deleteCookie } from "cookies-next";
import { useRouter } from 'next/navigation';

const SideNav = () => {
    const [isMenuOpen, setIsMenuOpen] = useState(true);
    const toggleMenu = () => {
        setIsMenuOpen(!isMenuOpen);
    };

    return (
        <div className={`sidebar ${isMenuOpen ? 'open' : ''}`}>
            <ul>
                <li onClick={toggleMenu} className="nav">
                    <FontAwesomeIcon icon={faBars} className="fa-icon me-2" />
                    <span className={`text-hidden ${isMenuOpen ? '' : 'hidden'}`}>
                        Abacaxi
                    </span>
                </li>
            </ul>
            <ul className="list-unstyled">
                <Link href="/admin/init">
                    <li className="nav">
                        <FontAwesomeIcon icon={faHome} className="fa-icon me-2" />
                        <span className={`text-hidden ${isMenuOpen ? '' : 'hidden'}`}>
                            Home
                        </span>
                    </li>
                </Link>
                <Link href="/admin/init/ad">
                    <li className="nav">
                        <FontAwesomeIcon icon={faMessage} className="fa-icon me-2" />
                        <span className={`text-hidden ${isMenuOpen ? '' : 'hidden'}`}>
                            ad
                        </span>
                    </li>
                </Link>
                <Link href="/admin/init/products">
                    <li className="nav">
                        <FontAwesomeIcon icon={faCartFlatbed} className="fa-icon me-2" />
                        <span className={`text-hidden ${isMenuOpen ? '' : 'hidden'}`}>
                            Product
                        </span>
                    </li>
                </Link>
                <Link href="/admin/init/reports">
                    <li className="nav">
                        <FontAwesomeIcon icon={faChartLine} className="fa-icon me-2" />
                        <span className={`text-hidden ${isMenuOpen ? '' : 'hidden'}`}>
                            Reports
                        </span>
                    </li>
                </Link>
            </ul>
            <ul className='close'>
                <li className="nav">
                    <Link href={'/login'}>
                        <FontAwesomeIcon icon={faPowerOff} className="fa-icon me-2" />
                        <span className={`text-hidden ${isMenuOpen ? '' : 'hidden'}`} onClick={() => { deleteCookie('token') }}>
                            Logout
                        </span>
                    </Link>
                </li>
            </ul>
        </div>
    );
}

export default SideNav;
