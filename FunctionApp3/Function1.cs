using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;

namespace FunctionApp3
{
    public static class Function1
    {
        [FunctionName("Function1")]
        [return: EventHub("logapim", Connection = "AzureWebJobsStorage")]
        public static async Task<string> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "Logger")] HttpRequestMessage req,
            ILogger log)
        {
            log.LogInformation("STEP: ApiLoggerTrigger");
            
            var loggerData = await req.Content.ReadAsStringAsync();

            return loggerData;
        }
    }
}
