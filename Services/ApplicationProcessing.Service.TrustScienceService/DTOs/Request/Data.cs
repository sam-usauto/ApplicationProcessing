using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationProcessing.Service.TrustScienceService.DTOs.Request
{
    public class Data
    {
        public Data()
        {
            person = new Person();
            currentEmployment = new CurrentEmployment();
            loanApplication = new LoanApplication();
            autoLoan = new AutoLoan();
            bureau = new Bureau();
        }


        public Person person { get; set; }
        public CurrentEmployment currentEmployment { get; set; }
        public LoanApplication loanApplication { get; set; }
        public AutoLoan autoLoan { get; set; }
        public Bureau bureau { get; set; }
    }
}
