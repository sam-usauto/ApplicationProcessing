using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DTOs.Application
{
    public class ApplicationStepInput
    {
        public int ApplicationID { get; set; }
        public string UnprotectedSsn { get; set; }
        public int LogId { get; set; }
        public int UserID { get; set; }
        public int ApplicationFlowStepResultID { get; set; }
    }
}
