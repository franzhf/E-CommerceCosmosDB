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

namespace E_Commerce.Inventory.ConsoleApp
{
    class Program
    {
        private const string EndpointUrl = "https://fhf-api-core-sql.documents.azure.com:443/";
        private const string PrimaryKey = "QBHiOsvFAMJ65i7u0QAuKivVQxn0l42d6Q6c4VHMkZrqdUkKwx8qfJdHT2ZmEoGhIMkyPW2iUqNkFV7O9prAdQ==";
        
        private const string dbName = "InventoryDB";
        private const string CollectionName = "products";


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
            DocumentClient client = new DocumentClient(new Uri(EndpointUrl), PrimaryKey);
            // Accessing documents
            ListProducts(client,dbName, CollectionName);
            GetProductByName(client, dbName, CollectionName, "iPad");
            GetProductWhichLowerThan(client, dbName, CollectionName, 500);
            
            Console.ReadKey();
        }


        private async Task AsycnGetProducts()
        {
            //this.client = new DocumentClient(new Uri(EndpointUrl), PrimaryKey);
            
        }

        private static void ListDBs()
        {
            using (var client = new DocumentClient(new Uri(EndpointUrl), PrimaryKey))
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
            var documentCollectionUri = "/dbs/" + dbName + "/colls/" + collName;
            if (client != null)
            {
                IEnumerable<Document> docs = from c in client.CreateDocumentQuery(documentCollectionUri, feedOption)
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
            var documentCollectionUri = "/dbs/" + dbName + "/colls/" + collName;
            if (client != null)
            {
                IEnumerable<Product> docs = from c in client.CreateDocumentQuery<Product>(documentCollectionUri, feedOption)
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
            var documentCollectionUri = "/dbs/" + dbName + "/colls/" + collName;
            if (client != null)
            {
                IEnumerable<Product> docs = from c in client.CreateDocumentQuery<Product>(documentCollectionUri, feedOption)
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
            var documentCollectionUri = "/dbs/" + dbName + "/colls/" + collName;
            if (client != null)
            {
                IEnumerable<Product> docs = from c in client.CreateDocumentQuery<Product>(documentCollectionUri, feedOption)
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
