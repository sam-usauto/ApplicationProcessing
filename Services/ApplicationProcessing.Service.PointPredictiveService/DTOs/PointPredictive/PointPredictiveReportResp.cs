using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationProcessing.Service.PointPredictiveService.DTOs.PointPredictive
{
    public class PointPredictiveReportResp
    {
        public PointPredictiveScoreResp PointPredictiveScoreResp { get; set; }
        public Exception Exception { get; set; }
        public string ExceptionInArea { get; set; }
        public string ExceptionFile { get; set; }
        public HttpGeneralResponse HttpRespone { get; set; }
        public string SavedReqRespID { get; set; }
        public string FraudScore { get; set; }
    }
}
