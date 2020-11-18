using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationProcessing.Service.PointPredictiveService.DTOs.PointPredictive
{
    public class VehicleInfo
    {
        public Int64 sale_price { get; set; }
        public Int64 year_of_manufacture { get; set; }
        public string make { get; set; }
        public string model { get; set; }
        public string vin { get; set; }
        public string new_or_used { get; set; }
        public Int64 mileage { get; set; }
    }
}
