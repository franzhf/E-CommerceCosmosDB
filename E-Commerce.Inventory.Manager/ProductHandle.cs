using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using E_Commerce.Entities;

namespace E_Commerce.Inventory.Manager
{
    public class ProductHandle
    {
        const string _productCollection = "products";

        public List<Product> GetProducts()
        {
            return DataBaseFacade.GetDocuments<Product>(_productCollection);
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await DataBaseFacade.GetDocumentsAsync<Product>(_productCollection);
        }

        public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name)
        {
            string sqlQuery = $"SELECT * FROM products p WHERE contains(p.name, '{name}')";
            return await DataBaseFacade.GetDocumentsQueryAsync<Product>(_productCollection, sqlQuery);
        }

        public async void AddProduct(Product product)
        {
            
            DataBaseFacade.CreateDocument<Product>(_productCollection, product);
        }
    }
}
