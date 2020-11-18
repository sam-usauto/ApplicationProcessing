using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationProcessing.Service.PointPredictiveService.DTOs.PointPredictive
{
    public class SavePointPredictiveResp
    {
        public Exception Exception { get; set; }
        public string ExceptionInArea { get; set; }
        public string ExceptionFile { get; set; }
        public bool Completed { get; set; }
        public string NewSaveId { get; set; }
    }
}
