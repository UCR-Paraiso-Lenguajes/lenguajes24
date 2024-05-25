using System.Linq;
using MyStoreAPI.Models;
namespace MyStoreAPI.Models
{
    public sealed class Categories{
        public IEnumerable<Category> AllProductCategories {get; private set;}        

        private Categories(){

            List<Category> orderedCategoryList = new List<Category>(){
                new Category(0, "Todos los productos"),
                new Category(1, "Redes"),
                new Category(2, "Celulares"),
                new Category(3, "Videojuegos"),
                new Category(4, "Entretenimiento"),
                new Category(5, "Música"),
                new Category(6, "Computadoras"),
                new Category(7, "Juguetes")
            };
            //Nos ahorramos la creacion de una clase Compare<Category>
            orderedCategoryList.Sort((x, y) => string.Compare(x.name, y.name));
            AllProductCategories = orderedCategoryList;
        }        
        public static readonly Categories Instance;                                

        static Categories() {
            Categories.Instance = new Categories();
        }        
    }

}
