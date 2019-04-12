using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Inventory.Function
{
    public interface ICloudStorageStrategy
    {
        CloudStorageAccount GetCloudStorageAccount(); 
    }
}
