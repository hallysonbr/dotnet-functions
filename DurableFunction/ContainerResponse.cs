using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurableFunction
{
    public class ContainerResponse
    {
        public List<string> NomeDocumentos { get; set; }

        public ContainerResponse() 
        {
            NomeDocumentos = new List<string>();
        }
    }
}
