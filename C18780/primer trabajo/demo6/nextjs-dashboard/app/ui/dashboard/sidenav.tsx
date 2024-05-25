import Link from 'next/link';
import NavLinks from '@/app/ui/dashboard/nav-links'; // componente que contiene los enlaces de navegación de la barra lateral
import Abacaxi from '@/app/ui/abacaxi-logo'; // muestra el logotipo de la aplicación
import "bootstrap/dist/css/bootstrap.min.css";
import 'bootstrap/dist/js/bootstrap.bundle.min.js';
import '../../ui/styles/nav.css';
import { useFetchCategoriesList } from '@/app/api/http.category';
import { Category } from '@/app/lib/data-definitions';
import { useState } from 'react';

const Categories = ({ categories, onAddCategory }: { categories: Category[], onAddCategory: any }) => {
  const [nameCategory, setNameCategory] = useState<string>("Category");
  return (
    <li className="nav-item dropdown">
      <div className="dropdown">
        <button className="btn btn-dark dropdown-toggle" type="button" data-bs-toggle="dropdown" aria-expanded="false">
          {nameCategory.localeCompare("Category") === 0 || nameCategory.localeCompare("All") === 0 ? "Category" :`Category: ${nameCategory}`}
        </button>
        <ul className="dropdown-menu dropdown-menu-dark">
          {categories.map((category, index) => (
            <li key={index}>
              <a className={`dropdown-item ${index === 0 ? "active" : ""}`} onClick={() => onAddCategory({ category }, setNameCategory(category.name))} href="#">{category.name}</a>
            </li>
          ))}
        </ul>
      </div>
    </li>
  );
}

const Search = ({ onAddSearch }: { onAddSearch: any }) => {
  const [newSearch, setNewSearch] = useState<string>();
  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setNewSearch(e.target.value);
  };

  const handleClick = () => {
    onAddSearch(newSearch);
  };

  return (
    <>
      <li className="nav-item">
        <a className="nav-link" href="#">
          <input className="form-control mr-sm-2" type="search" placeholder="Search" aria-label="Search" onChange={handleChange} />
        </a>
      </li>

      <li className="nav-item">
        <a className="nav-link" href="#">
          <button className="btn btn-outline-success my-2 my-sm-0" type="submit" onClick={handleClick}>
            <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor"
              className="bi bi-search" viewBox="0 0 16 16">
              <path
                d="M11.742 10.344a6.5 6.5 0 1 0-1.397 1.398h-.001q.044.06.098.115l3.85 3.85a1 1 0 0 0 1.415-1.414l-3.85-3.85a1 1 0 0 0-.115-.1zM12 6.5a5.5 5.5 0 1 1-11 0 5.5 5.5 0 0 1 11 0" />
            </svg>
          </button>
        </a>
      </li>
    </>
  );
}

export default function SideNav({ countCart = 0, onAddCategory, onAddSearch }: { countCart: number, onAddCategory: any, onAddSearch: any }) {
  const categories = useFetchCategoriesList();
  return (
    <nav className="navbar navbar-expand-lg navbar-dark bg-dark">
      <Link href="/dashboard">
        <div>
          <Abacaxi />
        </div>
      </Link>

      <button className="navbar-toggler" type="button" data-bs-toggle="collapse"
        data-bs-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false"
        aria-label="Toggle navigation">
        <span className="navbar-toggler-icon"></span>
      </button>

      <div className="collapse navbar-collapse" id="navbarSupportedContent">
        <ul className="navbar-nav mr-auto">
          <Search onAddSearch={onAddSearch} />

          <Categories categories={categories} onAddCategory={onAddCategory} />

          <li className="nav-item">
            <NavLinks countCart={countCart} />
          </li>
        </ul>
      </div>
    </nav >
  );
}
