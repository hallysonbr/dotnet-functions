using Azure;
using FunctionApp3.Model;
using FunctionApp3.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FunctionApp3
{
    public class LoggerBase
    {
        private readonly IDataService _dataService;

        public LoggerBase(IDataService dataService)
        {
            _dataService = dataService ?? throw new ArgumentNullException(nameof(LoggerBase));
        }

        protected async Task InsertData(HttpLogEntity entity)
        {
            await _dataService.InsertApimLogEntityAsync(entity);
        }

        protected Task<Pageable<EmployeeEntity>> GetEmployee()
        {
            return _dataService.GetEmployeeEntity();
        }

        protected async Task SendEmailToEmployee(Pageable<EmployeeEntity> data)
        {
            //Send e-mail through Azure Logic Apps

            var logicAppUrl = Environment.GetEnvironmentVariable("LogicAppsUrl");

            HttpClient client = new HttpClient();

            foreach (var item in data) 
            {
                var serializedObj = JsonSerializer.Serialize(item);
                await client.PostAsync(logicAppUrl, new StringContent(serializedObj, Encoding.UTF8, "application/json"));
            }
        }
    }
}
