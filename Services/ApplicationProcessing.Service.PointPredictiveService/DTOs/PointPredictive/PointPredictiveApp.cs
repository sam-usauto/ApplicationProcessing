using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationProcessing.Service.PointPredictiveService.DTOs.PointPredictive
{
    public class PointPredictiveApp
    {
        public PointPredictiveApp()
        {
            AdministrativeFields = new AdministrativeFields();
            PrimaryBorrower = new PrimaryBorrower();
            LoanInfo = new LoanInfo();
            CreditInfo = new CreditInfo();
            VehicleInfo = new VehicleInfo();
            CoBorrower = new CoBorrower();
            AlternateFields = new AlternateFields();
            UserDefinedFields = new UserDefinedFields();
        }

        public AdministrativeFields AdministrativeFields { get; set; }
        public PrimaryBorrower PrimaryBorrower { get; set; }
        public LoanInfo LoanInfo { get; set; }
        public CreditInfo CreditInfo { get; set; }
        public VehicleInfo VehicleInfo { get; set; }
        public CoBorrower CoBorrower { get; set; }
        public AlternateFields AlternateFields { get; set; }
        public UserDefinedFields UserDefinedFields { get; set; }
    }
}
