using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationProcessing.Service.TrustScienceService.DTOs.Request
{
    public class Ssn
    {
        public string type { get; set; }
        public string value { get; set; }
        public string issuer { get; set; }
    }
}
