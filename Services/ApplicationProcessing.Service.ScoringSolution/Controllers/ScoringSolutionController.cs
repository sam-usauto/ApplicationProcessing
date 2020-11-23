using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using ApplicationProcessing.Service.ScoringSolution.DTOs.Configuration;
using ApplicationProcessing.Service.ScoringSolution.Repositories;
using ApplicationProcessing.Service.ScoringSolution.Services;
using Common.DTOs.Application;
using Common.Helper;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationProcessing.Service.ScoringSolution.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScoringSolutionController : ControllerBase
    {
        private readonly string _version = "1.0.0";
        private readonly string _lastUpdated = "11/21/2020";
        private readonly ScoringSolutionConfiguration _config;
        private readonly IScoringSolutionService _scoringSolutionService;
        private readonly IScoringSolutionRepository _scoringSolutionRepository;
        private readonly SsnNumberService _ssnNumberService;
        private ApplicationStepInput _applicationStepInput = null;
        private int _dapperTimeOut = 90;


        public ScoringSolutionController(ScoringSolutionConfiguration config,
                                            IScoringSolutionRepository scoringSolutionRepository,
                                            IScoringSolutionService scoringSolutionService)
        {
            _config = config;
            _scoringSolutionRepository = scoringSolutionRepository;
            _ssnNumberService = new SsnNumberService(_config.SsnEncryptUrl, _config.SsnDecryptUrl);
            _dapperTimeOut = _config.DapperDefaultTimeOut;
            _scoringSolutionService = scoringSolutionService;
        }

        [HttpPost]
        [Route("Execute")]
        // Send application to Scoring Solution to be process
        public async Task<IActionResult> Execute([FromBody] ApplicationStepInput appInfo)
        {
            try
            {
                _applicationStepInput = appInfo;

                // collect all the data needed by Scoring Solution for scoring request
                var app = await _scoringSolutionRepository.GetScoringSolutionApplication(appInfo.ApplicationID);

                if (String.IsNullOrEmpty(appInfo.ProtectedSsn))
                {
                    // call the SSN decryption
                    if (String.IsNullOrEmpty(app.EncryptedSsn) == false)
                    {
                        var ssnResp = await _ssnNumberService.UnprotectSsn(app.EncryptedSsn);
                        var unprotectedSsn = ssnResp.ResponseData;
                        app.Ssn = unprotectedSsn;
                    }
                }
                else
                {
                    // get the SSN from appInfo which was passed as a parameter
                    app.Ssn = appInfo.UnprotectedSsn;
                }

                var x = 1;

                //await Task.FromResult(1);

                //return new HttpResponseMessage(HttpStatusCode.OK);
                return Ok("ScoringSolution.Execute()");
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
            return Ok($"Scoring Solution Service - Version: {_version} Last Updated On: {_lastUpdated}.");
        }
    }
}
