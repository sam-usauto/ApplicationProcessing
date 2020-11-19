using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationProcessing.Service.TrustScienceService.DTOs.Request
{
    public class Custom
    {
        public Custom()
        {
            clientApplicationId = "";
            minDownPaymentPercent = "";
            nonNormalizedIncome = 0;
            applicantType = "primary";
        }

        public string clientApplicationId { get; set; }
        public string minDownPaymentPercent { get; set; }
        public int nonNormalizedIncome { get; set; }
        public string applicantType { get; set; }
    }
}
