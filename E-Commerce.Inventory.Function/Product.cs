using System;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage.Table;

namespace E_Commerce.Inventory.Function
{
    class ProductTable : TableEntity
    {
        public ProductTable(string partionKey)
        {
            this.RowKey = Guid.NewGuid().ToString();
            this.PartitionKey = partionKey;
            Price = 0;
        }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "price")]
        public double Price { get; set; }

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(Name) && Price == 0)
                return false;
            return true;
        }
    }

}
