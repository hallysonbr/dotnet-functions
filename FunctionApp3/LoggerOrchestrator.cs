using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Messaging.EventHubs;
using FunctionApp3.Model;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FunctionApp3
{
    public static class LoggerOrchestrator
    {
        [FunctionName("LoggerOrchestrator")]
        public static async Task Run([EventHubTrigger("logapim", Connection = "AzureWebJobsStorage")] EventData[] events, ILogger log,
            [EventHub("logapim400", Connection = "AzureWebJobsStorage")] IAsyncCollector<ApimLoggerData> output400,
            [EventHub("logapim500", Connection = "AzureWebJobsStorage")] IAsyncCollector<ApimLoggerData> output500)
        {
            var exceptions = new List<Exception>();

            foreach (EventData eventData in events)
            {
                try
                {
                    var data = JsonConvert.DeserializeObject<ApimLoggerData>(Encoding.UTF8.GetString(eventData.EventBody));

                    var code = int.Parse(data.HttpStatusCode);

                    if (code >= 400 && code < 500)                
                        await output400.AddAsync(data);
                   
                    else if (code >= 500 && code < 600)                 
                        await output500.AddAsync(data);                
                }
                catch (Exception e)
                {
                    exceptions.Add(e);
                }
            }

            if (exceptions.Count > 1)
                throw new AggregateException(exceptions);

            if (exceptions.Count == 1)
                throw exceptions.Single();
        }
    }
}
