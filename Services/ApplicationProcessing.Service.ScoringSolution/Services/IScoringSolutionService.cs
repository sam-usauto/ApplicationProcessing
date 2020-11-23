using ApplicationProcessing.Service.ScoringSolution.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationProcessing.Service.ScoringSolution.Services
{
    public interface IScoringSolutionService
    {
        Task<int> ProcessApplicationAsync(ScoringSolutionRequest scoringSolutionRequest);
    }
}
