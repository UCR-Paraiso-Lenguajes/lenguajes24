'use client'
import {useFetchInitialStore} from '@/app/api/http.initialStore';
import React, { useEffect } from 'react';
import { Chart } from "react-google-charts";
import { useState } from 'react';
import { useFetchCategoriesList } from '@/app/api/http.category';
import { useFetchCreateProduct } from '@/app/api/http.products';
import { Category, Product, ProductC } from '@/app/utils/data-definitions';
import '../../styles/managerProducts.css';

const options = {
    allowHtml: true,
    showRowNumber: true,
    legend: { position: "bottom" },
    pageSize: 4,
};

const Card = ({ onCreateProduct }: { onCreateProduct: any }) => {
    const [category, setCategory] = useState<Category>();
    const [img, setImg] = useState('');
    const [name, setName] = useState('');
    const [price, setPrice] = useState<number | null>(null); 
    const [description, setDescription] = useState('');
    const categoryList: Category[] = useFetchCategoriesList();
    const [product, setProduct] = useState<ProductC>();
    const [messages, setMessages] = useState<boolean>(false);
    useFetchCreateProduct(product);

    const handleSaveProduct = () => {
        if (name.trim() && img.trim() && price !== null && price > 0 && description.trim() && category?.uuid) {
            const newProduct: ProductC = {
                name: name,
                imageUrl: img,
                price: price,
                description: description,
                category: category.uuid
            };
            setProduct(newProduct);
            onCreateProduct(newProduct);
        } else {
            setMessages(true);
        }
    };

    return (
        <div className="col">
            <div className="card">
                <img
                    src={img || 'https://i.imgur.com/hm1XPhB.png'}
                    width={1000}
                    height={760}
                    className="card-img-top hidden md:block"
                    alt={name}
                />
                <div className="card-body">
                    <div className="btn-group">
                        <button className="btn btn-secondary btn-sm dropdown-toggle" type="button" data-bs-toggle="dropdown" aria-expanded="false">
                            {category ? category.name : 'Category'}
                        </button>
                        <ul className="dropdown-menu">
                            {categoryList.map((category) => (
                                <li key={category.uuid}>
                                    <a className="dropdown-item" href="#" onClick={() => setCategory(category)}>
                                        {category.name}
                                    </a>
                                </li>
                            ))}
                        </ul>
                    </div>

                    <input
                        placeholder="Name"
                        type="text"
                        className="input"
                        required
                        value={name}
                        onChange={(e) => setName(e.target.value)}
                    />
                    <input
                        placeholder="Price"
                        type="number"
                        className="input"
                        required
                        min={0}
                        value={price !== null ? price : ''}
                        onChange={(e) => {
                            const value = e.target.value !== '' ? Number(e.target.value) : null;
                            if (value !== null && value >= 0) {
                                setPrice(value);
                            } else {
                                setPrice(null);
                            }
                        }}
                    />
                    <input
                        placeholder="URL"
                        type="url"
                        className="input"
                        required
                        value={img}
                        onChange={(e) => setImg(e.target.value)}
                    />
                    <textarea
                        placeholder="Description"
                        className="input"
                        required
                        value={description}
                        onChange={(e) => setDescription(e.target.value)}
                    />
                    <div className="text-center my-4">
                        <a href="#" className="btn btn-warning" onClick={handleSaveProduct}>Create</a>
                        {messages && "fields required"}
                    </div>
                </div>
            </div>
        </div>
    );
};

export default function Products() {
    let initialCategory: string[] = ['All'];
    let initialSearch = 'none';
    const [data, setData] = useState<(string)[][]>([]);

    const [category, setCategory] = useState<string[]>(initialCategory);
    const [search, setSearch] = useState<string>(initialSearch);


    const products: Product[] = useFetchInitialStore({ category, search });

    const handleCreateProduct = (newProduct: ProductC) => {
        const newData = [...data, ["newProduct", newProduct.name, newProduct.description, `<img src='${newProduct.imageUrl}'/>`]];
        setData(newData);
    };

    useEffect(() => {
        const tableData = [["ID", "Name", "Description", "IMG"]];
        products.forEach(product => {
            tableData.push([product.uuid, product.name, product.description, "<img src='" + product.imageUrl + "'/>"]);
        });
        setData(tableData);
    }, [products])

    return (
        <>
            <div className='container-fluid'>
                <div className='row'>
                    <div className='col-lg-3 col-md-4 col-sm-12 mb-3'>
                        <Card onCreateProduct={handleCreateProduct} />
                    </div>
                    <div className='col-lg-9 col-md-8 col-sm-12'>
                        <Chart
                            className='chart'
                            chartType="Table"
                            data={data}
                            options={options}
                            width="100%"
                            height="auto"
                        />
                    </div>
                </div>
            </div>
        </>
    );
}
