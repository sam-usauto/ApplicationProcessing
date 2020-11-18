using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationProcessing.Service.PointPredictiveService.DTOs.PointPredictive
{
    public class ValidationError
    {
        public ValidationErrorDetail Validation_error { get; set; }

        public class ValidationErrorDetail
        {
            public string Message { get; set; }
            public string Object { get; set; }
        }
    }
}
