
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

        // Save init application to related tables
        public async Task<ClientApplicationLogIds> SaveApplicationToDB((ShortApp application, int logId, int userID, int lotID) applicationAndLog)
        {
            try
            {
                using (var conn = new SqlConnection(_scoringDbConn))
                {
                    var application = applicationAndLog.application;
                    var logID = applicationAndLog.logId;
                    var userID = applicationAndLog.userID;
                    var lotID = applicationAndLog.lotID;

                    var queryParameters = new DynamicParameters();

                    queryParameters.Add("@FirstName", application.FirstName);
                    queryParameters.Add("@MiddleName", application.MiddleName);
                    queryParameters.Add("@LastName", application.LastName);
                    queryParameters.Add("@HomePhone", application.PhoneNumber);
                    queryParameters.Add("@PhoneType", application.PhoneType);
                    queryParameters.Add("@Email", application.Email);
                    queryParameters.Add("@HouseNumber", application.HouseNumber);
                    queryParameters.Add("@StreetName", application.StreetName);
                    queryParameters.Add("@StreetTypeId", application.StreetTypeId);
                    queryParameters.Add("@City", application.City);
                    queryParameters.Add("@Zip", application.Zip);
                    queryParameters.Add("@StateId", application.StateId);
                    queryParameters.Add("@HouseTypeId", application.HouseTypeId);
                    queryParameters.Add("@Referrer", application.Referrer);
                    queryParameters.Add("@TimeAtResidenceYears", application.TimeAtResidenceYears);
                    queryParameters.Add("@TimeAtResidenceMonths", application.TimeAtResidenceMonths);
                    queryParameters.Add("@TimeAtJobYears", application.TimeAtJobYears);
                    queryParameters.Add("@TimeAtJobMonths", application.TimeAtJobMonths);
                    queryParameters.Add("@NetPeriodPaycheck", application.NetPeriodPaycheck);
                    queryParameters.Add("@PaymentTypeId", application.PaymentTypeId);
                    queryParameters.Add("@OtherIncome", application.OtherIncome);
                    queryParameters.Add("@OtherIncomePayPeriodId", application.OtherIncomePayPeriodId);
                    queryParameters.Add("@ActiveOrFormerMilitary", application.ActiveOrFormerMilitary);
                    queryParameters.Add("@MilitaryChoise", application.MilitaryChoise);
                    queryParameters.Add("@Ssn", application.Ssn);
                    queryParameters.Add("@CurrentlyInBankruptcy", application.CurrentlyInBankruptcy);
                    queryParameters.Add("@ClientIP", application.ClientIP);
                    queryParameters.Add("@Last4Ssn", application.Last4Ssn);

                    // pass the log ID
                    queryParameters.Add("@LogId", logID);
                    // Modified By ID
                    queryParameters.Add("@UserId",userID);
                    queryParameters.Add("@LotID", lotID);


                    ClientApplicationLogIds returnVal = await conn.QueryFirstAsync<ClientApplicationLogIds>(
                                 "[app].[SaveInitApp]",
                                 queryParameters,
                                 commandType: CommandType.StoredProcedure);

                    return await Task.FromResult(returnVal);
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

        public async Task<IEnumerable<ApplicationFlowStep>> GetApplicationFlowSteps(int logID)       
        {
            try
            {
                using (var conn = new SqlConnection(_scoringDbConn))
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@LogID", logID);

                    var stepList = await conn.QueryAsync<ApplicationFlowStep>(
                                 "[app].[GetApplicationFlowSteps]",
                                 queryParameters,
                                 commandType: CommandType.StoredProcedure);

                    return stepList;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Save original application to log table
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
