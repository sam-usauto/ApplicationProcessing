using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationProcessing.Service.PointPredictiveService.DTOs.PointPredictive
{
    public class PointPredictiveScoreResp
    {
        public PointPredictiveScoreResp()
        {
            Administrative_fields = new Administrative_fields();
            Application_fraud_information = new Application_fraud_information();
            Dealer_risk_information = new Dealer_risk_information();
            Report_links = new Report_links();

            DaysSinceLastCall = 0;
            Status = String.Empty;
            UWStatusId = 0;
        }

        public Administrative_fields Administrative_fields { get; set; }
        public Application_fraud_information Application_fraud_information { get; set; }
        public Dealer_risk_information Dealer_risk_information { get; set; }
        public Report_links Report_links { get; set; }

        public int DaysSinceLastCall { get; set; }
        public string Status { get; set; }
        public int UWStatusId { get; set; }

    }

    public class Administrative_fields
    {
        public string application_identifier { get; set; }
        public string interface_version { get; set; }
        //public string lender_identifier { get; set; }
    }

    public class Application_fraud_information
    {
        public string fraud_score { get; set; }
        public string reason_code_1 { get; set; }
        public string reason_code_1_text { get; set; }
        public string reason_code_2 { get; set; }
        public string reason_code_2_text { get; set; }
        public string reason_code_3 { get; set; }
        public string reason_code_3_text { get; set; }
    }

    public class Dealer_risk_information
    {
        public string dealer_risk_score { get; set; }
        public string reason_code_1 { get; set; }
        public string reason_code_1_text { get; set; }
        public string reason_code_2 { get; set; }
        public string reason_code_2_text { get; set; }
        public string reason_code_3 { get; set; }
        public string reason_code_3_text { get; set; }
    }

    public class Report_links
    {
        public string dealer_risk_report_link { get; set; }
        public string fraud_epd_report_link { get; set; }
    }
}
}
