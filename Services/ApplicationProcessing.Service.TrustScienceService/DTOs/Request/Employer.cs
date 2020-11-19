using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationProcessing.Service.TrustScienceService.DTOs.Request
{
    public class Employer
    {
        public Employer()
        {
            name = "";
            //address = new Address2();
            phone = "";
        }

        public string name { get; set; }
        // NOTE: We do not collect employee address information
        //public Address2 address { get; set; }
        public string phone { get; set; }
    }
}
