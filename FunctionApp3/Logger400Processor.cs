using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.EventHubs;
using FunctionApp3.Model;
using FunctionApp3.Service;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace FunctionApp3
{
    public class Logger400Processor : LoggerBase
    {
        public Logger400Processor(IDataService dataService) : base(dataService) { }

        [FunctionName("Logger400Processor")]
        public async Task Run([EventHubTrigger("logapim400", Connection = "EventHubConnectionString")] EventData[] events, ILogger log)
        {
            log.LogInformation($"STEP: {nameof(Logger400Processor)}");

            var exceptions = new List<Exception>();

            foreach (EventData eventData in events)
            {
                try
                {
                   var dataFromEventHub = eventData.EventBody.ToObjectFromJson<ApimLoggerData>();

                    var entity = new HttpLogEntity
                    {
                        PartitionKey = Guid.NewGuid().ToString(),
                        RowKey = Guid.NewGuid().ToString(),
                        HttpStatus = dataFromEventHub.HttpStatusCode,
                    };

                    await this.InsertData(entity);

                    var employeeSupport = await this.GetEmployee();

                    await SendEmailToEmployee(employeeSupport);
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
