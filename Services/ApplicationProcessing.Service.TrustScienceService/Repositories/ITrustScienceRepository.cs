using ApplicationProcessing.Service.TrustScienceService.DTOs;
using ApplicationProcessing.Service.TrustScienceService.DTOs.Responses;
using Common.DTOs.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationProcessing.Service.ScoringSolution.Repositories
{
    public interface ITrustScienceRepository
    {
        Task<TrustScienceBatchItem> GetFullApplicationByID(int creditScoreApplicationID);
        Task ReqSaveRespToLog(string req, string resp, ApplicationStepInput appInfo, int which = 1);
        Task<SaveCreateFullScoringToTableResp> SaveFullScroingInfo(TrustScienceScore trustScienceScore);
        Task MarkStepAsCompleted(ApplicationStepInput appInfo);
        Task<IEnumerable<ReportReq>> GetListOfMissingReport();
        void SaveGetScoringReportResp(string requestID, int logID, string getScoringReportJsonResp, ScoringReportResp scoringReportResp, string status);
        void SaveProcessingInfo(ProcessingResult processingResult);

    }
}
