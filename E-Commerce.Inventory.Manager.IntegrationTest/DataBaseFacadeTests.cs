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
                Id = "1010",
                Name = "iPad",
                Price = 500
            };
            // Act 
            var query = $"SELECT * FROM products p WHERE p.id = '{inputProduct.Id}'";
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
                Id = "10",
                Name = "iPad",
                Price = 500
            };
            // Act 
            var query = $"SELECT * FROM products p WHERE p.id = {inputProduct.Id}";
            var result = dataBaseFacade.ExistsDocumentAsync(inputProduct, query).Result;

            // Assert
            Assert.IsFalse(result);
        }
        [Test]
        public void Should_add_new_product_using_SProc()
        {
            // Arrange
            Product inputProduct = new Product
            {
                Id = "6007",
                Name = "smart keyboard 2",
                Price = 100,
                Category = "Electronic"
            };
            // Act 
            var result = dataBaseFacade.CreateDocumentSprocAsync(inputProduct, "Electronic").Result;

            // Assert
            Assert.IsTrue(result);
        }
        [Test]
        public void Should_delete_document_and_create_a_new_delete_document()
        {
            // Arrange
            var document = new {
                id = "2001",
                postTrigger = "postDeleteProductAddDeletedDocument",
                pkeyValue = "category"
            };

            // Act 
            var result = dataBaseFacade.DeleteDocumentAsync(document.id, document.postTrigger, document.pkeyValue).Result;

            // Assert
            Assert.IsTrue(result);
        }
    }
}