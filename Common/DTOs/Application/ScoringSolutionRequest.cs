using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DTOs.Application
{
    public class ScoringSolutionRequest
    {
        public string ModelId { get; set; }
        public bool IsCoBuyer { get; set; }

        public string CoBuyerCode => IsCoBuyer ? "01" : "00";
        public int ApplicationID { get; set; }
        public string PaymentIncome { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public string CustomerMiddleName { get; set; }
        public string CustomerSSN { get; set; }
        public string HouseNumber { get; set; }
        public string QuadRant { get; set; }
        public string StreetName { get; set; }
        public string StreetTypeName { get; set; }
        public string City { get; set; }
        public string StateAbbreviation { get; set; }
        public string PostalCode { get; set; }
        public DateTime DateModified { get; set; }

        public string DateModifiedFormatted => DateModified.ToString("yyyyddMM");

        public int MonthsCurrentJob { get; set; }
        public int? MonthsPreviousJob { get; set; }
        public string CustomerSuffixTypeValue { get; set; }
        public int MonthsCurrentResidence { get; set; }
        public int? MonthsPreviousResidence { get; set; }
        public string HousingTypeName { get; set; }
        public decimal NetIncome { get; set; }
        public decimal? HousingPayment { get; set; }

        public string EquifaxRawData { get; set; }

        // not sure about the field bellow [Sam]
        public int? bureau { get; set; }

        public decimal? PeriodPaycheck { get; set; }
        public int SalaryTypeID { get; set; }
        public decimal? OtherIncome { get; set; }

    }
}
