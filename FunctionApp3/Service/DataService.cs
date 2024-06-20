using Azure;
using Azure.Data.Tables;
using FunctionApp3.Model;
using Microsoft.AspNetCore.Mvc.Formatters.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionApp3.Service
{
    public class DataService : IDataService
    {
        public Task<Pageable<EmployeeEntity>> GetEmployeeEntity()
        {
            var tableClient = TableClient("EmployeeSupport");

            var result = tableClient.Query<EmployeeEntity>(e => e.IsSuppport);

            return Task.FromResult(result);
        }

        public async Task InsertApimLogEntityAsync(HttpLogEntity entity)
        {
            var tableClient = TableClient("DataLogginApim");

            try
            {
                await tableClient.UpdateEntityAsync(entity, entity.ETag);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task InsertEmployeeEntityAsync(EmployeeEntity entity)
        {
            var tableClient = TableClient("EmployeeSupport");

            try
            {
                await tableClient.UpdateEntityAsync(entity, entity.ETag);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private TableClient TableClient(string tableName) 
        {
            var tableClient = new TableClient(Environment.GetEnvironmentVariable("AzureWebJobsStorage"), tableName);

            tableClient.CreateIfNotExists();

            return tableClient;            
        }
    }
}
