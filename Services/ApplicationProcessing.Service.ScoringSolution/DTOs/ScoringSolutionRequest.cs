using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationProcessing.Service.ScoringSolution.DTOs
{
    public class ScoringSolutionRequest
    {
        public string ModelId { get; set; }
        public bool IsCoBuyer { get; set; }
        public string CoBuyerCode => IsCoBuyer ? "01" : "00";
        public string ApplicationID { get; set; }
        public string PaymentIncome { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string EncryptedSsn { get; set; }
        public string Ssn { get; set; }
        public string HouseNumber { get; set; }

        // TODO: What is that fields
        public string QuadRant { get; set; }
        public string StreetName { get; set; }
        public string StreetTypeName { get; set; }
        public string City { get; set; }
        public string StateAbbreviation { get; set; }
        public string PostalCode { get; set; }
        public string DateModified { get; set; }
        public string MonthsCurrentJob { get; set; }
        public string MonthsPreviousJob { get; set; }
        // TODO: What is that field
        public string CustomerSuffixTypeValue { get; set; }
        public string MonthsCurrentResidence { get; set; }
        public string MonthsPreviousResidence { get; set; }
        public string HousingTypeName { get; set; }
        public string NetIncome { get; set; }
        public string HousingPayment { get; set; }
        public string PeriodPaycheck { get; set; }
        public string OtherIncome { get; set; }
        public string EquifaxRawData { get; set; }
    }
}
