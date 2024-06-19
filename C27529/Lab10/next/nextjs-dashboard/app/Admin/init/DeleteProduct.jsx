import React, { useEffect, useState } from 'react';
import DataTable from 'react-data-table-component';
import 'bootstrap/dist/css/bootstrap.min.css';

function DeleteProduct() {
  const [productList, setProductList] = useState([]);

  useEffect(() => {
    const loadData = async () => {
      try {
        const response = await fetch('https://localhost:7280/api/store');
        if (!response.ok) {
          throw new Error('Failed to fetch data');
        }
        const json = await response.json();
        if (!json) throw new Error('Failed to fetch data, null response');
        
        setProductList(json.products); 
        localStorage.setItem('productList', JSON.stringify(json.products));
      } catch (error) {
        throw new Error('Error loading data:', error);
      }
    };

    loadData();

    const storedProductList = localStorage.getItem('productList');
    if (storedProductList) {
      setProductList(JSON.parse(storedProductList));
    }
  }, []);

  const columns = [
    {
      name: 'ID',
      selector: row => row.id,
      sortable: true,
    },
    {
      name: 'Nombre',
      selector: row => row.name,
      sortable: true,
    },
    {
      name: 'Descripción',
      selector: row => row.description,
    },
    {
      name: 'Imagen',
      cell: row => <img src={row.imageURL} alt={row.name} width="50" />,
    },
  ];

  return (
    <div className="product-list-container">
      <h2>Delete Product</h2>
      {productList.length > 0 ? (
        <DataTable
          columns={columns}
          data={productList}
          pagination
        />
      ) : (
        <div className="d-flex justify-content-center">
          <div className="spinner-border text-primary" role="status">
            <span className="visually-hidden">Cargando...</span>
          </div>
        </div>
      )}
    </div>
  );
}

export default DeleteProduct;
