using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationProcessing.Service.PointPredictiveService.DTOs.PointPredictive
{
    public class PrimaryBorrower
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
        public Int64 ssn { get; set; }
        public string rentown { get; set; }
        public string rentmortgage { get; set; }
        //public Int64 rentmortgage { get; set; }
        public Int64 months_at_residence { get; set; }
        public string occupation { get; set; }
        public Int64 annual_income { get; set; }
        public string self_employed { get; set; }
        public string employer_name { get; set; }
        public string employer_street_address { get; set; }
        public string employer_city { get; set; }
        public string employer_state { get; set; }
        public string employer_zip { get; set; }
        public Int64 employer_phone { get; set; }
        public Int64 months_at_employer { get; set; }
        public string other_bank_relationships { get; set; }
        public Int64 customer_since_date { get; set; }
        public string channel { get; set; }
    }
}
