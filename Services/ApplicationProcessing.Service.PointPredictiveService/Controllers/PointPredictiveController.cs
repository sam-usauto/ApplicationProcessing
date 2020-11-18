using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using ApplicationProcessing.Service.PointPredictiveService.DTOs;
using ApplicationProcessing.Service.PointPredictiveService.Repositories;
using Common.DTOs.Application;
//using ApplicationWorkerDataLayer.Interfaces;
//using Common.DTOs.Application;
//using Common.DTOs.Configurations;
using Common.DTOs.Configurations.ApplicationWorker;
using Common.Helper;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationProcessing.Service.PointPredictiveService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PointPredictiveController : ControllerBase
    {
        private readonly string _version = "1.0.0";
        private readonly string _lastUpdated = "11/18/2020";

        private readonly PointPredictiveConfig _config;
        private readonly IPointPredictiveRepository _pointPredictiveRepository;
        private readonly SsnNumberService _ssnNumberService;

        private ApplicationStepInput _applicationStepInput = null;

        private int _dapperTimeOut = 90;

        public PointPredictiveController(PointPredictiveConfig config, IPointPredictiveRepository pointPredictiveRepository)
        {
            _config = config;
            _pointPredictiveRepository = pointPredictiveRepository;
            _ssnNumberService = new SsnNumberService(_config.SsnEncryptUrl, _config.SsnDecryptUrl);
            _dapperTimeOut = _config.DapperDefaultTimeOut;
        }

        [HttpPost]
        [Route("Execute")]
        //public async Task<HttpResponseMessage> Execute([FromBody] (int applicationID, int logId, int userID) appInfo )
        public async Task<IActionResult> Execute([FromBody] ApplicationStepInput appInfo)
        {
            try
            {
                _applicationStepInput = appInfo;

                // collect all the data needed by Point Predictive for the request
                var app = await _pointPredictiveRepository.GetApplicationDetailsByAppIdAsync(appInfo.ApplicationID, _dapperTimeOut);

                if (app == null)
                {
                    return BadRequest("Failed collecting application by applicationId");
                }

                // call the SSN decryption
                if (String.IsNullOrEmpty(app.SSN) == false)
                {
                    var ssnResp = await _ssnNumberService.UnprotectSsn(app.SSN);
                    var unprotectedSsn = ssnResp.ResponseData;
                    app.SSN = unprotectedSsn;
                }

                // make sure data is clean
                CleanApp(app);

                return Ok(app);
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

        private void CleanApp(AppRequest app)
        {
            // sometime we are getting spaces in the phone number..
            // needed to remove... can not convert to number
            app.CellPhoneNumber = app.CellPhoneNumber.Replace(" ", "");
            app.HomePhoneNumber = app.HomePhoneNumber.Replace(" ", "");
            app.WorkPhoneNumber = app.WorkPhoneNumber.Replace(" ", "");
            app.CobCellPhoneNumber = app.CobCellPhoneNumber.Replace(" ", "");
            app.CobHomePhoneNumber = app.CobHomePhoneNumber.Replace(" ", "");
            app.CobWorkPhoneNumber = app.CobWorkPhoneNumber.Replace(" ", "");
            app.EmployerPhone = app.EmployerPhone.Replace(" ", "");
            // "-999999" => empty date
            app.CustomerSinceDate = String.IsNullOrEmpty(app.CustomerSinceDate) ? "-999999" : app.CustomerSinceDate;
        }

        [HttpGet]
        [Route("Info")]
        //public async Task<HttpResponseMessage> Execute([FromBody] (int applicationID, int logId, int userID) appInfo )
        public IActionResult Info()
        {
            return Ok($"Point Predictive Service: Version: {_version} Last Updated On: {_lastUpdated}.");
        }
    }
}

