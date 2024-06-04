using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using DurableFunction.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace DurableFunction
{
    public class FunctionEngineering
    {
        private readonly IContainerService _containerService;

        public FunctionEngineering(IContainerService containerService)
        {
            _containerService = containerService ?? throw new ArgumentException(nameof(FunctionEngineering));
        }

        [FunctionName(nameof(FunctionEngineering))]
        public Task<string> Run([ActivityTrigger] string name, ILogger log)
        {
            log.LogInformation("Saying hello to {name}.", name);

            var blobResult = _containerService.GetBlobs(Global.ENGINEERING_CONTAINER);

            log.LogInformation($"Found: { blobResult }");

            return Task.FromResult($"Hello {name}!");
        }
    }
}