using System;
using System.Collections.Generic;
using System.Linq;
namespace storeApi.Models.Data;
public class BinarySearch
{
    public Product Product { get; private set; }
    public BinarySearch izq { get; set; }
    public BinarySearch der { get; set; }

    public BinarySearch(Product product)
    {
        if (product == null) throw new ArgumentNullException($"El producto {nameof(product)} no puede ser nulo.");
        if (string.IsNullOrEmpty(product.name)) throw new ArgumentException("El nombre del producto no puede estar vacío.", nameof(product));
        Product = product;
    }
}
public class BinarySearchTree
{
    private BinarySearch raiz;
    public void InsertProduct(Product product)
    {
        if (product == null) throw new ArgumentNullException($"El producto {nameof(product)} no puede ser nulo.");
        if (string.IsNullOrEmpty(product.name)) throw new ArgumentException($"El nombre del producto {nameof(product)} no puede estar vacío.");
        product.name = product.name.ToLower();
        raiz = InsertProductTree(raiz, product);
    }

    private BinarySearch InsertProductTree(BinarySearch cabeza, Product product)
    {
        if (cabeza == null) return new BinarySearch(product);
        int comparationString = string.Compare(product.name, cabeza.Product.name);
        if (comparationString < 0) cabeza.izq = InsertProductTree(cabeza.izq, product);
        else if (comparationString > 0) cabeza.der = InsertProductTree(cabeza.der, product);
        return cabeza;
    }

    public IEnumerable<Product> Search(string textToSearch)
    {
        if (string.IsNullOrEmpty(textToSearch)) throw new ArgumentException($"El texto de búsqueda {nameof(textToSearch)} no puede estar vacío.");
        List<Product> productsFound = new List<Product>();
        Search(productsFound,textToSearch.ToLower(),raiz);
        return productsFound;
    }
    private void Search(List<Product> chargedProducs,string textToSearch,BinarySearch cabeza)
    {
        if (cabeza == null) { return; }
        if (chargedProducs == null ) throw new ArgumentNullException($"La lista de productos {nameof(chargedProducs)}, cargados no puede ser nula.");
        if (string.IsNullOrEmpty(textToSearch)) throw new ArgumentException($"El texto de búsqueda {nameof(textToSearch)} no puede estar vacío.");
        var tieneNombre=!string.IsNullOrEmpty(cabeza.Product.name);
        var nombreContieneTexto= tieneNombre &&  cabeza.Product.name.ToLower().Contains(textToSearch);
        if (nombreContieneTexto) { chargedProducs.Add(cabeza.Product); }
        var tieneDescripcion = !string.IsNullOrEmpty(cabeza.Product.description);
        var descripcionContieneTexto = tieneDescripcion && cabeza.Product.description.ToLower().Contains(textToSearch);
        if (descripcionContieneTexto) { chargedProducs.Add(cabeza.Product); }
        Search(chargedProducs,textToSearch,cabeza.izq);
        Search(chargedProducs,textToSearch,cabeza.der);
    }
}