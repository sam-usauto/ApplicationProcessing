using ApplicationProcessing.Service.TrustScienceService.DTOs;
using ApplicationProcessing.Service.TrustScienceService.DTOs.Configuration;
using ApplicationProcessing.Service.TrustScienceService.DTOs.Responses;
using Common.DTOs.Application;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationProcessing.Service.ScoringSolution.Repositories
{
    public class TrustScienceRepository : ITrustScienceRepository
    {

        public TrustScienceConfiguration _config;
        bool _production = false;                           // Is Production?... read from appSettings.json
        string _scoringDbConn = string.Empty;               // SQL connection string...  Depended on IsProduction setting

        public TrustScienceRepository(TrustScienceConfiguration config)
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

        // save report req/resp to Trust Science log [dbo].[TrustScienceScore] table.
        public async void SaveGetScoringReportResp(
                                                string requestID, 
                                                int logID, 
                                                string getScoringReportJsonResp, 
                                                ScoringReportResp scoringReportResp, 
                                                string status)
        {
            try
            {
                using (var conn = new SqlConnection(_scoringDbConn))
                {
                    var qualifierCode1 = "";
                    var qualifierCodeDescription1 = "";
                    var qualifierCode2 = "";
                    var qualifierCodeDescription2 = "";
                    var qualifierCode3 = "";
                    var qualifierCodeDescription3 = "";
                    var qualifierCode4 = "";
                    var qualifierCodeDescription4 = "";

                    var scoreReasonCode1 = "";
                    var scoreReasonDescription1 = "";
                    var scoreReasonCode2 = "";
                    var scoreReasonDescription2 = "";
                    var scoreReasonCode3 = "";
                    var scoreReasonDescription3 = "";
                    var scoreReasonCode4 = "";
                    var scoreReasonDescription4 = "";

                    // load the qualifier codes
                    if (status == "OK")
                    {
                        var itemNum = 1;
                        foreach (var qualifier in scoringReportResp.scoreQualifier)
                        {
                            if (itemNum == 1)
                            {
                                qualifierCode1 = qualifier.qualifierCode;
                                qualifierCodeDescription1 = qualifier.qualifierDescription;
                            }
                            if (itemNum == 2)
                            {
                                qualifierCode2 = qualifier.qualifierCode;
                                qualifierCodeDescription2 = qualifier.qualifierDescription;
                            }
                            if (itemNum == 3)
                            {
                                qualifierCode3 = qualifier.qualifierCode;
                                qualifierCodeDescription3 = qualifier.qualifierDescription;
                            }
                            if (itemNum == 4)
                            {
                                qualifierCode4 = qualifier.qualifierCode;
                                qualifierCodeDescription4 = qualifier.qualifierDescription;
                            }

                            itemNum++;
                            if (itemNum > 4)
                            {
                                break;
                            }

                        }

                        itemNum = 1;
                        foreach (var scoreReason in scoringReportResp.scoreReasons)
                        {
                            if (itemNum == 1)
                            {
                                scoreReasonCode1 = scoreReason.code;
                                scoreReasonDescription1 = scoreReason.description;
                            }
                            if (itemNum == 2)
                            {
                                scoreReasonCode2 = scoreReason.code;
                                scoreReasonDescription2 = scoreReason.description;
                            }
                            if (itemNum == 3)
                            {
                                scoreReasonCode3 = scoreReason.code;
                                scoreReasonDescription3 = scoreReason.description;
                            }
                            if (itemNum == 4)
                            {
                                scoreReasonCode4 = scoreReason.code;
                                scoreReasonDescription4 = scoreReason.description;
                            }

                            itemNum++;
                            if (itemNum > 4)
                            {
                                break;
                            }
                        }

                    }

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@LogID", logID);
                    queryParameters.Add("@RequestID", requestID);
                    queryParameters.Add("@Score", status == "OK" ? scoringReportResp.score : 0);
                    queryParameters.Add("@QualifierCode1", qualifierCode1);
                    queryParameters.Add("@QualifierCodeDescription1", qualifierCodeDescription1);
                    queryParameters.Add("@QualifierCode2", qualifierCode2);
                    queryParameters.Add("@QualifierCodeDescription2", qualifierCodeDescription2);
                    queryParameters.Add("@QualifierCode3", qualifierCode3);
                    queryParameters.Add("@QualifierCodeDescription3", qualifierCodeDescription3);
                    queryParameters.Add("@QualifierCode4", qualifierCode4);
                    queryParameters.Add("@QualifierCodeDescription4", qualifierCodeDescription4);

                    queryParameters.Add("@ScoreReasonCode1", scoreReasonCode1);
                    queryParameters.Add("@ScoreReasonDescription1", scoreReasonDescription1);
                    queryParameters.Add("@ScoreReasonCode2", scoreReasonCode2);
                    queryParameters.Add("@ScoreReasonDescription2", scoreReasonDescription2);
                    queryParameters.Add("@ScoreReasonCode3", scoreReasonCode3);
                    queryParameters.Add("@ScoreReasonDescription3", scoreReasonDescription3);
                    queryParameters.Add("@ScoreReasonCode4", scoreReasonCode4);
                    queryParameters.Add("@ScoreReasonDescription4", scoreReasonDescription4);

                    queryParameters.Add("@ScoringDetailsURL", status == "OK" ? scoringReportResp.appendix.links.scoringDetailsUrl : "");
                    queryParameters.Add("@Response", getScoringReportJsonResp);
                    queryParameters.Add("@CallStatus", status);

                    await conn.ExecuteAsync(
                                 "[trustScience].[SaveGetReportRespone]",
                                 queryParameters,
                                 commandType: CommandType.StoredProcedure);

                    return;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // get detail of application by creditScoreApplicationID
        public async Task<TrustScienceBatchItem> GetFullApplicationByID(int applicationID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_scoringDbConn))
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@ApplicationID", applicationID);
                    var result = await conn.QueryFirstOrDefaultAsync<TrustScienceBatchItem>(
                                    "[trustScience].[GetApplicationDetail]",
                                    queryParameters,
                                    commandType: CommandType.StoredProcedure);

                    // if missing application scoreModal
                    if(result == null)
                    {
                        throw new Exception($"TrustScienceRepository.GetFullApplicationByID did not returned application ({applicationID})");
                    }

                    // caluclate the nonNormalizedIncome
                    queryParameters = new DynamicParameters();
                    queryParameters.Add("@ApplicationID", result.clientApplicationId);
                    var income = await conn.ExecuteScalarAsync<int>(
                                    "TrustScienceGetNormaliazedIncome",
                                    queryParameters,
                                    commandType: CommandType.StoredProcedure);

                    result.nonNormalizedIncome = income;
                    result.applicantType = "primary";
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // build list of all report needed to be saved into [dbo].[TrustScienceScore] log table
        public async Task<IEnumerable<ReportReq>> GetListOfMissingReport()
        {
            try
            {
                // TODO: Change the SP to limit the number of trying the same report
                using (SqlConnection conn = new SqlConnection(_scoringDbConn))
                {
                    var list = await conn.QueryAsync<ReportReq>(
                                    "[trustScience].[GetAppsToConvertToReports]", 
                                    new { }, 
                                    commandType: CommandType.StoredProcedure
                                    );
                    return list;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task MarkStepAsCompleted(ApplicationStepInput appInfo)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_scoringDbConn))
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@ApplicationFlowStepResultID", appInfo.ApplicationFlowStepResultID);

                    var result = await conn.ExecuteAsync(
                                    "[logs].[MarkStepAsCompleted]",
                                    queryParameters,
                                    commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task ReqSaveRespToLog(string req, string resp, ApplicationStepInput appInfo, int which = 1)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_scoringDbConn))
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@ApplicationFlowStepResultID", appInfo.ApplicationFlowStepResultID);
                    queryParameters.Add("@Req", req);
                    queryParameters.Add("@Resp", resp);
                    queryParameters.Add("@Which", which);

                    var result = await conn.ExecuteAsync(
                                    "[trustScience].[SaveReqRespToLog]",
                                    queryParameters,
                                    commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<SaveCreateFullScoringToTableResp> SaveFullScroingInfo(TrustScienceScore trustScienceScore)
        {
            try
            {
                using (var conn = new SqlConnection(_scoringDbConn))
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@CustomerID", trustScienceScore.CustomerID);
                    queryParameters.Add("@ApplicationID", trustScienceScore.ApplicationID);
                    queryParameters.Add("@Request", trustScienceScore.Request);
                    queryParameters.Add("@Response", trustScienceScore.Response);
                    queryParameters.Add("@CallStatus", trustScienceScore.CallStatus);
                    queryParameters.Add("@RequestID", trustScienceScore.RequestID);

                    var insertedId = await conn.QueryFirstAsync<int>(
                                 "[trustScience].[SaveCreateFullScoringReqResp]",
                                 queryParameters,
                                 commandType: CommandType.StoredProcedure);

                    var resp = new SaveCreateFullScoringToTableResp();
                    resp.LogID = insertedId;        // save Trust Science log ID
                    return await Task.FromResult(resp);
                }
            }
            catch (Exception ex)
            {
                throw ex;
                //return await Task.FromResult(new SaveCreateFullScoringToTableResp());
            }
        }


        public async void SaveProcessingInfo(ProcessingResult processingResult)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_scoringDbConn))
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@LastItemDateTime", processingResult.LastItemDateTime);
                    queryParameters.Add("@TotalItemCount", processingResult.TotalItemCount);
                    queryParameters.Add("@SuccessfulItemCount", processingResult.SuccessfulItemCount);
                    queryParameters.Add("@FailedItemCount", processingResult.FailedItemCount);
                    queryParameters.Add("@ProcessingErrorItemCount", processingResult.ProcessingErrorItemCount);
                    queryParameters.Add("@StartDateTime", processingResult.StartDateTime);
                    queryParameters.Add("@EndDateTime", processingResult.EndDateTime);

                    await conn.ExecuteAsync(
                                            "[trustScience].[SaveProcessingResult]", 
                                            queryParameters, 
                                            commandType: 
                                            CommandType.StoredProcedure);
                    return;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
