﻿
using ApplicationWorkerDataLayer.Interfaces;
using Common.DTOs.Application;
using Common.DTOs.Configurations.ApplicationWorker;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationWorkerDataLayer.Repositories
{
    public class ApplicationRepository : IApplicationRepository
    {
        public ApplicationProcessingConfig _config;         // ApplicationWorker configuration object
        bool _production = false;                           // Is Production?... read from appSettings.json
        string _scoringDbConn = string.Empty;               // SQL connection string...  Depended on IsProduction setting
        

        public ApplicationRepository(ApplicationProcessingConfig config)
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

        public async Task<int> SaveApplicationToDB(ShortApp application)
        {
            try
            {
                using (var conn = new SqlConnection(_scoringDbConn))
                {
                    var queryParameters = new DynamicParameters();
                    //queryParameters.Add("@FirstName", firstName);
                    //queryParameters.Add("@LastName", lastName);
                    //queryParameters.Add("@PhoneNumber", phoneNumber);
                    //queryParameters.Add("@Email", email);
                    //queryParameters.Add("@Ssn", ssn);
                    queryParameters.Add("@OriginalApp", 1);

                    var insertedId = await conn.QueryFirstAsync<int>(
                                 "[logs].[SaveOriginalApp]",
                                 queryParameters,
                                 commandType: CommandType.StoredProcedure);

                    return await Task.FromResult(insertedId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> SaveClientOriginalApplication(string firstName, string lastName, string phoneNumber, string email, string ssn, string appJson)
        {
            try
            {
                using (var conn = new SqlConnection(_scoringDbConn))
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@FirstName", firstName);
                    queryParameters.Add("@LastName", lastName);
                    queryParameters.Add("@PhoneNumber", phoneNumber);
                    queryParameters.Add("@Email", email);
                    queryParameters.Add("@Last4Ssn", ssn);
                    queryParameters.Add("@OriginalApp", appJson);

                    var insertedId = await conn.QueryFirstAsync<int>(
                                 "[logs].[SaveOriginalApp]",
                                 queryParameters,
                                 commandType: CommandType.StoredProcedure);

                    return await Task.FromResult(insertedId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
