using System;
using System.IO;
using System.Xml;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace FunctionApp1
{
    [StorageAccount("AzureWebJobsStorage")]
    public static class Function1
    {
        [FunctionName("Function1")]
        [return: Queue("queueprocess")]
        public static string Run([BlobTrigger("tobeprocess/{name}", Connection = "AzureWebJobsStorage")]
        Stream myBlob, string name, TraceWriter log)
        {
            log.Info($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");

            var currentDoc = ProcessDocument(myBlob, name);

            return JsonConvert.SerializeObject(currentDoc);
        }

        private static DocFile ProcessDocument(Stream blob, string name)
        {
            XmlDocument doc = new XmlDocument();
            using (Stream stream = blob)
            {
                using (XmlReader reader = XmlReader.Create(stream)) 
                {
                    if(stream.Position > 0)
                        stream.Position = 0;

                    doc.Load(stream);
                }
            }

            var cpfFromXml = doc.SelectSingleNode("nota_fiscal/cpf").InnerText;
            var nameFromXml = doc.SelectSingleNode("nota_fiscal/nome").InnerText;

            var docFile = new DocFile
            {
                FileName = name,
                FileSize = blob.Length,
                PersonName = nameFromXml,
                PersonId = cpfFromXml,
                ProcessDate = DateTime.UtcNow
            };

            return docFile;
        }
    }
}
