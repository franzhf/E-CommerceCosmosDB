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
                Id = "1010",
                Name = "iPad",
                Price = 500
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
                Id = "10",
                Name = "iPad",
                Price = 500
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
            var expectedResult = 2600;
            Product inputProduct = new Product
            {
                Id = "2000"
                //price = 500
            };
            // Act 
            var result = productHandle.GetProductsIncludeTax(inputProduct);

            // Assert
            Assert.IsTrue(result.PriceWithTaxes == expectedResult);
        }
        [Test]
        public void Should_update_product_name_no_partition_key_value()
        {
            // Arrange
            Product inputProduct = new Product
            {
                Id = "2004",
                Name = "Monitor Samsung 3",
                Price = 200,

            };
            // Act 
            var result = productHandle.UpdateProductAsync(inputProduct).Result;

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void Should_update_product_name_partition_key_value()
        {
            // Arrange
            Product inputProduct = new Product
            {
                Id = "2000",
                Name = "TV 4K",
                Price = 2000,
                Category = "Electronic" // Partition key
            };
            // Act 
            var result = productHandle.UpdateProductAsync(inputProduct).Result;

            // Assert
            Assert.IsTrue(result);
        }


        [Test]
        public void Should_add_new_product()
        {
            // Arrange
            Product inputProduct = new Product
            {
                Id = "6004",
                Name = "Monitor Samsung 2",
                Price = 200

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
                Id = "1010",
                Name = "iPad",
                Price = 500
            };
            // Act 
            // Assert
            var ex = Assert.ThrowsAsync<ProductException>(async () => await productHandle.AddProductAsync(inputProduct));
            Assert.That(ex.Message, Is.EqualTo("Cannot add duplicate products"));
        }
    }
}
