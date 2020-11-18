using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationProcessing.Service.PointPredictiveService.DTOs
{
    public class PointPredictiveConfigSection
    {
        public string ApplicationLenderIdentifier { get; set; }
        public string BaseURL { get; set; }
        public string UserID { get; set; }
        public string Password { get; set; }
        public string PdfLinkUserName { get; set; }
        public string PdfLinkPassword { get; set; }
        public string SsnDecryptUrl { get; set; }
    }
}
