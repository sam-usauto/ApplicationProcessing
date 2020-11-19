using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationProcessing.Service.TrustScienceService.DTOs.Request
{
    public class LoanApplication
    {
        public LoanApplication()
        {
            dateOriginated = "";
            clientCustomerId = "";
            clientLoanReferenceId = "";
            principalAmount = 0;
            annualInterestRate = 0;
            paymentsPerYear = 0;
            totalNumberOfPayments = 0;
            custom = new List<Custom>();

            paymentAmount = 0;
        }

        public string dateOriginated { get; set; }
        public string clientCustomerId { get; set; }
        public string clientLoanReferenceId { get; set; }
        public double principalAmount { get; set; }
        public double annualInterestRate { get; set; }
        public int paymentsPerYear { get; set; }
        public int totalNumberOfPayments { get; set; }
        public int termMonth { get; set; }
        public List<Custom> custom { get; set; }
        public int paymentAmount { get; set; }
    }
}
