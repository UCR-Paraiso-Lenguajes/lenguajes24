using Core;
using MyStoreAPI.Business;
using MyStoreAPI.DB;
using MyStoreAPI.Models;
using NUnit.Framework;
namespace UT{

    [TestFixture]
    public class ProductCategoriesTesting : TestBase{
        
        private ProductsLogic productsLogic;


        [SetUp]
        public void SetUp(){
            productsLogic = new ProductsLogic();            
        }

        [Test]
        public void GetProductsByIdCategory(){
            IEnumerable<Product> products;


            //Pruebas para Categoria Celulares

           products = productsLogic.filterProductsByCategory(2);

            foreach (var item in products){                
                Assert.AreEqual("Celulares",item.category.name);                
                Assert.AreEqual(2,item.category.id);                
            }
            Assert.AreEqual(3,products.Count());


            //Pruebas para Categoria Videojuegos
            products = productsLogic.filterProductsByCategory(3);

            foreach (var item in products){                
                Assert.AreEqual("Videojuegos",item.category.name);                
                Assert.AreEqual(3,item.category.id);                
            }
            Assert.AreEqual(2,products.Count());

            //Pruebas para Categoria Juguete (no hay productos con esta categoria)
            products = productsLogic.filterProductsByCategory(7);

            foreach (var item in products){                
                Assert.AreEqual("Juguetes",item.category.name);                
                Assert.AreEqual(7,item.category.id);                
            }
            Assert.AreEqual(0,products.Count());
        }

        [Test]
        public void ErrorObtainingProducts(){
            IEnumerable<Product> products;
            
            //Nombre diferente
                //Pruebas para Categoria Videojuegos
            products = productsLogic.filterProductsByCategory(3);

            foreach (var item in products){                
                Assert.AreNotEqual("Nombre diferente",item.category.name);
                Assert.AreNotEqual(2,item.category.id);
            }
            Assert.AreNotEqual(0,products.Count());


            //ID Categoria menor a cero, ya que la categoria 0 hace referencia a todas las categorias
            Assert.Throws<BussinessException>(() => productsLogic.filterProductsByCategory(-1));

            //ID Categoria que no existe
            Assert.Throws<BussinessException>(() => productsLogic.filterProductsByCategory(10));
        }

        [Test]
        public void getAndFilterProductsBySearchAndCategoryWithSuccess(){
            IEnumerable<Product> products;
            
            int[] ids = new int[] {1,2,3};
            products = productsLogic.filterProductsBySearchTextAndCategory("use",ids);

            //Comprobar el filtro de productos
            Assert.AreNotEqual(0,products.Count());
            
            //Comprobar la exactitud del filtro
            Product firstProduct = products.FirstOrDefault();
            string productName = firstProduct.name;
            Assert.AreEqual("Mouse Microsoft", productName);

            //Comprobar la cantidad de productos sin filtro por texto
            products = productsLogic.filterProductsBySearchTextAndCategory("default",ids);
            Assert.IsTrue(products.Count() > 1);

            //Comprobar que no solo se tienen un producto
            firstProduct = products.FirstOrDefault();
            productName = firstProduct.name;
            Assert.AreNotEqual("Mouse Microsoft", productName);

            //Multiples productos
                //Se buscan los Dualshock de PS 4 y 5
            products = productsLogic.filterProductsBySearchTextAndCategory("ps",ids);
            Assert.AreEqual(products.Count(), 2);

            //Comprobar que hayan varios productos (para los controles de PS solo hay 2, uno de PS5 y otro de PS4)
            List<Product> productsPS = products.ToList();;
            string producPS4 = products.FirstOrDefault().name;
            string producPS5 = products.Last().name;
            Assert.AreEqual("Dualshock PS5", producPS4);
            Assert.AreEqual("Dualshock PS4", producPS5);


            //Todas las categ
                //Nota: "default" es el valor que recibe la API en caso de que el buscador del FE envie un Null, " " o undefined
            ids = new int[] {1,2,3,4,5,6,7};//todos los productos
            products = productsLogic.filterProductsBySearchTextAndCategory("default",ids);
            Assert.AreEqual(Store.Instance.Products.Count(), products.Count());

            //Categ no indicadas                
            ids = new int[0];//todos los productos
            products = productsLogic.filterProductsBySearchTextAndCategory("default",ids);
            Assert.AreEqual(Store.Instance.Products.Count(), products.Count());
        }


        [Test]
        public void getAndFilterProductsBySearchAndCategoryWithError(){
            IEnumerable<Product> products;            
            int[] ids = null;
            string searchText = null;
            Assert.Throws<BussinessException>(() => products = productsLogic.filterProductsBySearchTextAndCategory(searchText,ids));                     
        }
    }

}

