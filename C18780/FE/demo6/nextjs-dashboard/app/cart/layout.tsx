'use client'
import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/js/bootstrap.bundle.min.js';
import './../styles/cardPage.css';
export default function Layout({
    children,
}: {
    children: React.ReactNode;
}) {
    return (
        <>
            <main>{children}</main>
        </>
    );
}