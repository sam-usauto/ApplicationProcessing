using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationProcessing.Service.ScoringSolution.DTOs.Configuration
{
    public class ScoringSolutionSection
    {
        public string CreateFullScoringRequestUrl { get; set; }
        public string GetScoringReportUrl { get; set; }
        public string ApiKey { get; set; }
    }
}
