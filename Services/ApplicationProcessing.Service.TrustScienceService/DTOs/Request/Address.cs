using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationProcessing.Service.TrustScienceService.DTOs.Request
{
    public class Address
    {
        public Address()
        {
            streetAddress = "";
            city = "";
            state = "";
            postalCode = "";
            country = "";
            residenceStatus = "";
            monthlyHousingCost = 0;
            isCurrentResidence = false;
            monthsAtResidence = 0;
        }


        public string streetAddress { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string postalCode { get; set; }
        public string country { get; set; }
        public string residenceStatus { get; set; }
        public int monthlyHousingCost { get; set; }
        public bool isCurrentResidence { get; set; }
        public int monthsAtResidence { get; set; }
    }
}
