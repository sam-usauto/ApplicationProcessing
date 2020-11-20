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
                using (SqlConnection conn = new SqlConnection(_scoringDbConn))
                {
                    var list = await conn.QueryAsync<ReportReq>(
                                    "TrustScienceGetFailedBatchToReprocess", 
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
    }
}
