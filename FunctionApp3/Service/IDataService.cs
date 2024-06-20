using Azure;
using FunctionApp3.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionApp3.Service
{
    public interface IDataService
    {
        Task InsertApimLogEntityAsync(HttpLogEntity entity);
        Task<Pageable<EmployeeEntity>> GetEmployeeEntity();
        Task InsertEmployeeEntityAsync(EmployeeEntity entity);
    }
}
