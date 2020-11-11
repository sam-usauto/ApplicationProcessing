using LoggerMicroService.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoggerMicroService
{
    [Serializable]
    public class SerilogConfig
    {
        public bool IsProduction { get; set; }

        public string CorsList { get; set; }

        public bool EnableDiagnostics { get; set; }
        public bool EnablePerf { get; set; }

        public FileLocation FileLocation { get; set; }

        public SqlConnectionStrings ConnectionStringsUAT { get; set; }

        public SqlConnectionStrings ConnectionStringsPROD { get; set; }



    }
}
