using Common.DTOs.Application;
using System.Threading.Tasks;

namespace ApplicationWorkerDataLayer.Interfaces
{
    public interface IScoringSolutionRepository
    {
        Task<ScoringSolutionRequest> GetScoringSolutionApplication(int applicationId);
    }
}
