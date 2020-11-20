using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationProcessing.Service.TrustScienceService.DTOs.Responses
{
    public class FullScoringResponseBad
    {
        public List<Message> message { get; set; }
    }

    public class Message
    {
        public string keyword { get; set; }
        public string dataPath { get; set; }
        public string schemaPath { get; set; }
        public Params @params { get; set; }
        public string message { get; set; }
    }

    public class Params
    {
        public string pattern { get; set; }
        public int? limit { get; set; }
    }
}
