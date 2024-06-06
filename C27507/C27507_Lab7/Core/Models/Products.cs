using MyStoreAPI.Models;
namespace Core;

public class Products{
    
    public IEnumerable<Product> productsFromStore {get;}
    public IEnumerable<Category> categoriesFromStore {get;}
    public Dictionary<int, List<Product>> dictionaryOfProducts {get;}

    public Products(){
        //Obtenemos los datos que estan en memoria en Store.Instance        
        productsFromStore = Store.Instance.Products;
        categoriesFromStore = Categories.Instance.AllProductCategories;

        if(productsFromStore == null || productsFromStore.Count() == 0)
            throw new ArgumentException($"{nameof(productsFromStore)} no es válido" );
        if(categoriesFromStore == null || categoriesFromStore.Count() == 0)
            throw new ArgumentException($"{nameof(categoriesFromStore)} no es válido" );

        dictionaryOfProducts = new Dictionary<int, List<Product>>();
        //Llenamos el diccionario
        fillDictionaryOfProducts();
    }

    private void fillDictionaryOfProducts(){

        //Creamos una lista de productos apra cada categoria
        foreach (var category in categoriesFromStore){            
            dictionaryOfProducts[category.id] = new List<Product>();
        }
        //Agregamos el producto recorrido actual a una de la listas
        foreach (var product in productsFromStore){            
            dictionaryOfProducts[product.category.id].Add(product);
        }
    }
}
