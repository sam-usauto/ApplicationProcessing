using ApplicationProcessing.Service.PointPredictiveService.DTOs.PointPredictive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationProcessing.Service.PointPredictiveService.Services
{
    public interface IPointPredictiveService
    {
        string JsonRemoveEmptyProperties(PointPredictiveScoreReq pointPredictiveScoreReq);
        Task<PointPredictiveReportResp> GetPointPredictiveScoreAsync(PointPredictiveScoreReq pointPredictiveScoreReq);
    }
}
