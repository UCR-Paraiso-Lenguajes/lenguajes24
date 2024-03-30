"use client";
import { useEffect, useState } from 'react';
import "@/public/styles.css";
import Link from 'next/link';

export default function Payment() {

    return (
        <div>
            <div className="header">
                <Link href="/">
                    <h1>Amazon</h1>
                </Link>
            </div>

            <div className="body">
                <h2>Payment Method</h2>
                
            </div>

            <div className="footer">
                <h2>Amazon.com</h2>
            </div>
        </div>
    );
}