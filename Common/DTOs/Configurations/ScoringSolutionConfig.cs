using Common.DTOs.commo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DTOs.Configurations
{
    public class ScoringSolutionConfig
    {
        public bool IsProduction { get; set; }

        public string CorsList { get; set; }

        public SqlConnectionStrings ConnectionStringsUAT { get; set; }

        public SqlConnectionStrings ConnectionStringsPROD { get; set; }

        public string SsnDecryptUrl { get; set; }

        public string SsnEncryptUrl { get; set; }
    }
}


