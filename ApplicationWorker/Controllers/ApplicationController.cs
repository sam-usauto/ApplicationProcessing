using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ApplicationWorker.DTOs;
using ApplicationWorker.Helper;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApplicationWorker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        [HttpPost]
        public HttpResponseMessage Save([FromBody] SaveShortAppWrapper application)
        {
            var appValidator = new AppValidator();
            var validationErrorList = appValidator.ValidateApp(application);
            if(validationErrorList.Count > 0)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
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
