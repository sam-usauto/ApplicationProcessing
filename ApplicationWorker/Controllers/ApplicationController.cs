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

        private int _applicationLogInfoID = -1;


        public ApplicationController(ApplicationProcessingConfig config, IApplicationRepository applicationRepository)
        {
            _config = config;
            _applicationRepository = applicationRepository;
        }

        [HttpPost]
        [Route("save")]
        public HttpResponseMessage Save([FromBody] SaveShortAppWrapper application)
        {
            try
            {

                var json = JsonConvertion.ObjectToJson<SaveShortAppWrapper>(application, Indented);
                //var _applicationLogInfoID = 

                var appValidator = new AppValidator();
                var validationErrorList = appValidator.ValidateApp(application);
                if (validationErrorList.Count > 0)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

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
