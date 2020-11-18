using ApplicationProcessing.Service.PointPredictiveService.DTOs;
using ApplicationProcessing.Service.PointPredictiveService.DTOs.PointPredictive;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationProcessing.Service.PointPredictiveService.Repositories
{
    public interface IPointPredictiveRepository
    {
        Task<int> UpdateAutoScoreToCompleted(int Id);
        Task<UsAutoApp> GetApplicationDetailsByAppIdAsync(int applicationId, int cmdTimeOut);
        Task<SavePointPredictiveResp> SavePointPredictiveScoreAsync(
                             PointPredictiveReportResp pointPredictiveReportResp,
                             PointPredictiveScoreReq pointPredictiveScoreReq,
                             UsAutoApp usAutoApp,
                             string userName,
                             string cleanReq,
                             int creditId,
                             string errorMsg);
    }
}
