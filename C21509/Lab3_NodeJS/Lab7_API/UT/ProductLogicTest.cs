using Core.Business;
using NUnit.Framework;
using Store_API.Models;
using Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UT
{
    [TestFixture]
    public class ProductLogicTest
    {
        private List<Product> productsInserted;

        [SetUp]
        public void SetUp()
        {
            productsInserted = new List<Product>();
        }

        [Test]
        public async Task DelegateTesting()
        {
            ProductLogic productLogic = new ProductLogic();
            productLogic.referenceProductInserted = TestProductInsertedDelegate;

            Product addedProduct = new Product
            {
                Id = 1,
                Name = "Iphone",
                ImageURL = "/img/Iphone.jpg",
                Description= "Producto prueba",
                Price = 200,
                Categoria = new Category(1, "Electrónica") 
            };

            await productLogic.insertionProductAsyncUT(addedProduct, TestProductInsertedDelegate);

            Assert.IsTrue(productsInserted.Contains(addedProduct));
        }

        private void TestProductInsertedDelegate(Product newProduct)
        {
            productsInserted.Add(newProduct);
        }
    }
}