using ApplicationProcessing.Service.ScoringSolution.DTOs;
using ApplicationProcessing.Service.ScoringSolution.DTOs.Configuration;
using ApplicationProcessing.Service.ScoringSolution.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ApplicationProcessing.Service.ScoringSolution.Services
{
    public class ScoringSolutionService : IScoringSolutionService
    {

        private readonly ScoringSolutionConfiguration _config;
        private readonly ScoringSolutionSection _scoringSolutionSection;
        private readonly IScoringSolutionRepository _scoringSolutionRepository;
        private readonly string _scoringDbConn = string.Empty;
        private readonly bool _production = false;

        public ScoringSolutionService(ScoringSolutionConfiguration config, IScoringSolutionRepository scoringSolutionRepository)
        {
            _config = config;

            _scoringSolutionRepository = scoringSolutionRepository;

            // Is Production... read from appSettings.json
            _production = _config.IsProduction;

            // get SQL connection string
            if (_production)
            {
                _scoringDbConn = _config.ConnectionStringsPROD.scoringDb;
            }
            else
            {
                _scoringDbConn = _config.ConnectionStringsUAT.scoringDb;
            }

            // TODO: Not used yet
            // read the application configuration and save it.
            //if (_production)
            //{
            //    _trustScienceConfigsSettings = _config..ConnectionStringsPROD;
            //}
            //else
            //{
            //    _trustScienceConfigsSettings = _config.TrustScienceConfigsUAT;
            //}
        }


        public async Task<int> ProcessApplicationAsync(ScoringSolutionRequest scoringSolutionRequest)
        {
            //var sw = new StringWriter();
            //var svcRequest = GetServiceRequest(scoringSolutionRequest);

            //var xmlReq = new XmlSerializer(typeof(Request));  // comes from WCF client
            //xmlReq.Serialize(sw, svcRequest);
            //var input = sw.ToString();

            //// CDATA should not be escaped
            //input = input.Replace("&lt;", "<");
            //input = input.Replace("&gt;", ">");
            //input = input.Replace("&amp;", "&");
            //input = input.Replace("	&quot;", "\"");
            //input = input.Replace("&apos;", "'");

            //var appInfo = string.Empty;

            return await Task.FromResult(1);
        }




        #region Helper methods

        //public static Request GetServiceRequest(DataViewRequestDTO request)
        //{
        //    var res = new Request
        //    {
        //        MODEL_ID = request.ModelId,
        //        CUST_NUMBER = request.ApplicationID.ToString(),
        //        CUST_BUYER = request.CoBuyerCode,
        //        CUST_FIRSTNAME = request.CustomerFirstName,
        //        CUST_LASTNAME = request.CustomerLastName,
        //        CUST_MIDDLENAME = request.CustomerMiddleName,
        //        CUST_SUFFIX = request.CustomerSuffixTypeValue,
        //        CUST_SOCIAL_SECURITY = request.CustomerSSN.Unprotect(),
        //        CUST_HOUSENUMBER = request.HouseNumber,
        //        CUST_QUADRANT = request.QuadRant,
        //        CUST_STREETNAME = request.StreetName,
        //        CUST_STREETTYPE = request.StreetTypeName,
        //        CUST_CITY = request.City,
        //        CUST_STATE = request.StateAbbreviation,
        //        CUST_ZIP = request.PostalCode,
        //        APP_DATE = request.DateModifiedFormatted,
        //        APP_MTHS_CURR_JOB = request.MonthsCurrentJob.ToString(),
        //        APP_MTHS_PREV_JOB = request.MonthsPreviousJob.ToString(),
        //        APP_MTHS_CURR_RES = request.MonthsCurrentResidence.ToString(),
        //        APP_MTHS_PREV_RES = request.MonthsPreviousResidence.ToString(),
        //        APP_HOUSING_TYPE = request.HousingTypeName,
        //        APP_MTHLY_NET_INCOME = request.NetIncome.ToString(CultureInfo.InvariantCulture),
        //        APP_MTHLY_PMNT_INCOME = request.PaymentIncome,
        //        APP_MTHLY_HOUSING_PMNT = (request.HousingPayment ?? 0).ToString(CultureInfo.InvariantCulture),
        //        EquifaxRawData = request.EquifaxRawData
        //        // EquifaxRawData = request.EquifaxRawData,
        //        // bureau = request.bureau.HasValue ? request.bureau.Value.ToString() : String.Empty
        //    };
        //    return res;
        //}

        #endregion
    }
}
