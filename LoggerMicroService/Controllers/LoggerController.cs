using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Common.DTOs.Loggers.Serilog;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LoggerMicroService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoggerController : ControllerBase
    {
        //private readonly IEmailService _emailService;
        private readonly SerilogConfig _serilogConfig;

        public LoggerController(
                    //ITrustScienceService trustScienceService,
                    SerilogConfig config
                    )
        {
            //_trustScienceService = trustScienceService;
            _serilogConfig = config;
        }

        [HttpPost]
        public HttpResponseMessage Save([FromBody] LogDetail logDetail)
        {

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpGet]
        [Route("Config")]
        public IActionResult Config()
        {
            // TODO - mask the connection string and change the "//" to "/" in the locations
            return Ok(_serilogConfig);
        }

    }
}
