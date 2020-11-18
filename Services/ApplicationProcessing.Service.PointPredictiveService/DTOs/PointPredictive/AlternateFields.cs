using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationProcessing.Service.PointPredictiveService.DTOs.PointPredictive
{
    public class AlternateFields
    {
        public int primary_borrower_year_of_birth { get; set; }
        public int co_borrower_year_of_birth { get; set; }
        public string primary_borrower_credit_score_range { get; set; }
        public string co_borrower_credit_score_range { get; set; }
    }
}
