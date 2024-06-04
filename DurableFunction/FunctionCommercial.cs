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
    public class FunctionCommercial : BlobServiceBase
    {
        private readonly IContainerService _containerService;

        public FunctionCommercial(IContainerService containerService) 
            : base(containerService, Global.COMMERCIAL_CONTAINER) { }

        [FunctionName(nameof(FunctionCommercial))]
        public Task<ContainerResponse> Run([ActivityTrigger] string name) => Task.FromResult(Execute());
    }
}