﻿using Common.DTOs.Application;
using System.Threading.Tasks;

namespace ApplicationWorkerDataLayer.Interfaces
{

    public interface IApplicationRepository
    {
        Task<int> SaveClientOriginalApplication(string appJson);
        //Task<HttpGeneralResponse> CreateFullScoringRequest(TrustScienceBatchItem trustScienceBatchItem);
        //Task<HttpGeneralResponse> GetScoringReport(string id);
        //Task<SaveCreateFullScoringToTableResp> SaveFullScroingInfo(TrustScienceScore trustScienceScore);
        //Task<bool> SaveGetScoringReportResp(ScoringReportResp scoringReportResp);
        //void SaveGetScoringReportResp(string requestID, int logID, string getScoringReportJsonResp, ScoringReportResp scoringReportResp, string status);
        //Task<TrustScienceBatchItem> GetFullApplicationByID(int creditScoreApplicationID);
    }
}