using Azure;
using Azure.Storage.Blobs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurableFunction.Services
{
    public interface IContainerService
    {
        Pageable<BlobItem> GetBlobs(string containerName);
    }
}
