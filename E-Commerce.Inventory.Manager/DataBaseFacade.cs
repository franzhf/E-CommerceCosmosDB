using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Linq;
using E_Commerce.Entities;

namespace E_Commerce.Inventory.Manager
{
    enum TypeOfOperation { by_net_sdk, by_sproc, by_func };
    //[Obsolete("TODO: Refactoring from static method to instance class")]
    public class DataBaseFacade<T> where T : IEntity
    {
        private const string _databaseId = "InventoryDB";
        private FeedOptions _feedOption = new FeedOptions { EnableCrossPartitionQuery = true };
        private TypeOfOperation _typeOfOperation = TypeOfOperation.by_net_sdk;
        private string _collectionId;
        private Uri _documentCollectionUri;

        public DataBaseFacade(string collectionId)
        {
            _collectionId = collectionId;
            _documentCollectionUri = UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId);
            // contract
            ExistsCollection();
        }

        public DocumentCollection  GetDocumentCollection()
        {
            DocumentClient client = DocumentDBClientConfig.GetClientInstance;
            Database database = client.CreateDatabaseQuery()
                .Where(db => db.Id == _databaseId).AsEnumerable()
                .FirstOrDefault();
            var documentCollection = from c in client.CreateDocumentCollectionQuery(database.SelfLink)
                                     where c.Id == _collectionId
                                     select c;
            /* not  working version
             * DocumentCollection documentCollection = client.CreateDocumentCollectionQuery(database.SelfLink)
                    .Where(coll => coll.Id == collectionId)
                   .FirstOrDefault();*/                        
            if (documentCollection == null)
                throw new Exception($"{_collectionId} doesnt exists in the database {_databaseId}");
            return documentCollection.AsEnumerable().FirstOrDefault();
        }

        public bool ExistsCollection()
        {
            var documentCollection = GetDocumentCollection();
            if (documentCollection == null)
                return false;
            return true;
        }

        public List<T> GetDocuments()
        {
            if (!ExistsCollection())
                throw new Exception($" {_collectionId}  does not exists in the database {_databaseId} !!!");
            DocumentClient client = DocumentDBClientConfig.GetClientInstance;
            IEnumerable<T> docs = from c in client.CreateDocumentQuery<T>(_documentCollectionUri, _feedOption)
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

        public async Task<List<T>> GetDocumentsAsync()
        {
            if (!ExistsCollection())
                throw new Exception($" {_collectionId}  does not exists in the database {_databaseId} !!!");
            DocumentClient client = DocumentDBClientConfig.GetClientInstance;
            List<T> resultSet = new List<T>();
            //  run a query asynchronously using the AsDocumentQuery() interface
            var queryble = client.CreateDocumentQuery<T>(_documentCollectionUri, _feedOption).AsDocumentQuery();
            while (queryble.HasMoreResults)
            {
                foreach (T doc in await queryble.ExecuteNextAsync())
                {
                    resultSet.Add(doc);
                }
            }

            /*
            var _documentCollectionUri = $"/dbs/{_databaseId}/colls/{collectionId}";
            IEnumerable<T> docs = from c in client.CreateDocumentQuery<T>(_documentCollectionUri, feedOption)
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

        public async Task<List<T>> GetDocumentsQueryAsync(string documentName)
        {            
            if (!ExistsCollection())
                throw new Exception($" {_collectionId}  does not exists in the database {_databaseId} !!!");
            var query = $"SELECT * FROM {_collectionId} c WHERE contains(c.name, '{documentName}')";
            var resultSet = await RunQueryAsync(query);
            return resultSet;
        }

        public async Task<List<T>> RunQueryAsync (string query)
        {
            DocumentClient client = DocumentDBClientConfig.GetClientInstance;
            List<T> resultSet = new List<T>();
            //  run a query asynchronously using the AsDocumentQuery() interface
            var queryble = client.CreateDocumentQuery<T>(_documentCollectionUri, query, _feedOption).AsDocumentQuery();
            while (queryble.HasMoreResults)
            {
                foreach (T doc in await queryble.ExecuteNextAsync())
                {
                    resultSet.Add(doc);
                }
            }
            return resultSet;
        }

        public async Task<bool> CreateDocument(T newDocument) 
        {
            var client = DocumentDBClientConfig.GetClientInstance;
            var storedProcedureId = "addProductSproc";
            var requestOptions = new RequestOptions { PartitionKey = new PartitionKey($"/products/{newDocument.id}") };
            // var documentCollection = GetDocumentCollection(collectionId);
            // documentCollection.PartitionKey.Paths.Add("/productsid");
            ResourceResponse<Document> resourceResponse = null;
            if (_typeOfOperation == TypeOfOperation.by_net_sdk)
                resourceResponse = await client.CreateDocumentAsync(_documentCollectionUri, newDocument);
            /*else
            await client.ExecuteStoredProcedureAsync<string>(UriFactory.CreateStoredProcedureUri(_databaseId, _collectionId, storedProcedureId)
                                                    , requestOptions
                                                    , newDocument);*/
            if (resourceResponse != null && resourceResponse.StatusCode == System.Net.HttpStatusCode.Created)
                return true;
             return false;
            
        }

        public async Task<bool> ExistsDocumentAsync(T document,string query)
        {
            var resultSet = await RunQueryAsync(query);
            if (resultSet.Count > 0)
                return true;
            return false;
        }

        public async Task<bool> UpdateDocumentAsync(T document)
        {
            var client = DocumentDBClientConfig.GetClientInstance;
            Uri documentUri = UriFactory.CreateDocumentUri(_databaseId, _collectionId, document.id);

            var response = await client.ReplaceDocumentAsync(documentUri, document);            
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return true;
            return false;
        }

    }
}
