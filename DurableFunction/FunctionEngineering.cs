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
    public class FunctionEngineering : BlobServiceBase
    {
        private readonly IContainerService _containerService;

        public FunctionEngineering(IContainerService containerService)
            : base(containerService, Global.ENGINEERING_CONTAINER) { }

        [FunctionName(nameof(FunctionEngineering))]
        public Task<ContainerResponse> Run([ActivityTrigger] string name) => Task.FromResult(Execute());
    }
}