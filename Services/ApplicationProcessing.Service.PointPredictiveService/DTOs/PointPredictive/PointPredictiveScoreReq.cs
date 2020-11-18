using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationProcessing.Service.PointPredictiveService.DTOs.PointPredictive
{
    public class PointPredictiveScoreReq
    {
        public AdministrativeFields administrative_fields { get; set; }
        public PrimaryBorrower primary_borrower { get; set; }
        public LoanInfo loan_information { get; set; }
        public CreditInfo credit_information { get; set; }
        public VehicleInfo vehicle_information { get; set; }
        public CoBorrower co_borrower { get; set; }
        public AlternateFields alternate_fields { get; set; }
        public UserDefinedFields user_defined_fields { get; set; }
    }
}
