using ApplicationProcessing.Service.TrustScienceService.DTOs;
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
    }
}
