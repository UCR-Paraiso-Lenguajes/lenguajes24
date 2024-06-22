using MyStoreAPI.Models;
namespace Core;

public class ProductsLogic{
        
    public Products dataFromStore {get;}
    
    public ProductsLogic(){
        this.dataFromStore = new Products();
        
        if(dataFromStore == null) 
            throw new BussinessException($"{nameof(dataFromStore)} no puede ser nulo");        
    }

    public IEnumerable<Product> filterProductsByCategory(int categoryId){        

        if (categoryId < 0 )
            throw new BussinessException($"{nameof(categoryId)} id de categoría no válido");
        //verificar que la categoria exista en el struct 
        var thisCategoryExist = dataFromStore.categoriesFromStore.Any(c => c.id == categoryId);
        if (!thisCategoryExist)
            throw new BussinessException($"{nameof(categoryId)} no existe");


        //Si se permiten listas vacias
        if(categoryId == 0){            
            return dataFromStore.productsFromStore.ToList();
        }        
        var filteredProducts = dataFromStore.productsFromStore.Where(p => p.category.id == categoryId).ToList();                
        if (filteredProducts ==  null)
            throw new BussinessException($"{nameof(filteredProducts)} no puede ser nulo");        
        return filteredProducts;
    }

    public IEnumerable<Product> filterProductsBySearchTextAndCategory(string searchText, int[] categoryIds){
        //El texto del buscador si puede ser vacío
        if(searchText == null) throw new BussinessException($"{nameof(searchText)} no puede ser nulo");
        if(categoryIds == null) throw new BussinessException($"{nameof(categoryIds)} no puede ser nulo");

        var filteredProducts = new List<Product>();

        //Si la lista de categorias donde buscar esta vacío, devolvemos todos los productos
        if(categoryIds.Length == 0){
            foreach (var productList in dataFromStore.dictionaryOfProducts.Values){
                filteredProducts.AddRange(productList);
            }
        }else{
            //Si hay categorias especificadas
            foreach (var id in categoryIds){
                if (dataFromStore.dictionaryOfProducts.ContainsKey(id)){
                    //Guardamos todos los productos asociados a un id especifico            
                    filteredProducts.AddRange(dataFromStore.dictionaryOfProducts[id]);
                }
            }
        }

        //Ordenamos la lista con los productos para la busqueda binaria (por nombre de los productos)
        filteredProducts.Sort( (x,y) => string.Compare(x.name, y.name) );                


        //Verificamos si el texto del buscador por alguna razón es nulo o vacío. Y su valor es igual a "default"
        //es porque en el buscador nunca se escribió nada o solo había espacios en blanco.
        if(string.IsNullOrEmpty(searchText) || searchText == "default"){
            return filteredProducts;
        }    

        return filterByBinarySearch(searchText, filteredProducts);
        
    }

    private IEnumerable<Product> filterByBinarySearch(string searchText,List<Product> filteredProducts){

        var foundProducts = new List<Product>();
        int n = filteredProducts.Count;
        int step = (int)Math.Floor(Math.Sqrt(n));
        int prev = 0;

        while (prev < n)
        {
            int currentStep = Math.Min(step, n) - 1;
            Product currentProduct = filteredProducts[currentStep];
            
            if (currentProduct.name.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                for (int i = currentStep; i >= prev; i--){
                    Product product = filteredProducts[i];
                    if (product.name.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0){
                        foundProducts.Add(product);

                    }else break;                    
                }
                break;
            }

            prev = step;
            step += (int)Math.Floor(Math.Sqrt(n));
        }
        //Le permitimos que pueda devolver una lista vacía (pero no nula)
        return foundProducts;
    }
}
