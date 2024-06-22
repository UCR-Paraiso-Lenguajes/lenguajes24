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
import { insertNewProductInDBAsync } from '@/app/src/api/get-post-api';
import { AlertShop } from '@/app/global-components/generic_overlay';



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
    
    //Atributos del producto nuevo
    const [productName, setProductName] = useState('');
    const [productPrice, setProductPrice] = useState('');
    const [description, setDescription] = useState('');
    const [selectedCategory, setSelectedCategory] = useState('');    
    const [defaultImage, setDefaultImage] = useState<string>("");

    const [error, setError] = useState('');

    //gestionamiento para los alert de boostrap
    const [showAlert, setShowAlert] = useState(false);
    const [alertInfo,setAlertInfo] = useState("");
    const [alertTitle,setAlertTitle] = useState("");
    const [alertType,setAlertType] = useState("");
    
    function closeAlertShop(): void {
        setShowAlert(false);     
    }
            
    const deleteNewProductInfo = () => {
        setProductName('');
        setProductPrice('')
        setDescription('');
        setSelectedCategory('');
        setDefaultImage('');
        setError('');
    }
   
    const getImageURL = (e: React.ChangeEvent<HTMLInputElement>) => {
        const url = e.target.value;
        setDefaultImage(url);
    };


  const isValidInput = (input: string): boolean => {
    return input.trim() !== '';
  };

  const insertNewProduct = async () => {
    if (!isValidInput(productName)) {
      setError('Por favor ingrese un nombre de producto válido');
      return;
    }

    // const isValidURL = (url: string): boolean => {
    //     try {
    //         new URL(url);
    //         return true;
    //     } catch (_) {
    //         return false;
    //     }
    // };



    let validProductPrice = parseInt(productPrice);
    if ( isNaN(validProductPrice) || validProductPrice <= 0 || validProductPrice >= 10000000) {
        setError('Por favor ingrese un precio de producto válido');
        return;
    }

    if (!isValidInput(description)) {
      setError('Por favor ingrese una descripción válida');
      return;
    }
    if (selectedCategory === ''|| !selectedCategory) {
      setError('Por favor seleccione una categoría');
      return;
    }
    
    if (!defaultImage || defaultImage === '') {
        setError('Por favor ingrese una URL adecuada');
        return;
    }
            
    const newProduct : ProductAPI = {

            id: 0,
            name: productName,
            imageUrl: defaultImage,
            price: validProductPrice,
            quantity: 0,
            description: description,
            category: categoryListFromStore.find(category => category.id === parseInt(selectedCategory)) || undefined
    }
   
    deleteNewProductInfo();
        
    try{        
        let dataFromStore = await insertNewProductInDBAsync(newProduct);

        if (typeof dataFromStore  === "boolean" && dataFromStore !== null){
            callAlertShop("success","Inserción Exitosa","El producto " + newProduct.name  + "fue agregado con éxito a la base de datos");
        
        }else{
            callAlertShop("danger","Inserción Fallida","El producto " + newProduct.name  + "no pudo ser agregado")
        
        }
    } catch (error) {                
        throw new Error('Failed to fetch data:' + error);
    }

    //Cerramos el modal y borramos todo
    handleClose();
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
                        Precio del producto:
                        <input
                            type="number"
                            value={productPrice}
                            onChange={(e) => setProductPrice(e.target.value)}
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
                        URL de la imagen:
                        <input type="url" 
                                placeholder="Ingresa la URL de la imagen" 
                                value={defaultImage}
                                onChange={getImageURL} />
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

        <AlertShop alertTitle={alertTitle} alertInfo={alertInfo} alertType={alertType} showAlert={showAlert} onClose={closeAlertShop}/>
    </>
  );
};

function callAlertShop(arg0: string, arg1: string, arg2: string) {
    throw new Error('Function not implemented.');
}
