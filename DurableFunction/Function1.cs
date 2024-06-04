using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace DurableFunction
{
    public class Function1
    {
        [FunctionName("Function1")]
        public async Task RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var tasks = new List<Task<ContainerResponse>>
            {
                context.CallActivityAsync<ContainerResponse>(nameof(FunctionTechnology), "FunctionTechnology"),
                context.CallActivityAsync<ContainerResponse>(nameof(FunctionCommercial), "FunctionCommercial"),
                context.CallActivityAsync<ContainerResponse>(nameof(FunctionEngineering), "FunctionEngineering")
            };

            await Task.WhenAll(tasks);

            var resultFromFunctions = tasks.Select(x => x.Result).ToList();

            await context.CallActivityAsync<string>(nameof(FunctionConclusion), resultFromFunctions);
        }

        [FunctionName("Function1_HttpStart")]
        public async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        
        {
            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync("Function1", null);

            log.LogInformation("Started orchestration with ID = '{instanceId}'.", instanceId);

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}