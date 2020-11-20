using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationProcessing.Service.TrustScienceService.DTOs.Responses
{
    public class ScoringReportResp
    {
        public string name { get; set; }
        public DateTime scoringDate { get; set; }
        public int score { get; set; }
        public string scoringRequestId { get; set; }
        public List<string> components { get; set; }
        public string clientCustomerId { get; set; }
        public string clientLoanReferenceId { get; set; }
        public string version { get; set; }
        public List<ScoreQualifier> scoreQualifier { get; set; }
        public List<ScoreReason> scoreReasons { get; set; }
        public Appendix appendix { get; set; }
    }
}
