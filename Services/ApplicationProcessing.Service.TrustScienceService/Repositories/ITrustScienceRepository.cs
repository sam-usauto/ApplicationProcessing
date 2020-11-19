using ApplicationProcessing.Service.TrustScienceService.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationProcessing.Service.ScoringSolution.Repositories
{
    public interface ITrustScienceRepository
    {
        Task<TrustScienceBatchItem> GetFullApplicationByID(int creditScoreApplicationID);
    }
}
