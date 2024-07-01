"use client";
import React, { useState, ChangeEvent, FormEvent, useEffect } from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import Sidebar from "../init/page";
import '@/app/ui/Styles/products.css';
import { decodeToken, checkTokenDate } from '../../hooks/jwtHooks';
import { useRouter } from 'next/navigation';

const URLConection = process.env.NEXT_PUBLIC_API;

interface Product {
  id: number;
  name: string;
  imageUrl: string;
  price: number | '';
  quantity: number;
  categoriaId: number;
  categoriaNombre: string;
  description: string;
}

interface Categoria {
  id: number;
  nombre: string;
}

const Products: React.FC = () => {
  const [form, setForm] = useState<Product>({ id: 0, name: '', imageUrl: '', price: '', quantity: 1, categoriaId: 0, categoriaNombre: '', description: '' });
  const [showModal, setShowModal] = useState(false);
  const [errors, setErrors] = useState<{ name?: string, imageUrl?: string, price?: string, categoriaId?: string, description?: string }>({});
  const [categories, setCategories] = useState<Categoria[]>([]);
  const [productList, setProductList] = useState<Product[]>([]);
  const router = useRouter();

  const checkToken = () => {
    const token = sessionStorage.getItem("sessionToken");
    if (token) {
      const decodedToken = decodeToken(token);
      const isTokenAlive = checkTokenDate(decodedToken?.exp);
      if (!isTokenAlive) {
        sessionStorage.removeItem("sessionToken");
        sessionStorage.removeItem("expiracyToken");
        router.push("/admin");
        return false;
      }
      return true;
    } else {
      router.push("/admin");
      return false;
    }
  };

  useEffect(() => {
    if (checkToken()) {
      fetchStoreData();
    }
  }, [router]);

  const fetchStoreData = async () => {
    if (!checkToken()) return;

    try {
      const response = await fetch(`${URLConection}/admin/productos`, {
        headers: {
          'Authorization': `Bearer ${sessionStorage.getItem('sessionToken')}`
        }
      });
      if (response.ok) {
        const data = await response.json();
        setCategories(data.categorias || []);
        setProductList(data.products || []);
      } else {
        throw new Error('Failed to fetch data');
      }
    } catch (error) {
      throw new Error(`Error fetching store data: ${error.message}`);
    }
  };

  const validateForm = () => {
    const newErrors: { name?: string, imageUrl?: string, price?: string, categoriaId?: string, description?: string } = {};
    if (!form.name.trim()) newErrors.name = 'El nombre es requerido y no puede estar vacío o ser solo espacios';
    if (!form.imageUrl.trim() || !/^https?:\/\/.*\.(jpeg|jpg|gif|png)$/.test(form.imageUrl)) newErrors.imageUrl = 'La URL de la imagen es requerida y debe ser válida';
    if (typeof form.price !== 'number' || form.price <= 0 || form.price > 99999999) newErrors.price = 'El precio debe ser mayor a 0 y menor a 99,999,999';
    if (!form.categoriaNombre.trim()) newErrors.categoriaId = 'La categoría es requerida y no puede estar vacía o ser solo espacios';
    if (!form.description.trim()) newErrors.description = 'La descripción es requerida y no puede estar vacía o ser solo espacios';
    return newErrors;
  };

  const handleInputChange = (e: ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    if (name === 'categoria') {
      const selectedCategory = categories.find(categoria => categoria.nombre === value);
      setForm({ ...form, categoriaId: selectedCategory ? selectedCategory.id : 0, categoriaNombre: selectedCategory ? selectedCategory.nombre : '' });
    } else {
      setForm({ ...form, [name]: name === 'price' ? (value === '' ? '' : Number(value)) : value });
    }
  };

  const handleSubmit = async (e: FormEvent) => {
    e.preventDefault();
    if (!checkToken()) return;

    const formErrors = validateForm();
    if (Object.keys(formErrors).length > 0) {
      setErrors(formErrors);
      return;
    }

    // Verificar si el producto ya existe
    const existingProduct = productList.find(product => product.name.toLowerCase() === form.name.toLowerCase());
    if (existingProduct) {
      setErrors({ name: 'El producto ya existe. No se puede insertar un producto duplicado.' });
      return;
    }

    try {
      const response = await fetch(`${URLConection}/admin/producto`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${sessionStorage.getItem('sessionToken')}`
        },
        body: JSON.stringify(form),
      });

      if (response.ok) {
        setForm({ id: 0, name: '', imageUrl: '', price: '', quantity: 1, categoriaId: 0, categoriaNombre: '', description: '' });
        setErrors({});
        setShowModal(false);
        fetchStoreData();
      } else {
        const errorResponseText = await response.text();
        throw new Error(`Error en el servidor: ${errorResponseText}`);
      }
    } catch (error) {
      throw new Error(`Error submitting form: ${error.message}`);
    }
  };

  const handleDelete = async (productId: number) => {
    if (!checkToken()) return;

    if (!window.confirm('¿Estás seguro de que deseas eliminar este producto?')) {
      return;
    }
    try {
      const response = await fetch(`${URLConection}/admin/producto/${productId}`, {
        method: 'DELETE',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${sessionStorage.getItem('sessionToken')}`
        },
      });

      if (response.ok) {
        fetchStoreData();
      } else {
        const errorResponseText = await response.text();
        throw new Error(`Error en el servidor: ${errorResponseText}`);
      }
    } catch (error) {
      throw new Error(`Error deleting product: ${error.message}`);
    }
  };

  return (
    <div className="d-flex">
      <div className="sidebar">
        <Sidebar />
      </div>
      <div className="container">
        <h2 className="my-4 text-center">Productos</h2>
        <button className="btn btn-success mb-4" onClick={() => setShowModal(true)}>Nuevo Producto</button>

        {showModal && (
          <div className="modal" style={{ display: 'block' }}>
            <div className="modal-dialog" role="document">
              <div className="modal-content">
                <div className="modal-header">
                  <h5 className="modal-title">Nuevo Producto</h5>
                  <button type="button" className="close" onClick={() => setShowModal(false)}>
                    <span>&times;</span>
                  </button>
                </div>
                <div className="modal-body">
                  <form onSubmit={handleSubmit}>
                    <div className="mb-3">
                      <label htmlFor="name" className="form-label">Nombre</label>
                      <input
                        type="text"
                        name="name"
                        value={form.name}
                        onChange={handleInputChange}
                        className={`form-control ${errors.name ? 'is-invalid' : ''}`}
                        placeholder="Nombre"
                        required
                      />
                      {errors.name && <div className="invalid-feedback">{errors.name}</div>}
                    </div>
                    <div className="mb-3">
                      <label htmlFor="imageUrl" className="form-label">Imagen (URL)</label>
                      <input
                        type="text"
                        name="imageUrl"
                        value={form.imageUrl}
                        onChange={handleInputChange}
                        className={`form-control ${errors.imageUrl ? 'is-invalid' : ''}`}
                        placeholder="URL de la imagen"
                        required
                      />
                      {errors.imageUrl && <div className="invalid-feedback">{errors.imageUrl}</div>}
                    </div>
                    <div className="mb-3">
                      <label htmlFor="price" className="form-label">Precio</label>
                      <input
                        type="number"
                        name="price"
                        value={form.price}
                        onChange={handleInputChange}
                        className={`form-control ${errors.price ? 'is-invalid' : ''}`}
                        placeholder="Precio"
                        required
                        min="1"
                        max="99999999"
                      />
                      {errors.price && <div className="invalid-feedback">{errors.price}</div>}
                    </div>
                    <div className="mb-3">
                      <label htmlFor="categoria" className="form-label">Categoría</label>
                      <select
                        name="categoria"
                        value={form.categoriaNombre}
                        onChange={handleInputChange}
                        className={`form-control ${errors.categoriaId ? 'is-invalid' : ''}`}
                        required
                      >
                        <option value="">Seleccione una categoría</option>
                        {categories.map(categoria => (
                          <option key={categoria.id} value={categoria.nombre}>{categoria.nombre}</option>
                        ))}
                      </select>
                      {errors.categoriaId && <div className="invalid-feedback">{errors.categoriaId}</div>}
                    </div>
                    <div className="mb-3">
                      <label htmlFor="description" className="form-label">Descripción</label>
                      <textarea
                        name="description"
                        value={form.description}
                        onChange={handleInputChange}
                        className={`form-control ${errors.description ? 'is-invalid' : ''}`}
                        placeholder="Descripción"
                        rows={4}
                        required
                      ></textarea>
                      {errors.description && <div className="invalid-feedback">{errors.description}</div>}
                    </div>
                    <button type="submit" className="btn btn-primary">Guardar</button>
                  </form>
                </div>
                <div className="modal-footer">
                  <button type="button" className="btn btn-secondary" onClick={() => setShowModal(false)}>Cerrar</button>
                </div>
              </div>
            </div>
          </div>
        )}

        <table className="table table-striped">
          <thead>
            <tr>
              <th>Nombre</th>
              <th>Imágenes</th>
              <th>Precio</th>
              <th>Categoría</th>
              <th>Acciones</th>
            </tr>
          </thead>
          <tbody>
            {productList.map((product) => (
              <tr key={product.id}>
                <td>{product.name}</td>
                <td><img src={product.imageUrl} alt={product.name} style={{ width: '50px' }} /></td>
                <td>${product.price}</td>
                <td>{product.categoriaNombre || 'Sin Categoría'}</td>
                <td>
                  <button className="btn btn-danger" onClick={() => handleDelete(product.id)}>Eliminar</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
};

export default Products;