using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoggerMicroService.Configuration
{
    public class FileLocation
    {
        public string DiagnosticLoggerLocation { get; set; }
        public string ErrorLoggerLocation { get; set; }
        public string PerfLoggerLocation { get; set; }
        public string UsageLoggerLocation { get; set; }

        public string ApplicationLoggerLocation { get; set; }
    }
}
