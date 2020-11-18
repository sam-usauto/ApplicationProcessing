using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationProcessing.Service.PointPredictiveService.DTOs.PointPredictive
{
    public class ShortPointPredictiveReportResp
    {
        public ShortPointPredictiveReportResp()
        {
            HttpRespone = null;
            SavedReqRespID = string.Empty;
            FraudScore = string.Empty;
            FraudScore = string.Empty;
            CreditID = string.Empty;
            Fraud_epd_report_link = string.Empty;
            Errors = string.Empty;
            IsSuccessStatusCode = true;
            SavedReqRespID = string.Empty;
            Reason_code_1 = string.Empty;
            Reason_code_1_text = string.Empty;
            Reason_code_2 = string.Empty;
            Reason_code_2_text = string.Empty;
            Reason_code_3 = string.Empty;
            Reason_code_3_text = string.Empty;
            HasReportLink = false;


            DaysSinceLastCall = 300;
            Status = "";
            UWStatusId = 0;
        }

        public HttpGeneralResponse HttpRespone { get; set; }
        public string SavedReqRespID { get; set; }
        public string FraudScore { get; set; }
        public string CreditID { get; set; }
        public string Fraud_epd_report_link { get; set; }
        public string Errors { get; set; }
        public bool IsSuccessStatusCode { get; set; }
        public string Reason_code_1 { get; set; }
        public string Reason_code_1_text { get; set; }
        public string Reason_code_2 { get; set; }
        public string Reason_code_2_text { get; set; }
        public string Reason_code_3 { get; set; }
        public string Reason_code_3_text { get; set; }
        public bool HasReportLink { get; set; }

        public int DaysSinceLastCall { get; set; }
        public string Status { get; set; }
        public int UWStatusId { get; set; }
    }
}
