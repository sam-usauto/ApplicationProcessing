using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationProcessing.Service.TrustScienceService.DTOs.Configuration
{
    public class TrustScienceConfiguration
    {
        public bool IsProduction { get; set; }
        public string CorsList { get; set; }

        public SqlConnectionStrings ConnectionStringsUAT { get; set; }
        public SqlConnectionStrings ConnectionStringsPROD { get; set; }

        public TrustScienceSection TrustScienceConfigsUAT { get; set; }
        public TrustScienceSection TrustScienceConfigsPROD { get; set; }

        public int DapperDefaultTimeOut { get; set; }

        public string SsnDecryptUrl { get; set; }
        public string SsnEncryptUrl { get; set; }
    }
}
