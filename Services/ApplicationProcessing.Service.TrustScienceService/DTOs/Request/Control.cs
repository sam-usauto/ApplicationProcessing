using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationProcessing.Service.TrustScienceService.DTOs.Request
{
    public class Control
    {
        public Control()
        {
            tag = new List<string>();
        }

        public List<string> tag { get; set; }
    }
}
