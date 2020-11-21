using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ApplicationProcessing.Service.ScoringSolution.Repositories;
using ApplicationProcessing.Service.TrustScienceService.DTOs;
using ApplicationProcessing.Service.TrustScienceService.DTOs.Configuration;
using ApplicationProcessing.Service.TrustScienceService.DTOs.Responses;
using ApplicationProcessing.Service.TrustScienceService.Services;
using Common.DTOs.Application;
using Common.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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

                // create score object
                var trustScienceScore = new TrustScienceScore();
                trustScienceScore.Request = createFullScoringResp.RequestData;
                trustScienceScore.Response = createFullScoringResp.ResponseData;
                trustScienceScore.CallStatus = createFullScoringResp.ReasonPhrase;
                trustScienceScore.CustomerID = trustScienceBatchItem.clientCustomerId;
                trustScienceScore.ApplicationID = trustScienceBatchItem.clientApplicationId;


                // get the response data from the call
                var jsonResponse = createFullScoringResp.ResponseData;

                // check if we got an error
                if (createFullScoringResp.IsSuccessStatusCode)
                {
                    var responseOk = JsonConvert.DeserializeObject<FullScoringResponseOk>(jsonResponse);

                    trustScienceScore.RequestID = responseOk.requestId;     // Load the requestID from Trust Science response

                    // save Req/Resp info into TrustScienceScore table
                    var saveCreateFullScoringToTableResp = await _trustScienceRepository.SaveFullScroingInfo(trustScienceScore);
                    // save the ID of the item inserted to the log table 
                    var trustScienceLogID = saveCreateFullScoringToTableResp.LogID ?? -1;

                    // mark step as completed
                    await _trustScienceRepository.MarkStepAsCompleted(appInfo);

                    
                    responseOk.LogID = appInfo.LogId;
                    responseOk.TrustScienceLogID = trustScienceLogID;
                    return Ok(responseOk);
                }
                else
                {
                    var responseBad = JsonConvert.DeserializeObject<FullScoringResponseBad>(jsonResponse);
                    return BadRequest(responseBad);
                }

                // Note: for now do not save the req/resp info to step table we already have log table for tust science
                // Save req/Resp to log table  [logs].[ApplicationFlowStepResult]
                //await _trustScienceRepository.ReqSaveRespToLog(
                //                                                createFullScoringResp.RequestData, 
                //                                                createFullScoringResp.ResponseData,
                //                                                appInfo
                //                                                );

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

