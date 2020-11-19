using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationProcessing.Service.TrustScienceService.DTOs.Request
{
    public class CurrentEmployment
    {
        public CurrentEmployment()
        {
            employer = new Employer();
            jobTitle = "";
            isCurrentlyEmployed = false;
            monthlyIncomeNet = 0;
            employmentMonthCount = 0;
            paymentsPerYear = 0;
        }

        public Employer employer { get; set; }
        public string jobTitle { get; set; }
        public bool isCurrentlyEmployed { get; set; }
        public int monthlyIncomeNet { get; set; }
        public int employmentMonthCount { get; set; }
        public int paymentsPerYear { get; set; }
    }
}

