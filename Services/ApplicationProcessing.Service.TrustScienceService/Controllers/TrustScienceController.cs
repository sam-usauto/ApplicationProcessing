using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ApplicationProcessing.Service.ScoringSolution.Repositories;
using ApplicationProcessing.Service.TrustScienceService.DTOs.Configuration;
using ApplicationProcessing.Service.TrustScienceService.Services;
using Common.DTOs.Application;
using Common.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationProcessing.Service.TrustScienceService.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TrustScienceController : ControllerBase
    {
        private readonly string _version = "1.0.0";
        private readonly string _lastUpdated = "11/18/2020";

        private readonly TrustScienceConfiguration _config;
        private readonly ITrustScienceService _trustScienceService;
        private readonly ITrustScienceRepository _trustScienceRepository;
        private readonly SsnNumberService _ssnNumberService;

        private ApplicationStepInput _applicationStepInput = null;
        private int _dapperTimeOut = 90;

        public TrustScienceController(TrustScienceConfiguration config,
                                            ITrustScienceRepository trustScienceRepository,
                                            ITrustScienceService trustScienceService)
        {
            _config = config;
            _trustScienceService = trustScienceService;
            _ssnNumberService = new SsnNumberService(_config.SsnEncryptUrl, _config.SsnDecryptUrl);
            _dapperTimeOut = _config.DapperDefaultTimeOut;
            _trustScienceRepository = trustScienceRepository;
        }

        [HttpPost]
        [Route("Execute")]
        public async Task<IActionResult> Execute([FromBody] ApplicationStepInput appInfo)
        {
            try
            {
                await Task.FromResult(1);
                return Ok("Execute");
            }
            catch (SqlException ex)
            {
                throw ex;
                //return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                throw ex;
                //return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        [Route("Info")]
        public IActionResult Info()
        {
            return Ok($"Trust Science Service: Version: {_version} Last Updated On: {_lastUpdated}.");
        }
    }
}

