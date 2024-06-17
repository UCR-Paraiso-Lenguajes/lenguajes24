import React from 'react';
import {useState} from 'react';
import {useEffect} from 'react';
import Button from 'react-bootstrap/Button';
import Modal from 'react-bootstrap/Modal';

//Componentes
//import { BrowserRouter as Router, Route, Link } from 'react-router-dom';
import Link from 'next/link';

//Recursos
import 'bootstrap/dist/css/bootstrap.min.css';
import '../../src/css/modal_insert.css'
import '../../src/css/fonts_awesome/css/all.min.css'
import { mock } from 'node:test';
import { ProductAPI } from '@/app/src/models-data/ProductAPI';
import { CategoryAPI } from '@/app/src/models-data/CategoryAPI';



//Creamos la interfaz que deben seguir los props (o parametros) para el componente Modal
interface ModalInsertProps {
    show: boolean;
    handleClose: () => void;
    categoryListFromStore: CategoryAPI[];
}
  
export const ModalInsert: React.FC<ModalInsertProps> = ({ 
    show,
    handleClose,
    categoryListFromStore
}) => {
    
    const deleteNewProductInfo = () => {
        setProductName('');
        setDescription('');
        setSelectedCategory('');
        setImages([]);
        setError('');
    }
    const [productName, setProductName] = useState('');
    const [description, setDescription] = useState('');
    const [selectedCategory, setSelectedCategory] = useState('');
    const [images, setImages] = useState<File[]>([]);
    const [error, setError] = useState('');

  const isValidInput = (input: string): boolean => {
    return input.trim() !== '';
  };

  const insertNewProduct = () => {
    if (!isValidInput(productName)) {
      setError('Por favor ingrese un nombre de producto válido');
      return;
    }
    if (!isValidInput(description)) {
      setError('Por favor ingrese una descripción válida');
      return;
    }
    if (selectedCategory === '') {
      setError('Por favor seleccione una categoría');
      return;
    }

    var defaultImage = "./img/not_found_img.jpg";
    
    
    console.log(productName);
    console.log(description);
    console.log(selectedCategory);
    console.log(images);
    deleteNewProductInfo();
    handleClose();
  };

  const handleImageChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const files = e.target.files;
    if (files) {
        //convertimos la lista de archivos (img) en un array, para evitar el error de tipo de lista
        setImages(Array.from(files));
    }
  };

  return (
    <>
        <Modal show={show} onHide={handleClose} animation={false}>
            <Modal.Header closeButton>
            <Modal.Title>
                <div className="modal_title_btn">
                <h4>
                    <i className="fas fa-shopping-cart"></i>Insertar Nuevo Producto:
                </h4>
                </div>
            </Modal.Title>
            </Modal.Header>
            <Modal.Body>
                {error && <div className="alert alert-danger">{error}</div>}
                <form>
                    <label>
                        Nombre del producto:
                        <input
                            type="text"
                            value={productName}
                            onChange={(e) => setProductName(e.target.value)}
                        />
                    </label>

                    <label>
                        Descripción:
                        <textarea
                            value={description}
                            onChange={(e) => setDescription(e.target.value)}
                        />

                    </label>
                    <label>
                        Tipo de Categoría:
                        <select
                            value={selectedCategory}
                            onChange={(e) => setSelectedCategory(e.target.value)}
                        >
                            {categoryListFromStore.map((category) => (
                            <option key={category.id} value={category.id.toString()}>
                                {category.name}
                            </option>
                            ))}
                        </select>
                    </label>
                    <label>
                        Imágenes:
                        <input type="file" multiple onChange={handleImageChange} />
                    </label>
                </form>
            </Modal.Body>
            <Modal.Footer>
            
            <Button variant="primary" onClick={deleteNewProductInfo}>
                Borrar Datos
            </Button>

            <Button variant="secondary" onClick={handleClose}>
                Cerrar
            </Button>
            <Button variant="primary" onClick={insertNewProduct}>
                Insertar
            </Button>
            </Modal.Footer>
        </Modal>
    </>
  );
};