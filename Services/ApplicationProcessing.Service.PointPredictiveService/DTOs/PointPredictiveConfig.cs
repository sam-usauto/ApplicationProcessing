using Common.DTOs.commo;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationProcessing.Service.PointPredictiveService.DTOs
{
    public class PointPredictiveConfig
    {
        public bool IsProduction { get; set; }

        public string CorsList { get; set; }

        public SqlConnectionStrings ConnectionStringsUAT { get; set; }

        public SqlConnectionStrings ConnectionStringsPROD { get; set; }

        public string SsnDecryptUrl { get; set; }

        public string SsnEncryptUrl { get; set; }

        public int DapperDefaultTimeOut { get; set; }

    }
}
