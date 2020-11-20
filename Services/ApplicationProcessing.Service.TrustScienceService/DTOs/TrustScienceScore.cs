using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationProcessing.Service.TrustScienceService.DTOs
{
    public class TrustScienceScore
    {
        public int ID { get; set; }                     //(int, not null)
        public string RequestID { get; set; }           //(varchar(100), null)
        public int Score { get; set; }                  //(int, null)
        public string QualifierCode1 { get; set; }      //(varchar(20), null)
        public string QualifierCodeDescription1 { get; set; } //(varchar(100), null)
        public string QualifierCode2 { get; set; }      //(varchar(20), null)
        public string QualifierCodeDescription2 { get; set; } //(varchar(100), null)
        public string QualifierCode3 { get; set; }      //(varchar(20), null)
        public string QualifierCodeDescription3 { get; set; } //(varchar(100), null)
        public string QualifierCode4 { get; set; }      //(varchar(20), null)
        public string QualifierCodeDescription4 { get; set; } //(varchar(100), null)


        public string ScoreReasonCode1 { get; set; }      //(varchar(20), null)
        public string ScoreReasonDescription1 { get; set; } //(varchar(100), null)
        public string ScoreReasonCode2 { get; set; }      //(varchar(20), null)
        public string ScoreReasonDescription2 { get; set; } //(varchar(100), null)
        public string ScoreReasonCode3 { get; set; }      //(varchar(20), null)
        public string ScoreReasonDescription3 { get; set; } //(varchar(100), null)
        public string ScoreReasonCode4 { get; set; }      //(varchar(20), null)
        public string ScoreReasonDescription4 { get; set; } //(varchar(100), null)


        public string ScoringDetailsURL { get; set; }   //(varchar(250), null)
        public string Request { get; set; }             //(varchar(max), null)
        public string Response { get; set; }            //(varchar(max), null)
        public DateTime CreateDate { get; set; }        //(datetime, not null)
        public string CallStatus { get; set; }            //(varchar(max), null)
        public int CustomerID { get; set; }
        public int ApplicationID { get; set; }
    }
}
