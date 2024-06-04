using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Data.Tables;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace DurableFunction
{
    public class FunctionConclusion
    {
        [FunctionName("FunctionConclusion")]
        public async Task Run([ActivityTrigger] IDurableActivityContext context)
        {
            var info = context.GetInput<List<ContainerResponse>>();
            var tableClient = new TableClient(Environment.GetEnvironmentVariable("AzureWebJobsStorage"), "filesNamesArea");
            
            foreach (var names in info) 
            {
                foreach (var name in names.NomeDocumentos) 
                {
                    var tableEntity =  new TableEntity
                    {
                        { "PartitionKey", Guid.NewGuid().ToString() },
                        { "RowKey", Guid.NewGuid().ToString() },
                        { "FileName", name }
                    };

                    tableClient.AddEntity(tableEntity);
                }
            }        
        }
    }
}
