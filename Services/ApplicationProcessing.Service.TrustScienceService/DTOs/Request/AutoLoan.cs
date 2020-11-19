using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationProcessing.Service.TrustScienceService.DTOs.Request
{
    public class AutoLoan
    {
        public AutoLoan()
        {
            //vehicle = new Vehicle();
            dealer = new Dealer();
        }

        //public Vehicle vehicle { get; set; }
        public Dealer dealer { get; set; }
    }
}
