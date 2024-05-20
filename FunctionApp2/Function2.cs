using System;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace FunctionApp2
{
    public static class Function2
    {
        [FunctionName("Function2")]
        public static async Task Run([QueueTrigger("queueprocess", Connection = "AzureWebJobsStorage")] string myQueueItem, TraceWriter log)
        {
            log.Info($"C# Queue trigger function processed: {myQueueItem}");

            var queueItem = JsonConvert.DeserializeObject<DocFile>(myQueueItem);

            var blobClient = new BlobClient(Environment.GetEnvironmentVariable("AzureWebJobsStorage"),
                                            "tobeprocess", queueItem.FileName);
            var currentBlob = await blobClient.DownloadStreamingAsync();

            var blobContainerClient = new BlobContainerClient(Environment.GetEnvironmentVariable("AzureWebJobsStorage"),
                                                              "processdone");
            await blobContainerClient.UploadBlobAsync(queueItem.FileName, currentBlob.Value.Content);
            await blobClient.DeleteIfExistsAsync();
        }
    }
}