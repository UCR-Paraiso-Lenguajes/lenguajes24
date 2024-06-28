using System.Collections;
using System.Reflection.PortableExecutable;
using System.Security.Principal;
using TodoApi.models;

namespace Core;

public class Productos
{
    private List<Product> productos;
    private Dictionary<int, List<Product>> productosPorCategoria;

    public Productos()
    {
      /*  productos = new List<Product>();
        productos.Add(new Product("Producto 1", "imagen1.jpg", 29.99m, 1));
        productos.Add(new Product("Producto 2", "imagen2.jpg", 19.99m, 2));
        productos.Add(new Product("Producto 3", "imagen3.jpg", 39.99m, 1));
        productos.Add(new Product("Producto 4", "imagen4.jpg", 49.99m, 3));
        productos.Add(new Product("Producto 5", "imagen5.jpg", 9.99m, 2));
        productos.Add(new Product("Producto 6", "imagen6.jpg", 59.99m, 1));
        productos.Add(new Product("Producto 7", "imagen7.jpg", 14.99m, 3));
        productos.Add(new Product("Producto 8", "imagen8.jpg", 24.99m, 2));
        productos.Add(new Product("Producto 9", "imagen9.jpg", 34.99m, 1));
        productos.Add(new Producto("Producto 10", "imagen10.jpg", 64.99m, 3));

        foreach (var producto in productos)
        {
            if (!productosPorCategoria.ContainsKey(producto.CategoriaId))
            {
                productosPorCategoria[producto.CategoriaId] = new List<Producto>();
            }
            productosPorCategoria[producto.CategoriaId].Add(producto);
        }
*/
    }


    internal void Add(Product p)
    {
        if(p == null) throw new ArgumentException($"{nameof(p)} is null.");

        productos.Add(p);

       /* List<Product> temporal = new List<Product> ();
        if( ! productosPorCategoria.TryGetValue(p.CategoriaId, out temporal))
        {
            temporal = new List<Product>();
            productosPorCategoria[p.CategoriaId].Add(temporal);
        }
        temporal.Add(p);

        token= Tokenizer(p)// xbox - maquina de video juegos para personas
        //[xbox, maquina, de , video, juegos, para, persons] //token prepaosion, adverbio, adjetivo

        reduccion(token, prep)
        [xbox, maquina, video, juegos, persons]
        Dicc*/
        
        //productosPorCategoria[p.CategoriaId].Add(p);
    }

    public IEnumerable searchByCategoria(Categoria categoria )
    {
        return productosPorCategoria[categoria.Id];
    }
}

public class Categoria
{
    public int Id { get; internal set; }
}