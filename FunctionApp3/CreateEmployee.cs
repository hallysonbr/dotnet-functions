using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using FunctionApp3.Model;
using FunctionApp3.Service;

namespace FunctionApp3
{
    public class CreateEmployee
    {
        private readonly IDataService _dataService;

        public CreateEmployee(IDataService dataService)
        {
            _dataService = dataService;
        }

        [FunctionName("CreateEmployee")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "Create")] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            var content = JsonConvert.DeserializeObject<EmployeeEntity>(requestBody);
            content.PartitionKey = Guid.NewGuid().ToString();
            content.RowKey = Guid.NewGuid().ToString();

            await _dataService.InsertEmployeeEntityAsync(content);
            
           return new OkResult();
        }
    }
}
