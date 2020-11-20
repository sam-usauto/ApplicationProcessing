using ApplicationProcessing.Service.TrustScienceService.DTOs;
using Common.DTOs;
using Common.DTOs.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationProcessing.Service.TrustScienceService.Services
{
    public interface ITrustScienceService
    {
        Task<HttpGeneralResponse> CreateFullScoringRequest(TrustScienceBatchItem trustScienceBatchItem, ApplicationStepInput appInfo);

        Task<int> FetchReportsFromTrustScience();
    }
}
