using ApplicationProcessing.Service.ScoringSolution.DTOs;
using ApplicationProcessing.Service.ScoringSolution.DTOs.Configuration;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationProcessing.Service.ScoringSolution.Repositories
{

    public class ScoringSolutionRepository : IScoringSolutionRepository
    {
        public ScoringSolutionConfiguration _config;               // ApplicationWorker configuration object
        bool _production = false;                           // Is Production?... read from appSettings.json
        string _scoringDbConn = string.Empty;               // SQL connection string...  Depended on IsProduction setting


        public ScoringSolutionRepository(ScoringSolutionConfiguration config)
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

        // get application  information for Scoring Solution request
        public async Task<ScoringSolutionRequest> GetScoringSolutionApplication(int applicationId)
        {
            try
            {
                using (var conn = new SqlConnection(_scoringDbConn))
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@ApplicationID", applicationId);

                    var app = await conn.QueryFirstAsync<ScoringSolutionRequest>(
                                    "[scoringSolution].[GetApplicationByID]",
                                    queryParameters,
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
