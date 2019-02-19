using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Linq;
using Microsoft.CSharp;
namespace E_Commerce.Inventory.Manager
{
    public class DataBaseFacade
    {
        private const string databaseId = "InventoryDB";
        //private const string CollectionName = "products";

        public static DocumentCollection  CreateDBInstanceByCollection(string collectionName)
        {
            var feedOption = new FeedOptions { EnableCrossPartitionQuery = true };
            DocumentClient client = DocumentDBClientConfig.GetClientInstance;
            Database database = client.CreateDatabaseQuery()
                .Where(db => db.Id == databaseId).AsEnumerable()
                .FirstOrDefault();

            DocumentCollection documentCollection = client.CreateDocumentCollectionQuery(database.SelfLink)
                    .Where(coll => coll.Id == collectionName)
                    .FirstOrDefault();
            if (documentCollection == null)
                throw new Exception($"{collectionName} doesnt exists in the database {databaseId}");
            return documentCollection;
        }

        public static bool ExistsCollection(string collectionName)
        {
            var feedOption = new FeedOptions { EnableCrossPartitionQuery = true };
            DocumentClient client = DocumentDBClientConfig.GetClientInstance;
            Database database = client.CreateDatabaseQuery()
                .Where(db => db.Id == databaseId).AsEnumerable()
                .FirstOrDefault();
            var documentCollection = from c in client.CreateDocumentCollectionQuery(database.SelfLink)
                                                    where c.Id == collectionName
                                                    select c; 
                                                    
            if(documentCollection == null)
                throw new Exception($"{collectionName} doesnt exists in the database {databaseId}");
            return true;
        }

        public static List<T> GetDocuments<T>(string collectionName)
        {
            if (!ExistsCollection(collectionName))
                throw new Exception($" {collectionName}  does not exists in the database {databaseId} !!!");

            var feedOption = new FeedOptions { EnableCrossPartitionQuery = true };
            DocumentClient client = DocumentDBClientConfig.GetClientInstance;
            var documentCollectionUri = $"/dbs/{databaseId}/colls/{collectionName}";
            IEnumerable<T> docs = from c in client.CreateDocumentQuery<T>(documentCollectionUri, feedOption)
                                            select c;
            List<T> resultSet = new List<T>();
            if (docs != null)
            {
                foreach (var doc in docs)
                {
                    resultSet.Add(doc);
                }
            }
            return resultSet;
        }

        public static async Task<List<T>> GetDocumentsAsync<T>(string collectionName)
        {
            if (!ExistsCollection(collectionName))
                throw new Exception($" {collectionName}  does not exists in the database {databaseId} !!!");
            var feedOption = new FeedOptions { EnableCrossPartitionQuery = true };
            DocumentClient client = DocumentDBClientConfig.GetClientInstance;
            List<T> resultSet = new List<T>();
            var documentCollectionUri = $"/dbs/{databaseId}/colls/{collectionName}";            
            //  run a query asynchronously using the AsDocumentQuery() interface
            var queryble = client.CreateDocumentQuery<T>(documentCollectionUri, feedOption).AsDocumentQuery();
            while (queryble.HasMoreResults)
            {
                foreach (T doc in await queryble.ExecuteNextAsync())
                {
                    resultSet.Add(doc);
                }
            }

            /*
            var documentCollectionUri = $"/dbs/{databaseId}/colls/{collectionName}";
            IEnumerable<T> docs = from c in client.CreateDocumentQuery<T>(documentCollectionUri, feedOption)
                                  select c;
            
            List<T> resultSet = new List<T>();
            if (docs != null)
            {
                foreach (var doc in docs)
                {
                    resultSet.Add(doc);
                }
            }*/
            return resultSet;
        }


    }
}
