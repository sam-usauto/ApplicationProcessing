using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationProcessing.Service.TrustScienceService.DTOs
{
    public class TrustScienceBatchItem
    {
        public string jurisdictionState { get; set; }       //(nvarchar(50), not null)
        public string firstName { get; set; }               //(nvarchar(50), null)
        public string middleName { get; set; }              //(nvarchar(50), null)
        public string lastName { get; set; }                //(nvarchar(50), null)
        public string streetAddress1 { get; set; }          //(nvarchar(252), null)
        public string city1 { get; set; }                   //(nvarchar(100), null)
        public string state1 { get; set; }                  //(nvarchar(50), not null)
        public string postalCode1 { get; set; }             //(nvarchar(5), null)
        public string country1 { get; set; }                //(varchar(3), not null)
        public int monthsAtResidence1 { get; set; }         //(int, not null)
        public string residenceStatus1 { get; set; }        //(nvarchar(10), null)
        public string isCurrentResidence { get; set; }      //(varchar(4), not null)
        public string streetAddress2 { get; set; }          //(nvarchar(100), null)
        public string city2 { get; set; }                   //(nvarchar(100), null)
        public string state2 { get; set; }                  //(nvarchar(50), not null)
        public string postalCode2 { get; set; }             //(nvarchar(5), not null)
        public string country2 { get; set; }                //(varchar(3), not null)
        public string residenceStatus2 { get; set; }        //(nvarchar(10), null)
        public string mobilePhone { get; set; }             //(nvarchar(4000), not null)
        public string homePhone { get; set; }               //(nvarchar(4000), not null)
        public string workPhone { get; set; }               //(nvarchar(4000), not null)
        public string email { get; set; }                   //(nvarchar(50), null)
        public string birthday { get; set; }                //(varchar(10), null)
        public int applicationID { get; set; }              //(int, not null)
        public string SSN { get; set; }                     //(nvarchar(255), not null)
        public string driverLicenseNumber { get; set; }
        public string issueState { get; set; }
        public string driverlicenseExpirationDate { get; set; }
        public string employerName { get; set; }            //(nvarchar(100), null)
        public string employerPhone { get; set; }           //(nvarchar(4000), not null)
        public string jobTitle { get; set; }                //(nvarchar(100), null)
        public string isCurrentlyEmployed { get; set; }     //(varchar(4), not null)
        public decimal monthlyIncomeNet { get; set; }       //(decimal(18,2), null)
        public int employmentMonthCount { get; set; }       //(int, not null)
        public string paymentsPerYear { get; set; }         //(varchar(2), not null)
        public string dateOriginated { get; set; }          //(varchar(10), null)
        public string term { get; set; }
        public int clientCustomerId { get; set; }           //(int, not null)
        public int clientLoanReferenceId { get; set; }      //(int, not null)
        public int clientApplicationId { get; set; }        //(int, not null)
        public decimal maxPurchasePrice { get; set; }
        public decimal maxMonthlyPayment { get; set; }
        public float minDownPaymentPercent { get; set; }
        public decimal principalAmount { get; set; }        //(decimal(18,2), null)
        public double annualInterestRate { get; set; }      //(float, null)
        public string lotName { get; set; }                 //(nvarchar(50), not null)
        public string streetAddress { get; set; }           //(varchar(50), null)
        public string city { get; set; }                    //(varchar(50), null)
        public string postalCode { get; set; }              //(varchar(5), null)
        public string state { get; set; }                   //(varchar(2), null)
        public string outputxml { get; set; }               //(nvarchar(max), null)
        public DateTime dateModified { get; set; }
        public int nonNormalizedIncome { get; set; }
        public string applicantType { get; set; }
    }
}
