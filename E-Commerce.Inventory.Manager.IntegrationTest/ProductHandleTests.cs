using NUnit.Framework;
using E_Commerce.Inventory.Manager;
using E_Commerce.Entities;

namespace E_Commerce.Inventory.Manager.IntegrationTest
{
    class ProductHandleTests
    {
        string collectionId;
        ProductHandle productHandle;

        [SetUp]
        public void Setup()
        {
            collectionId = "products";
            productHandle = new ProductHandle();
        }

        [Test]
        public void Should_be_true_when_product_match_all_fields()
        {
            // Arrange
            Product inputProduct = new Product
            {
                id = "1010",
                name = "iPad",
                price = 500
            };
            // Act 
            bool result = productHandle.ExistsProductAsync(inputProduct).Result;

            // Assert
            Assert.IsFalse(result);
        }
        [Test]
        public void Should_be_false_when_a_product_doesnt_exists()
        {
            // Arrange
            Product inputProduct = new Product
            {
                id = "10",
                name = "iPad",
                price = 500
            };
            // Act 
            var result = productHandle.ExistsProductAsync(inputProduct).Result;

            // Assert
            Assert.IsFalse(result);
        }
        [Test]
        public void Should_price_include_the_tax_to_amount()
        {
            // Arrange
            var expectedResult = 600;
            Product inputProduct = new Product
            {
                id = "1010"
                //price = 500
            };
            // Act 
            var result = productHandle.GetProductsIncludeTax(inputProduct);

            // Assert
            Assert.IsTrue(result.price == expectedResult);
        }
        [Test]
        public void Should_update_product_name()
        {
            // Arrange
            Product inputProduct = new Product
            {
                id = "1010",
                name = "new iPad Pro",
                price = 500
            };
            // Act 
            var result = productHandle.UpdateProductAsync(inputProduct);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void Should_add_new_product()
        {
            // Arrange
            Product inputProduct = new Product
            {
                id = "2004",
                name = "Monitor Samsung 2",
                price = 200
            };
            // Act 
            var result = productHandle.AddProductAsync(inputProduct).Result;

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void Should_not_add_duplicate_product()
        {
            // Arrange
            Product inputProduct = new Product
            {
                id = "1010",
                name = "iPad",
                price = 500
            };
            // Act 
            // Assert
            var ex = Assert.ThrowsAsync<ProductException>(async () => await productHandle.AddProductAsync(inputProduct));
            Assert.That(ex.Message, Is.EqualTo("Cannot add duplicate products"));

            

        }
    }
}
