using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationProcessing.Service.TrustScienceService.DTOs.Request
{
    public class Address3
    {
        public Address3()
        {
            streetAddress = "";
            city = "";
            postalCode = "";
            state = "";
            country = "";
        }

        public string streetAddress { get; set; }
        public string city { get; set; }
        public string postalCode { get; set; }
        public string state { get; set; }
        public string country { get; set; }
    }
}
