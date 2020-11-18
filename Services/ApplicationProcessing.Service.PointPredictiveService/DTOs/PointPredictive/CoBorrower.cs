using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationProcessing.Service.PointPredictiveService.DTOs.PointPredictive
{
    public class CoBorrower
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string street_address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string country { get; set; }
        public Int64 home_phone_number { get; set; }
        public Int64 work_phone_number { get; set; }
        public Int64 cell_phone_number { get; set; }
        public string e_mail_address { get; set; }
        public Int64 date_of_birth { get; set; }
        public Int64 annual_income { get; set; }
        public string relationship { get; set; }
        public Int64 credit_score { get; set; }
    }
}
