using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using E_Commerce.Entities;

namespace E_Commerce.Inventory.Manager
{


    public class ProductHandle
    {
        const string _productCollection = "products";
        DataBaseFacade<Product> dataBaseFacade;

        public ProductHandle()
        {
            dataBaseFacade = new DataBaseFacade<Product>(_productCollection);
        }

        public List<Product> GetProducts()
        {
            return dataBaseFacade.GetDocuments();
        }

        public Product GetProductsIncludeTax(Product product)
        {
            var query = $"SELECT p.id, p.name, udf.udfTax(p.price) + p.price as price FROM products p" +
                $" WHERE p.id = '{product.id}'";
            var result = dataBaseFacade.RunQueryAsync(query).Result;
            return result[0];
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await dataBaseFacade.GetDocumentsAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name)
        {
            //string sqlQuery = $"SELECT * FROM products p WHERE contains(p.name, '{name}')";
            return await dataBaseFacade.GetDocumentsQueryAsync(name);
        }

        public async Task<bool> AddProductAsync(Product product)
        {
            try
            {
                var response = await dataBaseFacade.CreateDocument(product);
                return response;
            }
            catch (System.AggregateException ex)
            {
                throw new ProductException ("Cannot add duplicate products", ex);
            }
        }

        public async Task<bool> ExistsProductAsync(Product product)
        {
            var query = $"SELECT * FROM {_productCollection} c WHERE c.id = '{product.id}' AND" +
                $" c.name = '{product.name}' AND c.price = {product.price}";
            bool response = await dataBaseFacade.ExistsDocumentAsync(product, query);
            return response;
        }

        public bool UpdateProductAsync(Product product)
        {
            bool response = dataBaseFacade.UpdateDocumentAsync(product).Result;
            return response;
        }

    }
}
