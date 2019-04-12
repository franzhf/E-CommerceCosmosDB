using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using E_Commerce.Entities;

namespace E_Commerce.Inventory.Function
{
    public static class CreateProductHttpTrigger
    {


        [FunctionName("createproducthttptrigger")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");
            // Create a conection 
            CloudStorageAccount cloudStorageAccount = StorageAccountFactory.GetCloudStorageAccountInstance();
            CloudTableClient cloudTableClient = cloudStorageAccount.CreateCloudTableClient();
            CloudTable cloudTable = cloudTableClient.GetTableReference("products");
            cloudTable.CreateIfNotExists();

            // Get request body
            dynamic requestBody = await req.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<Product>(requestBody as string);

            ProductTable product = new ProductTable(data.Category);
            product.Name = data.Name;
            product.Price = data.Price;            
            if(!product.IsValid())
            {
                return req.CreateResponse(HttpStatusCode.BadRequest, "Data is not valid!");
            }
            TableOperation insertData = TableOperation.Insert(product);
            TableResult tableResult = await cloudTable.ExecuteAsync(insertData);

            if (tableResult.HttpStatusCode == 204 /*(int)HttpStatusCode.Accepted*/)
                return req.CreateResponse(HttpStatusCode.OK, "Product was inserted!");
            return req.CreateResponse(HttpStatusCode.NotAcceptable, "Insertion failed!");       
        }
    }
}
