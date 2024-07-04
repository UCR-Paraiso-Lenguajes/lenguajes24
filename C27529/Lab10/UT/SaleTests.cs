using NUnit.Framework;
using storeApi;
using storeApi.Models;
using storeApi.Business;
using System.Collections.Generic;

namespace storeApi.Tests
{
    public class SaleTests
    {
        private IEnumerable<Product> validProducts;
        private string validAddress;
        private decimal validAmount;
        private PaymentMethod.Type validPaymentMethod;
        private Category.ProductCategory validCategory;

        [SetUp]
        public void Setup()
        {
            var category = new Category();
            validCategory = category.GetCategoryById(1); // Assuming the category ID 1 exists

            validProducts = new List<Product>
            {
                new Product { Id = 1, Name = "Product1", Price = 10.0m, Description = "Desc1", ImageURL = "http://image1.com", Category = validCategory },
                new Product { Id = 2, Name = "Product2", Price = 20.0m, Description = "Desc2", ImageURL = "http://image2.com", Category = validCategory }
            };
            validAddress = "123 Main St";
            validAmount = 30.0m;
            validPaymentMethod = PaymentMethod.Type.CASH;
        }

        [Test]
        public void SaleConstructor_ValidParameters_ShouldCreateSale()
        {
            // Arrange
            var paymentMethod = PaymentMethod.SetPaymentType(validPaymentMethod);

            // Act
            var sale = new Sale(validProducts, validAddress, validAmount, paymentMethod.PaymentType);

            // Assert
            Assert.AreEqual(validProducts, sale.Products);
            Assert.AreEqual(validAddress, sale.Address);
            Assert.AreEqual(validAmount, sale.Amount);
            Assert.AreEqual(paymentMethod.PaymentType, sale.PaymentMethod);
            Assert.IsNotNull(sale.PurchaseNumber);
            Assert.IsNotEmpty(sale.PurchaseNumber);
        }

        [Test]
        public void SaleConstructor_NullProducts_ShouldThrowArgumentException()
        {
            // Arrange
            var paymentMethod = PaymentMethod.SetPaymentType(validPaymentMethod);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => new Sale(null, validAddress, validAmount, paymentMethod.PaymentType));
            Assert.That(ex.Message, Is.EqualTo("Uno o más parámetros son nulos o tienen valores no válidos."));
        }

        [Test]
        public void SaleConstructor_EmptyProducts_ShouldThrowArgumentException()
        {
            // Arrange
            var paymentMethod = PaymentMethod.SetPaymentType(validPaymentMethod);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => new Sale(new List<Product>(), validAddress, validAmount, paymentMethod.PaymentType));
            Assert.That(ex.Message, Is.EqualTo("Uno o más parámetros son nulos o tienen valores no válidos."));
        }

        [Test]
        public void SaleConstructor_NullAddress_ShouldThrowArgumentException()
        {
            // Arrange
            var paymentMethod = PaymentMethod.SetPaymentType(validPaymentMethod);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => new Sale(validProducts, null, validAmount, paymentMethod.PaymentType));
            Assert.That(ex.Message, Is.EqualTo("Uno o más parámetros son nulos o tienen valores no válidos."));
        }

        [Test]
        public void SaleConstructor_EmptyAddress_ShouldThrowArgumentException()
        {
            // Arrange
            var paymentMethod = PaymentMethod.SetPaymentType(validPaymentMethod);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => new Sale(validProducts, "", validAmount, paymentMethod.PaymentType));
            Assert.That(ex.Message, Is.EqualTo("Uno o más parámetros son nulos o tienen valores no válidos."));
        }

        [Test]
        public void SaleConstructor_NegativeAmount_ShouldThrowArgumentException()
        {
            // Arrange
            var paymentMethod = PaymentMethod.SetPaymentType(validPaymentMethod);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => new Sale(validProducts, validAddress, -10.0m, paymentMethod.PaymentType));
            Assert.That(ex.Message, Is.EqualTo("Uno o más parámetros son nulos o tienen valores no válidos."));
        }
    }
}
