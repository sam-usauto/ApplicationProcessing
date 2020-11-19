using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationProcessing.Service.TrustScienceService.DTOs.Request
{
    public class CreateFullScoringRequest
    {
        public CreateFullScoringRequest()
        {
            schemaVersion = "";
            scoring = new Scoring();
            control = new Control();
            user = "";
            source = "";
            batchName = "";
            sendMobileInvite = false;
            data = new Data();
        }

        public string schemaVersion { get; set; }
        public Scoring scoring { get; set; }
        public Control control { get; set; }
        public string user { get; set; }
        public string source { get; set; }
        public string batchName { get; set; }
        public bool sendMobileInvite { get; set; }
        public Data data { get; set; }
    }
}
