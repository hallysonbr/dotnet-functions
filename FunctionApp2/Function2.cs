using System;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FunctionApp2
{
    public static class Function2
    {
        [FunctionName("Function2")]
        [return: Table("docsinfo", Connection = "AzureWebJobsStorage")]
        public static async Task<DocEntity> Run([QueueTrigger("queueprocess", Connection = "AzureWebJobsStorage")] 
        string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");

            var queueItem = JsonConvert.DeserializeObject<DocFile>(myQueueItem);

            var blobClient = new BlobClient(Environment.GetEnvironmentVariable("AzureWebJobsStorage"),
                                            "tobeprocess", queueItem.FileName);
            var currentBlob = await blobClient.DownloadStreamingAsync();

            var blobContainerClient = new BlobContainerClient(Environment.GetEnvironmentVariable("AzureWebJobsStorage"),
                                                              "processdone");
            await blobContainerClient.UploadBlobAsync(queueItem.FileName, currentBlob.Value.Content);
            await blobClient.DeleteIfExistsAsync();

            return new()
            {
                PartitionKey = "nota_fiscal",
                RowKey = Guid.NewGuid().ToString(),
                PersonId = queueItem.PersonId,
                PersonName = queueItem.PersonName,
            };
        }
    }
}