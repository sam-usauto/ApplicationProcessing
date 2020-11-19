using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationProcessing.Service.TrustScienceService.DTOs.Request
{
    public class Scoring
    {
        public Scoring()
        {
            type = "";
            useCase = "";
            package = "";
            jurisdiction = new Jurisdiction();
            generateReport = false;
        }

        public string type { get; set; }
        public string useCase { get; set; }
        public string package { get; set; }
        public Jurisdiction jurisdiction { get; set; }
        public bool generateReport { get; set; }
    }
}

