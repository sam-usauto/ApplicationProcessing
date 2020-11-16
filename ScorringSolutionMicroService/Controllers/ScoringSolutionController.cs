using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Common.DTOs.Application;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ScoringSolutionMicroService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ScoringSolutionController : Controller
    {
        [HttpPost]
        [Route("Execute")]
        //public async Task<HttpResponseMessage> Execute([FromBody] (int applicationID, int logId, int userID) appInfo )
        public async Task<IActionResult> Execute([FromBody] ApplicationStepInput appInfo)
        {
            try
            {

                // Load ClientIP, Encrypt SS, save last 4 SSN etc.
                //await PreprocessApplication(application);



                // save original plain application to log and the Log entry id to class provate
                //_applicationLogID = await LogInitApplication(application);

                // Tuples
                //(ShortApp app, int logId, int _userID, int _lotID) appData = (application.Data, _applicationLogID, _userID, _lotID);

                // return the Ids of all the inserted tables related to the application
                //var SaveToDbResult = await _applicationRepository.SaveApplicationToDB(appData);

                // process the Application flow steps
                //await ExecuteApplicationProcessing(application.Data);
                await Task.FromResult(1);

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


    }
}
