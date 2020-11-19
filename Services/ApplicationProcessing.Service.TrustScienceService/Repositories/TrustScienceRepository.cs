using ApplicationProcessing.Service.TrustScienceService.DTOs;
using ApplicationProcessing.Service.TrustScienceService.DTOs.Configuration;
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
    }
}
