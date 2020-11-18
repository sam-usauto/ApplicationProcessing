using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ApplicationProcessing.Service.PointPredictiveService.DTOs;
using ApplicationProcessing.Service.PointPredictiveService.DTOs.PointPredictive;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ApplicationProcessing.Service.PointPredictiveService.Services
{
    public class PointPredictiveService : IPointPredictiveService
    {
        private readonly PointPredictiveConfig _config;
        private readonly string _scoringDbConn = string.Empty;
        private readonly bool _production = false;
        private readonly PointPredictiveConfigSection _pointPredictiveConfigSection;
        private string _currentToken = string.Empty;   // token is valid for over a day so load the token for the first time to this variable

        public PointPredictiveService(PointPredictiveConfig config)
        {
            _config = config;

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

            // read the application configuration and save it.
            if (_production)
            {
                _pointPredictiveConfigSection = _config.PointPredictiveConfigsPROD;
            }
            else
            {
                _pointPredictiveConfigSection = _config.PointPredictiveConfigsUAT;
            }
        }


        public async Task<PointPredictiveReportResp> GetPointPredictiveScoreAsync(PointPredictiveScoreReq pointPredictiveScoreReq)
        {
            /****************** DO NOT CHANGE THE CALLING ORDER  *******************/

            // TODO: would not accept app without Primary DOB... FIXED THE ISSUE
            pointPredictiveScoreReq.primary_borrower.date_of_birth = 19900101;

            // remove fields without data from the json
            var cleanJason = JsonRemoveEmptyProperties(pointPredictiveScoreReq);

            var serviceTokenURL = _pointPredictiveConfigSection.BaseURL;
            serviceTokenURL = serviceTokenURL.EndsWith(@"/")
                                ? serviceTokenURL + "score"
                                : serviceTokenURL + "/score";

            var httpRespone = new HttpGeneralResponse(serviceTokenURL, "Post");

            try
            {
                var stringContent = new StringContent(cleanJason, UnicodeEncoding.UTF8, "application/json");

                var client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                await GetAccessToken();
                var token = _currentToken;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);

                HttpResponseMessage response;
                response = await client.PostAsync(serviceTokenURL, stringContent);

                var jsonResponse = await response.Content.ReadAsStringAsync();

                // Test for PP Service validation errors
                if (response.IsSuccessStatusCode == false && jsonResponse.Contains("validation_error"))
                {
                    var validationError = JsonConvert.DeserializeObject<ValidationError>(jsonResponse);
                    var errors = string.Empty;
                    //foreach(var validationError in validationErrorList)
                    //{
                    errors += $"Message - {validationError.Validation_error.Message} Object - {validationError.Validation_error.Object}  | ";

                    //}
                    throw new Exception($"Point Predictive Service Validation Error(s): {errors}");
                }

                // Test for PP Service other erross
                if (response.IsSuccessStatusCode == false && jsonResponse.Contains("error_detail"))
                {
                    var serverOtherError = JsonConvert.DeserializeObject<ServerOtherError>(jsonResponse);
                    var errors = string.Empty;
                    //foreach(var validationError in validationErrorList)
                    //{
                    errors += $"Error Detail - {serverOtherError.Error_detail} System Error - {serverOtherError.System_error} Most probably invalid field (check CleanRequest in PP_Return table)  ";

                    //}
                    throw new Exception($"Point Predictive Service other error(s): {errors}");
                }


                if (response.IsSuccessStatusCode == false)
                {

                    throw new Exception($"Unknow Error: {response.StatusCode} - {response.ReasonPhrase}");
                }

                // TODO - Check if got Validation Error

                PointPredictiveScoreResp pointPredictiveScore = null;

                if (response.IsSuccessStatusCode)
                {
                    pointPredictiveScore = JsonConvert.DeserializeObject<PointPredictiveScoreResp>(jsonResponse);
                }
                httpRespone.StatusCode = (int)response.StatusCode;
                httpRespone.ReasonPhrase = response.ReasonPhrase;
                httpRespone.IsSuccessStatusCode = true;

                var respObj = new PointPredictiveReportResp
                {
                    PointPredictiveScoreResp = pointPredictiveScore,
                    HttpRespone = httpRespone
                };

                return respObj;
            }
            catch (Exception ex)
            {
                //throw ex;
                httpRespone.IsSuccessStatusCode = false;
                var respObj = new PointPredictiveReportResp
                {
                    Exception = ex,
                    ExceptionFile = @"Core\Services\PointPredictiveService.cs",
                    ExceptionInArea = "GetPointPredictiveScoreAsync",
                    PointPredictiveScoreResp = null,
                    HttpRespone = httpRespone
                };

                return respObj;
            }
        }


        public string JsonRemoveEmptyProperties(PointPredictiveScoreReq pointPredictiveScoreReq)
        {
            return CleanJson(pointPredictiveScoreReq);
        }

        #region Private methods

        // get token from Point Predictive
        private async Task<string> GetAccessToken()
        {
            if (_currentToken != string.Empty)
            {
                return await Task.FromResult<String>(null);  // we already have a token
            }

            var serviceTokenURL = _pointPredictiveConfigSection.BaseURL;
            serviceTokenURL = serviceTokenURL.EndsWith(@"/")
                                ? serviceTokenURL + "auth"
                                : serviceTokenURL + "/auth";
            // Build the Request
            var req = new TokenReq();
            req.username = _pointPredictiveConfigSection.UserID;
            req.password = _pointPredictiveConfigSection.Password;

            var jsonReq = JsonConvert.SerializeObject(req);
            var stringContent = new StringContent(jsonReq, UnicodeEncoding.UTF8, "application/json");
            HttpResponseMessage response; // = new HttpResponseMessage();

            var client = new HttpClient();
            try
            {
                response = await client.PostAsync(serviceTokenURL, stringContent);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var respObj = JsonConvert.DeserializeObject<TokenResp>(jsonResponse);
            _currentToken = respObj.access_token;
            return "";
        }

        // remove all emply properties from the request
        private string CleanJson(PointPredictiveScoreReq pointPredictiveScoreReq)
        {
            var cleanSection = "";
            try
            {
                var json = JsonConvert.SerializeObject(pointPredictiveScoreReq);
                JObject jo = JObject.Parse(json);
                cleanSection = "Remove user_defined_fields";
                jo.Remove("user_defined_fields");
                cleanSection = "Clean Buyer";
                CleanBuyer(jo, pointPredictiveScoreReq);
                cleanSection = "Clean Co-Buyer";
                CleanCoBuyer(jo, pointPredictiveScoreReq);
                cleanSection = "Clean Loan Info";
                CleanLoanInfo(jo, pointPredictiveScoreReq);
                cleanSection = "Clean Credit Info";
                CleanCreditInfo(jo, pointPredictiveScoreReq);
                cleanSection = "Clean Vehicle Info";
                CleanVehicleInfo(jo, pointPredictiveScoreReq);
                cleanSection = "Clean Alternate Fields";
                CleanAlternateFields(jo, pointPredictiveScoreReq);

                var newJson = jo.ToString();

                return newJson;
            }
            catch (Exception ex)
            {
                throw new Exception("JsonRemoveEmptyProperties - " + cleanSection + ex.Message);
            }
        }

        private void CleanAlternateFields(JObject jo, PointPredictiveScoreReq pointPredictiveScoreReq)
        {
            var alternateFields = pointPredictiveScoreReq.alternate_fields;
            var joAlternateFields = (JObject)jo.SelectToken("alternate_fields");
            if (alternateFields.primary_borrower_year_of_birth == -999999)
            {
                joAlternateFields.Remove("primary_borrower_year_of_birth");
            }
            if (alternateFields.co_borrower_year_of_birth == -999999)
            {
                joAlternateFields.Remove("co_borrower_year_of_birth");
            }
        }

        private void CleanVehicleInfo(JObject jo, PointPredictiveScoreReq pointPredictiveScoreReq)
        {
            var vehicleInfo = pointPredictiveScoreReq.vehicle_information;
            var joVehicleInfo = (JObject)jo.SelectToken("vehicle_information");
            if (vehicleInfo.sale_price == -999999)
            {
                joVehicleInfo.Remove("sale_price");
            }
            if (vehicleInfo.year_of_manufacture == -999999)
            {
                joVehicleInfo.Remove("year_of_manufacture");
            }
            if (vehicleInfo.mileage == -999999)
            {
                joVehicleInfo.Remove("mileage");
            }
        }

        private void CleanCreditInfo(JObject jo, PointPredictiveScoreReq pointPredictiveScoreReq)
        {
            var creditInfo = pointPredictiveScoreReq.credit_information;
            var joCreditInfo = (JObject)jo.SelectToken("credit_information");
            if (creditInfo.credit_score == -999999)
            {
                joCreditInfo.Remove("credit_score");
            }
            if (creditInfo.time_in_file == -999999)
            {
                joCreditInfo.Remove("time_in_file");
            }
            if (creditInfo.debt_to_income_ratio == -999999)
            {
                joCreditInfo.Remove("debt_to_income_ratio");
            }
            if (creditInfo.number_of_credit_inquiries_in_previous_two_weeks == -999999)
            {
                joCreditInfo.Remove("number_of_credit_inquiries_in_previous_two_weeks");
            }
            if (creditInfo.highest_credit_limit_from_trades_in_good_standing == -999999)
            {
                joCreditInfo.Remove("highest_credit_limit_from_trades_in_good_standing");
            }
            if (creditInfo.total_number_of_trade_lines == -999999)
            {
                joCreditInfo.Remove("total_number_of_trade_lines");
            }
            if (creditInfo.number_of_open_trade_lines == -999999)
            {
                joCreditInfo.Remove("number_of_open_trade_lines");
            }
            if (creditInfo.number_of_positive_auto_trades == -999999)
            {
                joCreditInfo.Remove("number_of_positive_auto_trades");
            }
            if (creditInfo.number_of_mortgage_trade_lines == -999999)
            {
                joCreditInfo.Remove("number_of_mortgage_trade_lines");
            }
            if (creditInfo.number_of_authorized_trade_lines == -999999)
            {
                joCreditInfo.Remove("number_of_authorized_trade_lines");
            }
            if (creditInfo.date_of_oldest_trade_line == -999999)
            {
                joCreditInfo.Remove("date_of_oldest_trade_line");
            }

        }

        private void CleanLoanInfo(JObject jo, PointPredictiveScoreReq pointPredictiveScoreReq)
        {
            var loanInfo = pointPredictiveScoreReq.loan_information;
            var joLoanInfo = (JObject)jo.SelectToken("loan_information");
            if (loanInfo.loan_amount == -999999)
            {
                joLoanInfo.Remove("loan_amount");
            }
            if (loanInfo.total_down_payment == -999999)
            {
                joLoanInfo.Remove("total_down_payment");
            }
            if (loanInfo.cash_down_payment == -999999)
            {
                joLoanInfo.Remove("cash_down_payment");
            }
            if (loanInfo.term == -999999)
            {
                joLoanInfo.Remove("term");
            }
            if (loanInfo.payment_to_income_ratio == -999999)
            {
                joLoanInfo.Remove("payment_to_income_ratio");
            }
        }

        private void CleanBuyer(JObject jo, PointPredictiveScoreReq pointPredictiveScoreReq)
        {
            var primaryBorrower = pointPredictiveScoreReq.primary_borrower;
            var joBuyer = (JObject)jo.SelectToken("primary_borrower");
            if (primaryBorrower.home_phone_number == -999999)
            {
                joBuyer.Remove("home_phone_number");
            }
            if (primaryBorrower.cell_phone_number == -999999)
            {
                joBuyer.Remove("cell_phone_number");
            }
            if (primaryBorrower.work_phone_number == -999999)
            {
                joBuyer.Remove("work_phone_number");
            }
            if (primaryBorrower.date_of_birth == -999999)
            {
                joBuyer.Remove("date_of_birth");
            }
            // TODO - ssn could be "Invalid"
            if (primaryBorrower.ssn == -999999)
            {
                joBuyer.Remove("ssn");
            }
            if (String.IsNullOrEmpty(primaryBorrower.rentmortgage))
            {
                joBuyer.Remove("rentmortgage");
            }
            if (primaryBorrower.annual_income == -999999)
            {
                joBuyer.Remove("annual_income");
            }
            if (primaryBorrower.employer_phone == -999999)
            {
                joBuyer.Remove("employer_phone");
            }
            if (primaryBorrower.customer_since_date == -999999)
            {
                joBuyer.Remove("customer_since_date");
            }
        }

        private void CleanCoBuyer(JObject jo, PointPredictiveScoreReq pointPredictiveScoreReq)
        {
            var coBuyer = pointPredictiveScoreReq.co_borrower;
            var joCoBuyer = (JObject)jo.SelectToken("co_borrower");
            if (coBuyer.home_phone_number == -999999)
            {
                joCoBuyer.Remove("home_phone_number");
            }
            if (coBuyer.work_phone_number == -999999)
            {
                joCoBuyer.Remove("work_phone_number");
            }
            if (coBuyer.cell_phone_number == -999999)
            {
                joCoBuyer.Remove("cell_phone_number");
            }
            if (coBuyer.date_of_birth == -999999)
            {
                joCoBuyer.Remove("date_of_birth");
            }
            if (coBuyer.annual_income == -999999)
            {
                joCoBuyer.Remove("annual_income");
            }
            if (coBuyer.credit_score == -999999)
            {
                joCoBuyer.Remove("credit_score");
            } 

        }

        #endregion


    }
}
