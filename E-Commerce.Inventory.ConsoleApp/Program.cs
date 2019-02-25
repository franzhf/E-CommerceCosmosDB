using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce.Entities;
// for cosmosdb with documentdb
using System.Net;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;
using E_Commerce.Inventory.Manager;
using System.Threading.Tasks;

namespace E_Commerce.Inventory.ConsoleApp
{
    class Program
    {
        static DocumentClient client;
        

        static void Main(string[] args)
        {
            /*try
            {
                Program p = new Program();
                p.AsycnGetProducts().Wait();
            }
            catch (DocumentClientException de)
            {
                Exception baseException = de.GetBaseException();
                Console.WriteLine("{0} error occurred: {1}, Message: {2}", de.StatusCode, de.Message, baseException.Message);
            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                Console.WriteLine("Error: {0}, Message: {1}", e.Message, baseException.Message);
            }
            finally
            {
                Console.WriteLine("End of demo, press any key to exit.");
                Console.ReadKey();
            }*/

            //ListDBs();
            client = DocumentDBClientConfig.GetClientInstance;
            ProductHandle productHandle = new ProductHandle();
            /*Console.WriteLine("Show list of products!!!");
            Console.WriteLine("==============================================================\n\n");
            foreach (var item in productHandle.GetProducts())
            {
                Console.WriteLine($"product {item.name}  price:{item.price}");
            }


            Console.WriteLine("[Async Fashion] Show list of products!!!");
            Console.WriteLine("==============================================================\n\n");
            var data = productHandle.GetProductsAsync();
            foreach (var item in data.Result)
            {
                Console.WriteLine($"product {item.name}  price:{item.price}");
            }
            */
           /* var productName = "iPad";
            Console.WriteLine($"[Async Fashion] Get products that match with {productName}!!!");
            Console.WriteLine("==============================================================\n\n");
            var data = productHandle.GetProductsByNameAsync(productName);
            foreach (var item in data.Result)
            {
                Console.WriteLine($"product {item.name}  price:{item.price}");
            }
            Console.WriteLine("List products is done!!!!");*/

            Console.WriteLine($"[Async Fashion] Add Product !!!");
            Console.WriteLine("==============================================================\n\n");
            Product p = new Product
            {
                id = "1013",
                name = "iPhone x",
                price = 300
            };

            productHandle.AddProductAsync(p);

            /*
            Task t = Task.Run(() => 
            {
                var data = productHandle.GetProductsByNameAsync(productName);
                foreach (var item in data.Result)
                {
                    Console.WriteLine($"product {item.name}  price:{item.price}");
                }
            });

            t.Wait();*/


            // Accessing documents
            /*var dbName = "InventoryDB";
            var collectionId = "products";
            ListProducts(client,dbName, collectionId);

            GetProductByName(client, dbName, collectionId, "iPad");
            GetProductWhichLowerThan(client, dbName, collectionId, 500);*/

            Console.ReadKey();
        }


        private async Task AsyncListProducts()
        {
            client = DocumentDBClientConfig.GetClientInstance;


        }

        private static void ListDBs()
        {
            using (var client = DocumentDBClientConfig.GetClientInstance)
            {
                var dbs = client.CreateDatabaseQuery();
                foreach(var db in dbs)
                {
                    Console.WriteLine("Database Id: {0}; Rid {1} \n", db.Id, db.ResourceId);
                    ListCollections(client, db, db.Id);
                }
            }
        }

        private static void ListCollections(DocumentClient client, Database db, string dbname)
        {
            if (client == null && db == null)
                return;

            List<DocumentCollection> collections = client.CreateDocumentCollectionQuery(db.SelfLink).ToList();
            Console.WriteLine("{0} collections for database: {1}", collections.Count.ToString(), dbname);
            foreach (DocumentCollection col in collections)
            {
                Console.WriteLine("Collection Id: {0}; Rid {1}", col.Id, col.ResourceId);
                ListDocuments(client, dbname, col.Id);
            }
        }

        public static void ListDocuments(DocumentClient client, string dbName, string collName)
        {
            // indicating whether users are enabled to send more than one request to execute the query in the Azure Cosmos DB service.
            var feedOption = new FeedOptions { EnableCrossPartitionQuery = true };
            var _documentCollectionUri = "/dbs/" + dbName + "/colls/" + collName;
            if (client != null)
            {
                IEnumerable<Document> docs = from c in client.CreateDocumentQuery(_documentCollectionUri, feedOption)
                                             select c;
                if (docs != null)
                {
                    foreach (var doc in docs)
                    {
                        Console.WriteLine("Document Id: {0}; Rid {1} ", doc.Id, doc.ResourceId);
                    }
                }
            }
        }

        public static void ListProducts(DocumentClient client, string dbName, string collName)
        {
            Console.WriteLine("List all products");
            // indicating whether users are enabled to send more than one request to execute the query in the Azure Cosmos DB service.
            var feedOption = new FeedOptions { EnableCrossPartitionQuery = true };
            var _documentCollectionUri = "/dbs/" + dbName + "/colls/" + collName;
            if (client != null)
            {
                IEnumerable<Product> docs = from c in client.CreateDocumentQuery<Product>(_documentCollectionUri, feedOption)
                                             select c; if (docs != null)
                {
                    //Console.WriteLine("Documents for collection {0}", collName);
                    foreach (var doc in docs)
                    {
                        Console.WriteLine($"Name: {doc.name} Price: {doc.price}" );
                    }
                }
            }
        }

        public static void GetProductByName(DocumentClient client, string dbName, string collName, string productName)
        {
            Console.WriteLine($"List {productName} products");
            // indicating whether users are enabled to send more than one request to execute the query in the Azure Cosmos DB service.
            var feedOption = new FeedOptions { EnableCrossPartitionQuery = true };
            var _documentCollectionUri = "/dbs/" + dbName + "/colls/" + collName;
            if (client != null)
            {
                IEnumerable<Product> docs = from c in client.CreateDocumentQuery<Product>(_documentCollectionUri, feedOption)
                                            where c.name.Contains(productName)// name should be mapping product document
                                            select c;
                if (docs != null)
                {
                    foreach (var doc in docs)
                    {
                        Console.WriteLine($"Name: {doc.name} Price: {doc.price}");
                    }
                }
            }
        }

        public static void GetProductWhichLowerThan(DocumentClient client, string dbName, string collName, double limitPrice)
        {
            Console.WriteLine($"\n List products which the price lower than {limitPrice} ");
            // indicating whether users are enabled to send more than one request to execute the query in the Azure Cosmos DB service.
            var feedOption = new FeedOptions { EnableCrossPartitionQuery = true };
            var _documentCollectionUri = "/dbs/" + dbName + "/colls/" + collName;
            if (client != null)
            {
                IEnumerable<Product> docs = from c in client.CreateDocumentQuery<Product>(_documentCollectionUri, feedOption)
                                            where c.price <= limitPrice// name should be mapping product document
                                            select c;
                if (docs != null)
                {
                    foreach (var doc in docs)
                    {
                        Console.WriteLine($"Name: {doc.name} Price: {doc.price}");
                    }
                }
            }
        }
    }
}
