using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationProcessing.Service.PointPredictiveService.DTOs.PointPredictive
{
    public class CreditInfo
    {
        public Int64 credit_score { get; set; }
        public Int64 time_in_file { get; set; }
        public Int64 debt_to_income_ratio { get; set; }
        public Int64 number_of_credit_inquiries_in_previous_two_weeks { get; set; }
        public Int64 highest_credit_limit_from_trades_in_good_standing { get; set; }
        public Int64 total_number_of_trade_lines { get; set; }
        public Int64 number_of_open_trade_lines { get; set; }
        public Int64 number_of_positive_auto_trades { get; set; }
        public Int64 number_of_mortgage_trade_lines { get; set; }
        public Int64 number_of_authorized_trade_lines { get; set; }
        public Int64 date_of_oldest_trade_line { get; set; }
    }
}
