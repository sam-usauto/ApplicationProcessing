using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationProcessing.Service.TrustScienceService.DTOs.Request
{
    public class Dealer
    {
        public Dealer()
        {
            name = "";
            type = "";
            address = new Address3();
        }

        public string name { get; set; }
        public string type { get; set; }
        public Address3 address { get; set; }
    }
}
