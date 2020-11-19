using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationProcessing.Service.TrustScienceService.DTOs.Request
{
    public class Jurisdiction
    {
        public Jurisdiction()
        {
            country = "";
            state = "";
        }

        public string country { get; set; }
        public string state { get; set; }
    }
}
