using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationProcessing.Service.PointPredictiveService.DTOs
{
    public class UsAutoApp
    {
        public string InterfaceVersion { get; set; }
        public string AccountIdentifier { get; set; }
        public string LenderIdentifier { get; set; }
        public int ApplicationIdentifier { get; set; }
        public string ApplicationDate { get; set; }
        public string ApplicationStatus { get; set; }
        public string DealerIdentifier { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string HomePhoneNumber { get; set; }
        public string WorkPhoneNumber { get; set; }
        public string CellPhoneNumber { get; set; }
        public string Email { get; set; }
        public string DateofBirth { get; set; }
        public string SSN { get; set; }
        public string RentOwn { get; set; }
        public string RentMortgage { get; set; }
        public int MonthsatResidence { get; set; }
        public string Occupation { get; set; }
        public decimal? AnnualIncome { get; set; }
        public string SelfEmployed { get; set; }
        public string EmployerName { get; set; }
        public string EmployerStreetAddress { get; set; }
        public string EmployerCity { get; set; }
        public string EmployerState { get; set; }
        public string EmployerZIP { get; set; }
        public string EmployerPhone { get; set; }
        public int MonthsatEmployer { get; set; }
        public string OtherBankRelationships { get; set; }
        public string CustomerSinceDate { get; set; }
        public string Channel { get; set; }
        public decimal LoanAmount { get; set; }
        public decimal TotalDownPayment { get; set; }
        public decimal CashDownPayment { get; set; }
        public string Term { get; set; }
        public float PaymentToIncomeRatio { get; set; }
        public string CreditScore { get; set; }
        public string TimeinFile { get; set; }
        public string DebtToIncomeRatio { get; set; }
        public string NumberofCreditInquiriesinprevioustwoweeks { get; set; }
        public int? HighestCreditLimitfromTradesinGoodStanding { get; set; }
        public int? TotalNumberofTradeLines { get; set; }
        public int? NumberofOpenTradeLines { get; set; }
        public int? NumberofPositiveAutoTrades { get; set; }
        public int? NumberofMortgageTradeLines { get; set; }
        public string NumberofAuthorizedTradeLines { get; set; }
        public string DateofOldestTradeLine { get; set; }
        public decimal SalePrice { get; set; }
        public string YearofManufacture { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string VIN { get; set; }
        public string NeworUsed { get; set; }
        public string Mileage { get; set; }
        public string CobFirstName { get; set; }
        public string CobLastName { get; set; }
        public string CobStreetAddress { get; set; }
        public string CobCity { get; set; }
        public string CobState { get; set; }
        public string CobZip { get; set; }
        public string CobHomePhoneNumber { get; set; }
        public string CobWorkPhoneNumber { get; set; }
        public string CobCellPhoneNumber { get; set; }
        public string CobEmail { get; set; }
        public string CobDateofBirth { get; set; }
        public string CobAnnualIncome { get; set; }
        public string CobRelationship { get; set; }
        public string CobCreditScore { get; set; }
        public string PrimaryBorrowerYearofBirth { get; set; }
        public string CoBorrowerYearofBirth { get; set; }
        public string PrimaryBorrowerCreditScoreRange { get; set; }
        public string CoBorrowerCreditScoreRange { get; set; }
        public int UDF1 { get; set; }
        public int UDF2 { get; set; }
        public int UDF3 { get; set; }
        public int UDF4 { get; set; }
        public int UDF5 { get; set; }
        public int UDF6 { get; set; }
    }
}
