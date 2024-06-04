using DurableFunction.Services;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurableFunction
{
    public class BlobServiceBase
    {
        private readonly IContainerService _containerService;
        private string ContainerName { get; set; }

        public BlobServiceBase(IContainerService containerService, string containerName)
        {
            _containerService = containerService ?? throw new ArgumentException(nameof(BlobServiceBase));
            ContainerName = containerName;
        }

        protected ContainerResponse Execute()
        {
            var blobResult = _containerService.GetBlobs(ContainerName);
            var result = new ContainerResponse();

            foreach (var blob in blobResult)            
                result.NomeDocumentos.Add(blob.Name);
           
            return result;
        }
    }
}
