using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace E_Commerce.Inventory.Manager
{
    public class DocumentDBClientConfig
    {
        //private const string EndpointUrl = "https://fhf-api-core-sql.documents.azure.com:443/";
        //private const string PrimaryKey = "QBHiOsvFAMJ65i7u0QAuKivVQxn0l42d6Q6c4VHMkZrqdUkKwx8qfJdHT2ZmEoGhIMkyPW2iUqNkFV7O9prAdQ==";
        private const string EndpointUrl = "https://localhost:8081";
        private const string PrimaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";

        private static DocumentClient _client;
        private DocumentDBClientConfig()
        {
            /*_client = new DocumentClient(new Uri(EndpointUrl), PrimaryKey);
            if (_client == null)
                throw new Exception("Cannot created an cosmosdb client service!!!");*/
        }
     
        static public DocumentClient GetClientInstance
        {
            get
            {
                if(_client == null)
                {
                    _client = new DocumentClient(new Uri(EndpointUrl), PrimaryKey);
                    if (_client == null)
                        throw new Exception("Cannot created an document client service!!!");
                }
                return _client;
            }
        }
    }
}
