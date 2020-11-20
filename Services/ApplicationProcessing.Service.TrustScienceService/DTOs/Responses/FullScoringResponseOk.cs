using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationProcessing.Service.TrustScienceService.DTOs.Responses
{
    public class FullScoringResponseOk
    {
        public string message { get; set; }
        public string requestId { get; set; }
        public int? LogID { get; set; }
        public int? TrustScienceLogID { get; set; }
    }
}
