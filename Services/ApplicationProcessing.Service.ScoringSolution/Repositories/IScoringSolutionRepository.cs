using ApplicationProcessing.Service.ScoringSolution.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationProcessing.Service.ScoringSolution.Repositories
{
    public interface IScoringSolutionRepository
    {
        Task<ScoringSolutionRequest> GetScoringSolutionApplication(int applicationId);
    }
}
