using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationProcessing.Service.PointPredictiveService.DTOs.PointPredictive
{
    public class TokenResp
    {
        public string expires { get; set; }
        public string access_token { get; set; }
    }
}
