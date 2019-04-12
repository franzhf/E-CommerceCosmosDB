using System;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;

namespace E_Commerce.Inventory.Function
{
    internal class StorageAccountFactory
    {
        const string ACOUNTNAME = "devstoreaccount1";
        const string PRIMARYKEY = "Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==";
        const string CONNECTION_STRING = "AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1";

        internal static CloudStorageAccount GetCloudStorageAccountInstance(string option = "Declarative")
        {
            CloudStorageAccount cloudStorageAccount = null;
            try
            {

                if(option.Contains("Declarative"))
                    cloudStorageAccount = CloudStorageAccount.Parse(CONNECTION_STRING);
                else if (option.Contains("Imperative"))
                    cloudStorageAccount = new CloudStorageAccount(new Microsoft.WindowsAzure.Storage.Auth
                                         .StorageCredentials(ACOUNTNAME, PRIMARYKEY), true);
            }
            catch (Exception ex)
            {
                // LogError(ex);
                return GetCloudStorageAccountInstance("Imperative");
            }

            return cloudStorageAccount;
        }
    }
}