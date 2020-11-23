using Common.DTOs.commo;

namespace ApplicationProcessing.Service.ScoringSolution.DTOs.Configuration
{
    public class ScoringSolutionConfiguration
    {
        public bool IsProduction { get; set; }
        public string CorsList { get; set; }
        public bool ReturnShortResp { get; set; }

        public SqlConnectionStrings ConnectionStringsUAT { get; set; }
        public SqlConnectionStrings ConnectionStringsPROD { get; set; }

        public ScoringSolutionSection TrustScienceConfigsUAT { get; set; }
        public ScoringSolutionSection TrustScienceConfigsPROD { get; set; }

        public int DapperDefaultTimeOut { get; set; }

        public int ReportGetRetryDelay { get; set; }

        public string SsnDecryptUrl { get; set; }
        public string SsnEncryptUrl { get; set; }
    }
}
