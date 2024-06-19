'use client'
import useFetchInitialStore from '@/app/api/http.initialStore';
import { Category, Product, ProductC } from '@/app/lib/data-definitions';
import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/js/bootstrap.bundle.min.js';
import React, { useEffect } from 'react';
import { Chart } from "react-google-charts";
import '../../../ui/styles/managerProducts.css';
import { useState } from 'react';
import { useFetchCategoriesList } from '@/app/api/http.category';
import { useFetchCreateProduct } from '@/app/api/http.products';

const options = {
    allowHtml: true,
    showRowNumber: true,
    legend: { position: "bottom" },
    pageSize: 4,
};

const Card = () => {
    const [category, setCategory] = useState<Category>();
    const [img, setImg] = useState('');
    const [name, setName] = useState('');
    const [price, setPrice] = useState(0);
    const [description, setDescription] = useState('');
    const categoryList: Category[] = useFetchCategoriesList();
    const [product, setProduct] = useState<ProductC>();
    useFetchCreateProduct(product);

    const handleSaveProduct = () => {
        if (name.trim() && img.trim() && price > 0 && description.trim() && category?.uuid) {
            setProduct({
                name: name,
                imageUrl: img,
                price: price,
                description,
                category: category.uuid
            });
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
                        required={true}
                        value={name}
                        onChange={(e) => setName(e.target.value)}
                    />
                    <input
                        placeholder="Price"
                        type="number"
                        className="input"
                        required={true}
                        value={price}
                        onChange={(e) => setPrice(Number(e.target.value))}
                    />
                    <input
                        placeholder="URL"
                        type="url"
                        className="input"
                        required={true}
                        value={img}
                        onChange={(e) => setImg(e.target.value)}
                    />
                    <textarea
                        placeholder="Description"
                        className="input"
                        required={true}
                        value={description}
                        maxLength={250}
                        onChange={(e) => setDescription(e.target.value)}
                    />
                    <div className="text-center my-4">
                        <a href="#" className="btn btn-warning" onClick={handleSaveProduct}>Create</a>
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
                        <Card />
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
