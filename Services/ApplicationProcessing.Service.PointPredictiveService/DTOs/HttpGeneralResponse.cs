using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationProcessing.Service.PointPredictiveService.DTOs
{
    public class HttpGeneralResponse
    {
        public HttpGeneralResponse(string ServiceTokenURL, string Verb)
        {
            StatusCode = -1;
            ReasonPhrase = "";
            IsSuccessStatusCode = false;
            Errors = new List<string>();
            Url = ServiceTokenURL;
            HttpVerb = Verb;
            ResponseData = "";
            RequestData = "";
        }

        public int StatusCode { get; set; }
        public string ReasonPhrase { get; set; }
        public bool IsSuccessStatusCode { get; set; }

        public List<string> Errors { get; set; }
        public string Url { get; set; }
        public String HttpVerb { get; set; }

        public string ResponseData { get; set; }
        public string RequestData { get; set; }
    }
}
