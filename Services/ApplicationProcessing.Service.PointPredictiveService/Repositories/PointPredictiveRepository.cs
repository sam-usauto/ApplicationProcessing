using ApplicationProcessing.Service.PointPredictiveService.DTOs;
using Dapper;
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


        public async Task<AppRequest> GetApplicationDetailsByAppIdAsync(int applicationId, int cmdTimeOut)
        {
            try
            {
                using (var conn = new SqlConnection(_scoringDbConn))
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@ApplicationID", applicationId);

                    var app = await conn.QueryFirstOrDefaultAsync<AppRequest>(
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
    }
}
