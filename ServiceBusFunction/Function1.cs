using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace ServiceBusFunction
{
    public class Function1
    {
        [FunctionName("Function1")]
        public async Task Run([ServiceBusTrigger("queue-filestreaming", Connection = "azureconnectionstring")]
        string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");

            var bytes = Convert.FromBase64String(myQueueItem);
            var content = new MemoryStream(bytes);

            try
            {
                var container = new BlobContainerClient(Environment.GetEnvironmentVariable("AzureWebJobsStorage"),
                                                        "content-files");

                var blobName = $"file-{ DateTime.UtcNow.ToString("dd-MM-yyyy_HH-mm:ss") }";
                await container.UploadBlobAsync(blobName, content);
                log.LogInformation("Arquivo inserido com sucesso!");
            }
            catch (Exception ex)
            {
              log.LogCritical($"Error: {ex}");
            }
        }
    }
}
