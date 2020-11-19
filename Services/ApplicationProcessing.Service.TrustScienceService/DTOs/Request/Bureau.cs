using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationProcessing.Service.TrustScienceService.DTOs.Request
{
    public class Bureau
    {
        public Bureau()
        {
            source = "";
            dataType = "";
            data = "";
        }

        public string source { get; set; }
        public string dataType { get; set; }
        public string data { get; set; }
    }
}
