
using ApplicationWorkerDataLayer.Interfaces;
using Common.DTOs.Application;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationWorkerDataLayer.Repositories
{
    public class ApplicationRepository : IApplicationRepository
    {
        public Task<int> SaveClientApplication(SaveShortAppWrapper saveShortAppWrapper)
        {
            throw new NotImplementedException();
        }
    }
}
