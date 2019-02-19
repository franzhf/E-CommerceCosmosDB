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
    [Obsolete("TODO: Refactoring from static method to instance class")]
    public class DataBaseFacade
    {
        private const string databaseId = "InventoryDB";
        private static FeedOptions _feedOption = new FeedOptions { EnableCrossPartitionQuery = true };
        //private const string CollectionName = "products";

        public static DocumentCollection  GetDocumentCollection(string collectionName)
        {
            DocumentClient client = DocumentDBClientConfig.GetClientInstance;
            Database database = client.CreateDatabaseQuery()
                .Where(db => db.Id == databaseId).AsEnumerable()
                .FirstOrDefault();
            var documentCollection = from c in client.CreateDocumentCollectionQuery(database.SelfLink)
                                     where c.Id == collectionName
                                     select c;
            /* not  working version
             * DocumentCollection documentCollection = client.CreateDocumentCollectionQuery(database.SelfLink)
                    .Where(coll => coll.Id == collectionName)
                    .FirstOrDefault();*/
            if (documentCollection == null)
                throw new Exception($"{collectionName} doesnt exists in the database {databaseId}");
            return documentCollection.AsEnumerable().FirstOrDefault();
        }

        public static bool ExistsCollection(string collectionName)
        {
            var documentCollection = GetDocumentCollection(collectionName);
            if (documentCollection == null)
                return false;
            return true;
        }

        public static List<T> GetDocuments<T>(string collectionName)
        {
            if (!ExistsCollection(collectionName))
                throw new Exception($" {collectionName}  does not exists in the database {databaseId} !!!");
            DocumentClient client = DocumentDBClientConfig.GetClientInstance;
            var documentCollectionUri = $"/dbs/{databaseId}/colls/{collectionName}";
            IEnumerable<T> docs = from c in client.CreateDocumentQuery<T>(documentCollectionUri, _feedOption)
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
            DocumentClient client = DocumentDBClientConfig.GetClientInstance;
            List<T> resultSet = new List<T>();
            var documentCollectionUri = $"/dbs/{databaseId}/colls/{collectionName}";            
            //  run a query asynchronously using the AsDocumentQuery() interface
            var queryble = client.CreateDocumentQuery<T>(documentCollectionUri, _feedOption).AsDocumentQuery();
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

        public static async Task<List<T>> GetDocumentsQueryAsync<T>(string collectionName, string sqlQuery)
        {            
            if (!ExistsCollection(collectionName))
                throw new Exception($" {collectionName}  does not exists in the database {databaseId} !!!");
            DocumentClient client = DocumentDBClientConfig.GetClientInstance;
            List<T> resultSet = new List<T>();
            var documentCollectionUri = $"/dbs/{databaseId}/colls/{collectionName}";
            //  run a query asynchronously using the AsDocumentQuery() interface
            var queryble = client.CreateDocumentQuery<T>(documentCollectionUri, sqlQuery, _feedOption).AsDocumentQuery();
            while (queryble.HasMoreResults)
            {
                foreach (T doc in await queryble.ExecuteNextAsync())
                {
                    resultSet.Add(doc);
                }
            }
            return resultSet;
        }

        public static async void CreateDocument<T>(string collectionName, T newDocument)
        {
            var client = DocumentDBClientConfig.GetClientInstance;
            var documentCollectionUri = $"/dbs/{databaseId}/colls/{collectionName}";
            await client.CreateDocumentAsync(documentCollectionUri, newDocument);

        }

    }
}
