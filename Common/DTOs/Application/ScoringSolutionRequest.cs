using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DTOs.Application
{
    public class ScoringSolutionRequest
    {
        //TODO: Varify that we getting the correct data... Do we have to calculate the Montly payment? Which payment is what field

        public string ModelId { get; set; }
        public string CoBuyerCode { get; set; }
        public string ApplicationID { get; set; }
        public string PaymentIncome { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string EncryptedSsn { get; set; }
        public string Ssn { get; set; }
        public string HouseNumber { get; set; }
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
