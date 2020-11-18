using ApplicationProcessing.Service.PointPredictiveService.DTOs;
using ApplicationProcessing.Service.PointPredictiveService.DTOs.PointPredictive;
using Dapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationProcessing.Service.PointPredictiveService.Repositories
{
    public class PointPredictiveRepository : IPointPredictiveRepository
    {

        public PointPredictiveConfig _config;               
        bool _production = false;                           // Is Production?... read from appSettings.json
        string _scoringDbConn = string.Empty;               // SQL connection string...  Depended on IsProduction setting

        public PointPredictiveRepository(PointPredictiveConfig config)
        {
            _config = config;
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
        }


        public async Task<UsAutoApp> GetApplicationDetailsByAppIdAsync(int applicationId, int cmdTimeOut)
        {
            try
            {
                using (var conn = new SqlConnection(_scoringDbConn))
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@ApplicationID", applicationId);

                    var app = await conn.QueryFirstOrDefaultAsync<UsAutoApp>(
                                 "[pp].[GetApplicationByAppID]",
                                 queryParameters,
                                 transaction: null,
                                 commandTimeout: cmdTimeOut,
                                 commandType: CommandType.StoredProcedure);

                    return app;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // save point predictive results to table
        public async Task<SavePointPredictiveResp> SavePointPredictiveScoreAsync(
                        PointPredictiveReportResp pointPredictiveReportResp, 
                        PointPredictiveScoreReq pointPredictiveScoreReq, 
                        UsAutoApp usAutoApp, 
                        string userName, 
                        string cleanReq, 
                        int creditId, 
                        string errorMsg)
        {
            {
                //var  creditId = PointPredictiveScoreReq.administrative_fields.application_identifier;
                var reqString = string.Empty;
                var respString = string.Empty;
                var ssnString = string.Empty;
                var fraudEpdReportLink = string.Empty;
                var dealerRiskReportLink = string.Empty;
                var fraudScore = string.Empty;
                var fraudReasonCode1 = string.Empty;
                var fraudReasonCode2 = string.Empty;
                var fraudReasonCode3 = string.Empty;
                var isCompleted = true;

                if (pointPredictiveScoreReq != null)
                {
                    reqString = JsonConvert.SerializeObject(pointPredictiveScoreReq, Formatting.Indented);


                    // todo:  fix the issue
                    //// decrypt SSN
                    //var ssnResp = usAutoApp..ss.SsnRespone;
                    //// Only show last 4 digits of ssn
                    //if (ssnResp.ResponseData.ToLower() != "Invalid Protected SSN".ToLower())
                    //{
                    //    var last4Digits = ssnResp.ResponseData.Substring(5, 4);
                    //    ssnResp.ResponseData = "******" + last4Digits;
                    //}
                    //ssnString = JsonConvert.SerializeObject(ssnResp);
                }
                else
                {
                    isCompleted = false;
                }

                if (pointPredictiveReportResp != null)
                {
                    if (pointPredictiveReportResp.HttpRespone.IsSuccessStatusCode)
                    {
                        respString = JsonConvert.SerializeObject(pointPredictiveReportResp,Formatting.Indented);

                        fraudEpdReportLink = pointPredictiveReportResp.PointPredictiveScoreResp.Report_links.fraud_epd_report_link;
                        dealerRiskReportLink = pointPredictiveReportResp.PointPredictiveScoreResp.Report_links.dealer_risk_report_link;

                        var fraudInfo = pointPredictiveReportResp.PointPredictiveScoreResp.Application_fraud_information;
                        fraudScore = fraudInfo.fraud_score;
                        fraudReasonCode1 = fraudInfo.reason_code_1;
                        fraudReasonCode2 = fraudInfo.reason_code_2;
                        fraudReasonCode3 = fraudInfo.reason_code_3;
                    }
                    else
                    {
                        // Response from PointPrediction failed
                        fraudEpdReportLink = "";
                        dealerRiskReportLink = "";
                        fraudScore = "";
                        fraudReasonCode1 = "";
                        fraudReasonCode2 = "";
                        fraudReasonCode3 = "";
                        respString = "Server generated Error! See Errors field for details.";

                        if (pointPredictiveReportResp.Exception != null)
                        {
                            errorMsg = pointPredictiveReportResp.Exception.Message;
                        }
                    }
                }
                else
                {
                    isCompleted = false;
                }

                try
                {
                    using (var conn = new SqlConnection(_scoringDbConn))
                    {
                        // get userID from user name
                        var userParameters = new DynamicParameters();
                        userParameters.Add("@UserName", userName);
                        var userID = await conn.ExecuteScalarAsync<int>(
                            "PointPredictiveGetUserId",
                            userParameters,
                            commandType: CommandType.StoredProcedure);

                        var comment = string.Empty;

                        //// if we have exception respString will be empty
                        //if (respString != string.Empty)
                        //{ 
                        //    comment = $"PP Fraud Score: {fraudScore} Returned on {DateTime.Now.ToString("MM/dd/yyyy")}. Report Link: ";
                        //    comment += $"<a style=\"color: blue;font-weight:700;\" href=\"{fraudEpdReportLink}\" target=\"_blank\">View Report</a>";
                        //}
                        /////////////////////////////////////////
                        // Save report result to comments table
                        /////////////////////////////////////////
                        var queryParameters = new DynamicParameters();
                        queryParameters.Add("@creditID", creditId);
                        queryParameters.Add("@reqString", reqString);
                        queryParameters.Add("@respString", respString);
                        queryParameters.Add("@ssnString", ssnString);
                        queryParameters.Add("@completed", isCompleted);
                        queryParameters.Add("@FraudEpdReportLink", fraudEpdReportLink);
                        queryParameters.Add("@DealerRiskReportLink", dealerRiskReportLink);
                        queryParameters.Add("@FraudScore", fraudScore);
                        queryParameters.Add("@FraudReasonCode1", fraudReasonCode1);
                        queryParameters.Add("@FraudReasonCode2", fraudReasonCode2);
                        queryParameters.Add("@FraudReasonCode3", fraudReasonCode3);
                        queryParameters.Add("@CleanReq", cleanReq);
                        queryParameters.Add("@ErrorMsg", errorMsg);
                        queryParameters.Add("@CreatedBy", userID);
                        // inset the comment before inserting the the PP call information
                        // in the SP PointPredictiveSaveReqResp
                        queryParameters.Add("@Comment", comment);

                        var insertedId = await conn.QueryFirstAsync<int>(
                             "PointPredictiveSaveReqResp",
                             queryParameters,
                             commandType: CommandType.StoredProcedure);

                        var entryId = insertedId.ToString();

                        // return Point Predictive result to user
                        return new SavePointPredictiveResp
                        {
                            NewSaveId = entryId,
                            Completed = true,
                            Exception = pointPredictiveReportResp.Exception
                        };
                    }
                }
                catch (Exception ex)
                {
                    return new SavePointPredictiveResp
                    {
                        NewSaveId = string.Empty,
                        Completed = false,
                        Exception = ex,
                        ExceptionFile = @"Core\Services\PointPredictiveService.cs",
                        ExceptionInArea = "SavePointPredictiveScoreAsync"
                    };
                }
            }
        }

        public async Task<int> UpdateAutoScoreToCompleted(int Id)
        {
            try
            {
                string sqlCmd = $"UPDATE PP_AutoRun SET IsSuccessful = 1 WHERE Id = {Id};";
                using (var conn = new SqlConnection(_scoringDbConn))
                {
                    return await conn.ExecuteAsync(sqlCmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
