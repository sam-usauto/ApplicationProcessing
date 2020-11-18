using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationProcessing.Service.PointPredictiveService.DTOs.PointPredictive
{
    public class AdministrativeFields
    {
        // drop the field per email from PP
        // public string lender_identifier { get; set; }
        public string application_identifier { get; set; }
        public Int64 application_date { get; set; }
        public string application_status { get; set; }
        public string dealer_identifier { get; set; }
    }
}
