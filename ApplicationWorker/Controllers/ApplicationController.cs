using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ApplicationWorker.Helper;
using ApplicationWorkerDataLayer.Interfaces;
using ApplicationWorkerDataLayer.Repositories;
using Common.DTOs.Application;
using Common.DTOs.Configurations.ApplicationWorker;
using Common.Helper;
using Microsoft.AspNetCore.Mvc;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApplicationWorker.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApplicationController : ControllerBase
    {

        private readonly ApplicationProcessingConfig _config;
        private readonly IApplicationRepository _applicationRepository;
        private readonly SsnNumberService _ssnNumberService;

        private int _applicationLogID = -1;


        public ApplicationController(ApplicationProcessingConfig config, IApplicationRepository applicationRepository)
        {
            _config = config;
            _applicationRepository = applicationRepository;
            _ssnNumberService = new SsnNumberService(_config);
        }

        [HttpPost]
        [Route("save")]
        public async Task<HttpResponseMessage> Save([FromBody] SaveShortAppWrapper application)
        {
            try
            {
                application.Data.ClientIP = GetClientIP();
                application.Data.Last4Ssn = GetLast4Ssn(application.Data.Ssn);
                // TODO - need to encrypt SSN
                // mask the SSN for the Json
                var ssnPlain = application.Data.Ssn;

                var encryptedSSN = "Error";
                // call the SSN decryption
                if (String.IsNullOrEmpty(ssnPlain) == false)
                {
                    var ssnResp = await _ssnNumberService.EncryptSsn(ssnPlain);
                    encryptedSSN = ssnResp.ResponseData;
                }

                application.Data.Ssn = encryptedSSN;
                var json = JsonConvertion.ObjectToJson<SaveShortAppWrapper>(application);
                _applicationLogID = await _applicationRepository.SaveClientOriginalApplication(
                                    application.Data.FirstName, 
                                    application.Data.LastName,
                                    application.Data.PhoneNumber,
                                    application.Data.Email,
                                    _ssnNumberService.LastFourDigits(ssnPlain),
                                    json);


                var appValidator = new AppValidator();
                var validationErrorList = appValidator.ValidateApp(application);
                if (validationErrorList.Count > 0)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                // Tuples
                (ShortApp app, int logId) appData = (application.Data, _applicationLogID);

                // return the Ids of all the inserted tables
                var result = await _applicationRepository.SaveApplicationToDB(appData);

                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("config")]
        public IActionResult Config()
        {
            // TODO - mask the connection string and change the "//" to "/" in the locations
            return Ok(_config);
        }

        [HttpGet("version")]
        public string Version()
        {
            return "Version 1.0.0";
        }

        // get last 4 digit from SSN
        private string GetLast4Ssn(string ssn)
        {
            ssn = ssn.Trim();
            if(string.IsNullOrEmpty(ssn) || ssn.Length < 4)
            {
                return "Err";
            }
            ssn = ssn.Substring(ssn.Length - 4);
            return ssn;
        }

        // the the Client IP address
        private string GetClientIP()
        {
            var remoteIpAddress = HttpContext.Connection.RemoteIpAddress;
            return remoteIpAddress.ToString();
        }



        // GET: api/<ApplicationController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ApplicationController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ApplicationController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ApplicationController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ApplicationController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
