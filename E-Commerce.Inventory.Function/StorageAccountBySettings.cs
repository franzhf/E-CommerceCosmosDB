using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;

namespace E_Commerce.Inventory.Function
{
    class StorageAccountSettings : ICloudStorageStrategy
    {
        public CloudStorageAccount GetCloudStorageAccount()
        {
            return CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("ConnectionString"));
        }
    }
}
