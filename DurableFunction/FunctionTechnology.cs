using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DurableFunction.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace DurableFunction
{
    public class FunctionTechnology : BlobServiceBase
    {
        private readonly IContainerService _containerService;

        public FunctionTechnology(IContainerService containerService)
            : base(containerService, Global.TECHNOLOGY_CONTAINER) { }
        

        [FunctionName(nameof(FunctionTechnology))]
        public Task<ContainerResponse> Run([ActivityTrigger] string name) => Task.FromResult(Execute());
        
    }
}