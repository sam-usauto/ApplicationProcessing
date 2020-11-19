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
                // get application detail
                var trustScienceBatchItem = await _trustScienceRepository.GetFullApplicationByID(appInfo.ApplicationID);

                // call the SSN decryption
                if (String.IsNullOrEmpty(trustScienceBatchItem.SSN) == false)
                {
                    var ssnResp = await _ssnNumberService.UnprotectSsn(trustScienceBatchItem.SSN);
                    var unprotectedSsn = ssnResp.ResponseData;
                    trustScienceBatchItem.SSN = unprotectedSsn;
                }

                // create request data to match Trust Science format
                var createFullScoringResp = await _trustScienceService.CreateFullScoringRequest(trustScienceBatchItem, appInfo);

                // Save req/Resp to log table  [logs].[ApplicationFlowStepResult]
                await _trustScienceRepository.ReqSaveRespToLog(
                                                                createFullScoringResp.RequestData, 
                                                                createFullScoringResp.ResponseData,
                                                                appInfo
                                                                );

                return Ok(trustScienceBatchItem);
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
            return Ok($"Trust Science Service - Version: {_version} Last Updated On: {_lastUpdated}.");
        }
    }
}

