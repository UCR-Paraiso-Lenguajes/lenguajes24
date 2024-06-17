using System.Data.Common;

namespace Core;

    public struct  Categoria
    {
    public int Id {get;}
    public string Nombre { get; private set; }

    public Categoria (int id, string nombre){
        if (id == null || id < 1)
            throw new ArgumentException("Una categoría debe tener un ID");
        if(string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException($"Se necesita un {nameof(nombre)}, ya que esta nulo");
        
        Id = id;
        Nombre = nombre;
        }
    }




