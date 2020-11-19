using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using ApplicationWorkerDataLayer.Interfaces;
using Common.DTOs.Application;
using Common.DTOs.Configurations;
using Common.DTOs.Configurations.ApplicationWorker;
using Common.Helper;
using Microsoft.AspNetCore.Mvc;

namespace ScoringSolutionMicroService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ScoringSolutionController : Controller
    {
        //private readonly ScoringSolutionConfig _config;
        //private readonly IScoringSolutionRepository _scoringSolutionRepository;
        //private readonly SsnNumberService _ssnNumberService;

        //private ApplicationStepInput _applicationStepInput = null;

        //public ScoringSolutionController(ScoringSolutionConfig config, IScoringSolutionRepository scoringSolutionRepository)
        //{
        //    _config = config;
        //    _scoringSolutionRepository = scoringSolutionRepository;
        //    _ssnNumberService = new SsnNumberService(_config.SsnEncryptUrl, _config.SsnDecryptUrl);
        //}


        //[HttpPost]
        //[Route("Execute")]
        ////public async Task<HttpResponseMessage> Execute([FromBody] (int applicationID, int logId, int userID) appInfo )
        //public async Task<IActionResult> Execute([FromBody] ApplicationStepInput appInfo)
        //{
        //    try
        //    {
        //        _applicationStepInput = appInfo;

        //        // collect all the data needed by Scoring Solution for scoring request
        //        var app = await _scoringSolutionRepository.GetScoringSolutionApplication(appInfo.ApplicationID);

        //        // call the SSN decryption
        //        if (String.IsNullOrEmpty(app.EncryptedSsn) == false)
        //        {
        //            var ssnResp = await _ssnNumberService.UnprotectSsn(app.EncryptedSsn);
        //            var unprotectedSsn = ssnResp.ResponseData;
        //            app.Ssn = unprotectedSsn;
        //        }

        //        // Load ClientIP, Encrypt SS, save last 4 SSN etc.
        //        //await PreprocessApplication(application);
        //        var x = 1;


        //        // save original plain application to log and the Log entry id to class provate
        //        //_applicationLogID = await LogInitApplication(application);

        //        // Tuples
        //        //(ShortApp app, int logId, int _userID, int _lotID) appData = (application.Data, _applicationLogID, _userID, _lotID);

        //        // return the Ids of all the inserted tables related to the application
        //        //var SaveToDbResult = await _applicationRepository.SaveApplicationToDB(appData);

        //        // process the Application flow steps
        //        //await ExecuteApplicationProcessing(application.Data);
        //        await Task.FromResult(1);

        //        //return new HttpResponseMessage(HttpStatusCode.OK);
        //        return Ok("ScoringSolution.Execute()");
        //    }
        //    catch (SqlException ex)
        //    {
        //        throw ex;
        //        //return new HttpResponseMessage(HttpStatusCode.BadRequest);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //        //return new HttpResponseMessage(HttpStatusCode.BadRequest);
        //    }
        //}


    }
}
