using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurableFunction.Services
{
    public class ContainerService : IContainerService
    {
        public Pageable<BlobItem> GetBlobs(string containerName)
        {
            try
            {
                var blobContaineClient = new BlobContainerClient
                    (Environment.GetEnvironmentVariable("AzureWebJobsStorage"),
                    containerName);

                var result = blobContaineClient.GetBlobs(BlobTraits.None, BlobStates.None);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
