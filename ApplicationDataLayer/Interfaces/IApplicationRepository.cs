using Common.DTOs.Application;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationWorkerDataLayer.Interfaces
{

    public interface IApplicationRepository
    {
        Task<int> SaveClientOriginalApplication(string firstName, string lastName, string phoneNumber, string email, string ssn, string appJson);
        Task<ClientApplicationLogIds> SaveApplicationToDB( (ShortApp application, int logId, int userID, int lotID) applicationAndLog );
        Task<IEnumerable<ApplicationFlowStep>> GetApplicationFlowSteps(int logID);


        //Task<HttpGeneralResponse> CreateFullScoringRequest(TrustScienceBatchItem trustScienceBatchItem);
        //Task<HttpGeneralResponse> GetScoringReport(string id);
        //Task<SaveCreateFullScoringToTableResp> SaveFullScroingInfo(TrustScienceScore trustScienceScore);
        //Task<bool> SaveGetScoringReportResp(ScoringReportResp scoringReportResp);
        //void SaveGetScoringReportResp(string requestID, int logID, string getScoringReportJsonResp, ScoringReportResp scoringReportResp, string status);
        //Task<TrustScienceBatchItem> GetFullApplicationByID(int creditScoreApplicationID);
    }
}
