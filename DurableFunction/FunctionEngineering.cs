using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace DurableFunction
{
    public class FunctionEngineering
    {
        [FunctionName(nameof(FunctionEngineering))]
        public Task<string> Run([ActivityTrigger] string name, ILogger log)
        {
            log.LogInformation("Saying hello to {name}.", name);
            return Task.FromResult($"Hello {name}!");
        }
    }
}