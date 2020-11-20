using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationProcessing.Service.TrustScienceService.DTOs
{
    public class ProcessingResult
    {
        public ProcessingResult()
        {
            TotalItemCount = 0;
            SuccessfulItemCount = 0;
            FailedItemCount = 0;
            ProcessingErrorItemCount = 0;
        }

        public DateTime LastItemDateTime { get; set; }
        public int TotalItemCount { get; set; }
        public int SuccessfulItemCount { get; set; }
        public int FailedItemCount { get; set; }
        public int ProcessingErrorItemCount { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
    }
}
