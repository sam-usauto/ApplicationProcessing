using ApplicationProcessing.Service.PointPredictiveService.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationProcessing.Service.PointPredictiveService.Repositories
{
    public interface IPointPredictiveRepository
    {

        Task<UsAutoApp> GetApplicationDetailsByAppIdAsync(int applicationId, int cmdTimeOut);
    }
}
