using NUnit.Framework;
using E_Commerce.Inventory.Manager;
using E_Commerce.Entities;

namespace E_Commerce.Inventory.Manager.IntegrationTest
{
    public class DataBaseFacadeTests
    {
        string collectionId;
        DataBaseFacade<Product> dataBaseFacade;

        [SetUp]
        public void Setup()
        {
            collectionId = "products";
            dataBaseFacade = new DataBaseFacade<Product>(collectionId);
        }


        [Test]
        public void Should_be_true_when_document_exists()
        {
            // Arrange
            Product inputProduct = new Product
            {
                id = "1010",
                name = "iPad",
                price = 500
            };
            // Act 
            var query = $"SELECT * FROM products p WHERE p.id = '{inputProduct.id}'";
            var result = dataBaseFacade.ExistsDocumentAsync(inputProduct,query).Result;

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void Should_be_false_when_a_document_doesnt_exists()
        {
            // Arrange
            Product inputProduct = new Product
            {
                id = "10",
                name = "iPad",
                price = 500
            };
            // Act 
            var query = $"SELECT * FROM products p WHERE p.id = {inputProduct.id}";
            var result = dataBaseFacade.ExistsDocumentAsync(inputProduct, query).Result;

            // Assert
            Assert.IsFalse(result);
        }

    }
}