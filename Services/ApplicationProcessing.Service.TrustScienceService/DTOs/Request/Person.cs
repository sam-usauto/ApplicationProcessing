using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationProcessing.Service.TrustScienceService.DTOs.Request
{
    public class Person
    {
        public Person()
        {
            firstName = "";
            lastName = "";
            middleName = "";
            addresses = new List<Address>();
            birthday = "";
            //identifiers = new List<Identifier>();
            identifiers = new List<Ssn>();
        }


        public string firstName { get; set; }
        public string lastName { get; set; }
        public string middleName { get; set; }
        public List<Address> addresses { get; set; }
        public IEnumerable<Dictionary<string, string>> phones { get; set; }
        public IEnumerable<Dictionary<string, string>> emails { get; set; }
        public string birthday { get; set; }
        //public List<Identifier> identifiers { get; set; }
        public List<Ssn> identifiers { get; set; }
    }
}
