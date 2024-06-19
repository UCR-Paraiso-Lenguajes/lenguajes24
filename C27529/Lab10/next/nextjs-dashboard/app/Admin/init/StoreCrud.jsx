import React, { useState } from 'react';
import AddProduct from './AddProduct';
import DeleteProduct from './DeleteProduct';
import 'bootstrap/dist/css/bootstrap.min.css';

function StoreCrud() {
  const [currentView, setCurrentView] = useState(null);

  const renderCurrentView = () => {
    switch (currentView) {
      case 'add':
        return <AddProduct />;
      case 'delete':
        return <DeleteProduct />;
      default:
        return <div>Seleccione una opción</div>;
    }
  };

  return (
    <div className="store-crud-container">
      <div className="button-group">
        <button className="btn btn-primary" onClick={() => setCurrentView('add')}>
          Agregar Producto
        </button>
        <button className="btn btn-danger" onClick={() => setCurrentView('delete')}>
          Eliminar Productos
        </button>
      </div>
      {renderCurrentView()}
    </div>
  );
}

export default StoreCrud;
