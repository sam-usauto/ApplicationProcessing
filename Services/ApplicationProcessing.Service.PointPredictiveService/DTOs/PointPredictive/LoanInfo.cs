using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationProcessing.Service.PointPredictiveService.DTOs.PointPredictive
{
    public class LoanInfo
    {
        public Int64 loan_amount { get; set; }
        public Int64 total_down_payment { get; set; }
        public Int64 cash_down_payment { get; set; }
        public Int64 term { get; set; }
        public Int64 payment_to_income_ratio { get; set; }
    }
}
