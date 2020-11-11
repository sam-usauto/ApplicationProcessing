using Common.DTOs.commo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.DTOs.Configurations.ApplicationWorker
{
    public class ApplicationProcessingConfig
    {
        public bool IsProduction { get; set; }

        public string CorsList { get; set; }

        public Recaptcha Recaptcha { get; set; }

        public SqlConnectionStrings ConnectionStringsUAT { get; set; }

        public SqlConnectionStrings ConnectionStringsPROD { get; set; }

    }
}
