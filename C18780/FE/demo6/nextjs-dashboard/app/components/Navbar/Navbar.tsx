'use client'
import Link from 'next/link';
import '../../styles/Navbar.css';
import Abacaxi from './../../../public/logo/abacaxi-logo';
import { useFetchCategoriesList } from '@/app/api/http.category';
import { Category } from '@/app/utils/data-definitions';
import { useEffect, useState } from 'react';
import { useFecthGetLatestAds, useSignalRGetAds } from '@/app/api/http.ad';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faBell, faCartShopping } from '@fortawesome/free-solid-svg-icons';
import Search from '../Search';

export default function Navbar({ onAddCategory, onAddSearch, countCart }: { onAddCategory: any, onAddSearch: any, countCart: number }) {
    const categories: Category[] = useFetchCategoriesList();
    const [nameCategory, setNameCategory] = useState<string>("Category");

    let notification = useSignalRGetAds().message;
    let messages = useFecthGetLatestAds(3).message;
    messages = [...messages, ...notification];


    const [newSearch, setNewSearch] = useState<string>();

    useEffect(() => {
        onAddSearch(newSearch);
    }, [newSearch]);

    return (
        <>
            <nav className="navbar navbar-expand-lg navbar-dark bg-dark">
                <div className="container-fluid">
                    <a className="navbar-brand" href="#">
                        <Abacaxi />
                    </a>
                    <button className="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                        <span className="navbar-toggler-icon"></span>
                    </button>
                    <div className="collapse navbar-collapse" id="navbarSupportedContent">
                        <ul className="navbar-nav me-auto mb-2 mb-lg-0">
                            {/*Category*/}
                            <li className="nav-item dropdown">
                                <div className="dropdown">
                                    <a className="btnNabvar " type="button" data-bs-toggle="dropdown" aria-expanded="false">
                                        {nameCategory.localeCompare("Category") === 0 || nameCategory.localeCompare("All") === 0 ? "Category" : `Category: ${nameCategory}`}
                                    </a>
                                    <ul className="dropdown-menu dropdown-menu-dark">
                                        {categories.map((category, index) => (
                                            <li key={index}>
                                                <a className={`dropdown-item`} onClick={() => onAddCategory({ category }, setNameCategory(category.name))} href="#">{category.name}</a>
                                            </li>
                                        ))}
                                    </ul>
                                </div>
                            </li>
                            {/*Cart*/}
                            <li className="nav-item">
                                <Link
                                    href={'/cart'}
                                >
                                    <div className="btnNabvar number"><FontAwesomeIcon icon={faCartShopping} className="fa-icon me-2" />{countCart}</div>
                                </Link >
                            </li>
                            {/*Notification*/}
                            <li className="nav-item dropdown">
                                <div className="dropdown">
                                    <a className="btnNabvar" type="button" data-bs-toggle="dropdown" aria-expanded="false">
                                        <div className="number"><FontAwesomeIcon icon={faBell} className="fa-icon me-2" />{messages.length}</div>
                                    </a>
                                    <ul className="dropdown-menu dropdown-menu-dark" style={{ pointerEvents: 'none' }}>
                                        {messages.length ? messages.map((notification, index) => (
                                            <li key={index} style={{ display: 'flex', alignItems: 'center' }}>

                                                <div className='container'>
                                                    <div className='row'>
                                                        <span>{new Date(notification.date).toLocaleDateString('es-Es', {
                                                            weekday: 'long',
                                                            year: 'numeric',
                                                            month: 'long',
                                                            day: 'numeric'
                                                        })}
                                                        </span>
                                                    </div>
                                                    <div className='row'>
                                                        <a className={`dropdown-item`} href="#">
                                                            <div className="specifications-content" dangerouslySetInnerHTML={{ __html: notification.message }} />
                                                        </a>
                                                    </div>
                                                </div>
                                            </li>
                                        )) : <img
                                            src="https://i.imgur.com/bUJ3Z0Y.png"
                                            style={{
                                                width: '100px',
                                                height: '100px'
                                            }}
                                        />}
                                    </ul>
                                </div >
                            </li >
                            {/*Search*/}
                            <form className="d-flex">
                                <Search onAddSearch={setNewSearch} />
                            </form>
                        </ul>
                    </div>
                </div>
            </nav>
        </>
    );
}