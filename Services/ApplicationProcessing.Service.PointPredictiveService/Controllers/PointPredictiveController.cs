using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using ApplicationProcessing.Service.PointPredictiveService.DTOs;
using ApplicationProcessing.Service.PointPredictiveService.DTOs.PointPredictive;
using ApplicationProcessing.Service.PointPredictiveService.Repositories;
using ApplicationProcessing.Service.PointPredictiveService.Services;
using Common.DTOs.Application;
//using ApplicationWorkerDataLayer.Interfaces;
//using Common.DTOs.Application;
//using Common.DTOs.Configurations;
using Common.DTOs.Configurations.ApplicationWorker;
using Common.Helper;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationProcessing.Service.PointPredictiveService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PointPredictiveController : ControllerBase
    {
        private readonly string _version = "1.0.0";
        private readonly string _lastUpdated = "11/18/2020";

        private readonly PointPredictiveConfig _config;
        private readonly IPointPredictiveService _pointPredictiveService;

        private readonly IPointPredictiveRepository _pointPredictiveRepository;
        private readonly SsnNumberService _ssnNumberService;

        private ApplicationStepInput _applicationStepInput = null;

        private int _dapperTimeOut = 90;

        public PointPredictiveController(PointPredictiveConfig config, 
                                            IPointPredictiveRepository pointPredictiveRepository,
                                            IPointPredictiveService pointPredictiveService)
        {
            _config = config;
            _pointPredictiveRepository = pointPredictiveRepository;
            _ssnNumberService = new SsnNumberService(_config.SsnEncryptUrl, _config.SsnDecryptUrl);
            _dapperTimeOut = _config.DapperDefaultTimeOut;
            _pointPredictiveService = pointPredictiveService;
        }

        [HttpPost]
        [Route("Execute")]
        //public async Task<HttpResponseMessage> Execute([FromBody] (int applicationID, int logId, int userID) appInfo )
        public async Task<IActionResult> Execute([FromBody] ApplicationStepInput appInfo)
        {
            try
            {
                _applicationStepInput = appInfo;

                // collect all the data needed by Point Predictive for the request
                var app = await _pointPredictiveRepository.GetApplicationDetailsByAppIdAsync(appInfo.ApplicationID, _dapperTimeOut);

                if (app == null)
                {
                    return BadRequest("Failed collecting application information by applicationId");
                }

                // call the SSN decryption
                if (String.IsNullOrEmpty(app.SSN) == false)
                {
                    var ssnResp = await _ssnNumberService.UnprotectSsn(app.SSN);
                    var unprotectedSsn = ssnResp.ResponseData;
                    app.SSN = unprotectedSsn;
                }

                // make sure data is clean
                CleanApp(app);

                // Us auto Application structure is diffrent than Point Predictive  structure
                var applicationInput = MapToApplicationInput(app);
                var pointPredictiveScoreReq = new PointPredictiveScoreReq
                {
                    administrative_fields = applicationInput.AdministrativeFields,
                    co_borrower = applicationInput.CoBorrower,
                    loan_information = applicationInput.LoanInfo,
                    primary_borrower = applicationInput.PrimaryBorrower,
                    credit_information = applicationInput.CreditInfo,
                    vehicle_information = applicationInput.VehicleInfo,
                    alternate_fields = applicationInput.AlternateFields,
                    user_defined_fields = applicationInput.UserDefinedFields
                };

                PointPredictiveReportResp pointPredictiveReportResp = await _pointPredictiveService.GetPointPredictiveScoreAsync(pointPredictiveScoreReq);

                // the request after cleaning emply fields
                var cleanReq = _pointPredictiveService.JsonRemoveEmptyProperties(pointPredictiveScoreReq).Replace("'", "''");

                var saveResp = await _pointPredictiveRepository.SavePointPredictiveScoreAsync(
                                        pointPredictiveReportResp,
                                        pointPredictiveScoreReq,
                                        app,
                                        "USAUTOSALES\\Sam Aloni",
                                        cleanReq,
                                        _applicationStepInput.ApplicationID,
                                        ""
                                        );

                //  got some errors from PP Server
                if(saveResp.Exception != null)
                {
                    var shortResp = new ShortPointPredictiveReportResp();
                    shortResp.IsSuccessStatusCode = false;
                    shortResp.Errors = $"Error Code#: {saveResp.NewSaveId}";
                    return Ok(shortResp);
                }

                //// get the PP status to be diplayed to the user
                //var pointPredictiveLastScore = await _ppService.GetPointPredictiveLastScoreAsync(creditId);

                //// TODO
                //// Test if reponse contains errors

                //// load PP status info
                //pointPredictiveReportResp.PointPredictiveScoreResp.UWStatusId = pointPredictiveLastScore.UWStatusId;
                //pointPredictiveReportResp.PointPredictiveScoreResp.Status = pointPredictiveLastScore.Status;
                //pointPredictiveReportResp.PointPredictiveScoreResp.DaysSinceLastCall = pointPredictiveLastScore.DaysSinceLastCall;

                //// save the entry GUID for easy access on the screen
                //pointPredictiveReportResp.SavedReqRespID = saveResp.NewSaveId;
                //// save the score for easy access on the screen
                //pointPredictiveReportResp.FraudScore =
                //        pointPredictiveReportResp.PointPredictiveScoreResp.Application_fraud_information.fraud_score;

                //// send short response
                //var shortPointPredictiveReportResp = MapRespToShortResp(pointPredictiveReportResp);


                ////Response.Headers.Add("access-control-allow-origin", "*");

                //if (_underwritingConfiguration.ReturnShortResp == true)
                //{
                //    // return short results
                //    return Ok(shortPointPredictiveReportResp);
                //}
                //else
                //{
                //    // return full results
                //    return Ok(pointPredictiveReportResp);
                //}


            return Ok(saveResp);
            }
            catch (SqlException ex)
            {
                throw ex;
                //return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                throw ex;
                //return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }



        [HttpGet]
        [Route("Info")]
        //public async Task<HttpResponseMessage> Execute([FromBody] (int applicationID, int logId, int userID) appInfo )
        public IActionResult Info()
        {
            return Ok($"Point Predictive Service - Version: {_version} Last Updated On: {_lastUpdated}.");
        }


        #region Private methods

        private void CleanApp(UsAutoApp app)
        {
            // sometime we are getting spaces in the phone number..
            // needed to remove... can not convert to number
            app.CellPhoneNumber = app.CellPhoneNumber.Replace(" ", "");
            app.HomePhoneNumber = app.HomePhoneNumber.Replace(" ", "");
            app.WorkPhoneNumber = app.WorkPhoneNumber.Replace(" ", "");
            app.CobCellPhoneNumber = app.CobCellPhoneNumber.Replace(" ", "");
            app.CobHomePhoneNumber = app.CobHomePhoneNumber.Replace(" ", "");
            app.CobWorkPhoneNumber = app.CobWorkPhoneNumber.Replace(" ", "");
            app.EmployerPhone = app.EmployerPhone.Replace(" ", "");
            // "-999999" => empty date
            app.CustomerSinceDate = String.IsNullOrEmpty(app.CustomerSinceDate) ? "-999999" : app.CustomerSinceDate;
        }


        private PointPredictiveApp MapToApplicationInput(UsAutoApp app)
        {
            var applicationInput = new PointPredictiveApp();

            try
            {

                MapToAdministrativeFields(applicationInput.AdministrativeFields, app);
                MapToPrimaryBorrower(applicationInput.PrimaryBorrower, app);
                MapToLoanInfo(applicationInput.LoanInfo, app);
                MapToCreditInfo(applicationInput.CreditInfo, app);
                MapToVehicleInfo(applicationInput.VehicleInfo, app);
                MapToCoBorrower(applicationInput.CoBorrower, app);
                MapToAlternateFields(applicationInput.AlternateFields, app);
                MapToUserDefinedFields(applicationInput.UserDefinedFields, app);

                return applicationInput;
            }
            catch (Exception ex)
            {
                throw new Exception("MapToApplicationInput : " + ex.Message);
            }

        }

        private void MapToUserDefinedFields(UserDefinedFields userDefinedFields,
                    UsAutoApp app)
        {
            try
            {
                userDefinedFields.user_defined_field_1 = "";  // app.user_defined_field_1
                userDefinedFields.user_defined_field_2 = "";  // app.user_defined_field_2
                userDefinedFields.user_defined_field_3 = "";  // app.user_defined_field_3
                userDefinedFields.user_defined_field_4 = "";  // app.user_defined_field_4
                userDefinedFields.user_defined_field_5 = "";  // app.user_defined_field_5
                userDefinedFields.user_defined_field_6 = "";  // app.user_defined_field_6
                userDefinedFields.user_defined_field_7 = "";  // app.user_defined_field_7
            }
            catch (Exception ex)
            {
                throw new Exception("MapToUserDefinedFields " + ex.Message);
            }
        }

        private void MapToAlternateFields(AlternateFields alternateFields,
                            UsAutoApp app)
        {
            alternateFields.primary_borrower_year_of_birth = -999999;
            alternateFields.co_borrower_year_of_birth = -999999;
            alternateFields.primary_borrower_credit_score_range = "";
            alternateFields.co_borrower_credit_score_range = "";
        }

        private void MapToCoBorrower(CoBorrower coBorrower,
                            UsAutoApp app)
        {
            try
            {
                coBorrower.first_name = app.CobFirstName;
                coBorrower.last_name = app.CobLastName;
                coBorrower.street_address = app.CobStreetAddress;
                coBorrower.city = app.CobCity;
                coBorrower.state = app.CobState;
                coBorrower.zip = app.CobZip;
                coBorrower.home_phone_number = PhoneToNumber(app.CobHomePhoneNumber);
                coBorrower.work_phone_number = PhoneToNumber(app.CobWorkPhoneNumber);
                coBorrower.cell_phone_number = PhoneToNumber(app.CobCellPhoneNumber);
                coBorrower.e_mail_address = app.CobEmail;
                coBorrower.date_of_birth = DateToNumber(app.CobDateofBirth);
                coBorrower.annual_income = MoneyToWholeNumber(app.CobAnnualIncome);
                coBorrower.relationship = app.CobRelationship;
                coBorrower.credit_score = StringToInt(app.CobCreditScore);
            }
            catch (Exception ex)
            {
                throw new Exception("MapToCoBorrower " + ex.Message);
            }

        }

        private void MapToVehicleInfo(VehicleInfo vehicleInfo,
                            UsAutoApp app)
        {
            try
            {
                vehicleInfo.sale_price = DecimalToInt(app.SalePrice);
                vehicleInfo.year_of_manufacture = StringToInt(app.YearofManufacture);
                vehicleInfo.make = app.Make;
                vehicleInfo.model = app.Model;
                vehicleInfo.vin = app.VIN;
                vehicleInfo.new_or_used = app.NeworUsed;
                vehicleInfo.mileage = StringToInt(app.Mileage);
            }
            catch (Exception ex)
            {
                throw new Exception("MapToVehicleInfo " + ex.Message);
            }
        }

        private void MapToCreditInfo(CreditInfo creditInfo,
                            UsAutoApp app)
        {
            try
            {
                creditInfo.credit_score = StringToInt(app.CreditScore);
                creditInfo.time_in_file = StringToInt(app.TimeinFile);
                creditInfo.debt_to_income_ratio = StringToInt(app.DebtToIncomeRatio); ;
                creditInfo.number_of_credit_inquiries_in_previous_two_weeks = StringToInt(app.NumberofCreditInquiriesinprevioustwoweeks);
                creditInfo.highest_credit_limit_from_trades_in_good_standing = IntToInt(app.HighestCreditLimitfromTradesinGoodStanding);
                creditInfo.total_number_of_trade_lines = IntToInt(app.TotalNumberofTradeLines);
                creditInfo.number_of_open_trade_lines = IntToInt(app.NumberofOpenTradeLines);
                creditInfo.number_of_positive_auto_trades = IntToInt(app.NumberofPositiveAutoTrades);
                creditInfo.number_of_mortgage_trade_lines = IntToInt(app.NumberofMortgageTradeLines);
                creditInfo.number_of_authorized_trade_lines = StringToInt(app.NumberofAuthorizedTradeLines);
                creditInfo.date_of_oldest_trade_line = StringToInt(app.DateofOldestTradeLine);
            }
            catch (Exception ex)
            {
                throw new Exception("MapToUserDefinedFields " + ex.Message);
            }
        }

        private void MapToLoanInfo(LoanInfo loanInfo,
                                    UsAutoApp app)
        {
            try
            {
                loanInfo.loan_amount = DecimalToInt(app.LoanAmount);
                loanInfo.total_down_payment = DecimalToInt(app.TotalDownPayment);
                loanInfo.cash_down_payment = DecimalToInt(app.CashDownPayment);
                loanInfo.term = StringToInt(app.Term);
                loanInfo.payment_to_income_ratio = FloatToInt(app.PaymentToIncomeRatio);
            }
            catch (Exception ex)
            {
                throw new Exception("MapToLoanInfo " + ex.Message);
            }
        }

        private void MapToPrimaryBorrower(PrimaryBorrower primaryBorrower,
                                            UsAutoApp app)
        {
            try
            {
                primaryBorrower.first_name = app.FirstName;
                primaryBorrower.last_name = app.LastName;
                primaryBorrower.street_address = app.StreetAddress;
                primaryBorrower.city = app.City;
                primaryBorrower.state = app.State;
                primaryBorrower.zip = app.Zip;
                primaryBorrower.country = app.Country;
                primaryBorrower.home_phone_number = StringToInt(app.HomePhoneNumber);
                primaryBorrower.work_phone_number = StringToInt(app.WorkPhoneNumber);
                primaryBorrower.cell_phone_number = StringToInt(app.CellPhoneNumber);
                primaryBorrower.e_mail_address = app.Email;
                primaryBorrower.date_of_birth = DateToNumber(app.DateofBirth);
                primaryBorrower.ssn = app.SSN == "Invalid Protected SSN" ? -999999 : StringToInt(app.SSN);
                primaryBorrower.rentown = app.RentOwn;
                //primaryBorrower.rentmortgage = StringToInt(app.RentMortgage);
                primaryBorrower.rentmortgage = app.RentMortgage;
                primaryBorrower.months_at_residence = app.MonthsatResidence;
                primaryBorrower.occupation = app.Occupation;
                primaryBorrower.annual_income = DecimalToInt(app.AnnualIncome);
                primaryBorrower.self_employed = app.SelfEmployed;
                primaryBorrower.employer_name = app.EmployerName;
                primaryBorrower.employer_street_address = app.EmployerStreetAddress;
                primaryBorrower.employer_city = app.EmployerCity;
                primaryBorrower.employer_state = app.EmployerState;
                primaryBorrower.employer_zip = app.EmployerZIP;
                primaryBorrower.employer_phone = StringToInt(app.EmployerPhone);
                primaryBorrower.months_at_employer = app.MonthsatEmployer;
                primaryBorrower.other_bank_relationships = app.OtherBankRelationships;
                primaryBorrower.customer_since_date = StringToInt(app.CustomerSinceDate);
                primaryBorrower.channel = app.Channel;
            }
            catch (Exception ex)
            {
                throw new Exception("MapToPrimaryBorrower " + ex.Message);
            }
        }

        private void MapToAdministrativeFields(AdministrativeFields administrativeFields,
                                               UsAutoApp app)
        {
            try
            {
                //administrativeFields.lender_identifier = app.LenderIdentifier;
                administrativeFields.application_identifier = app.ApplicationIdentifier.ToString();
                administrativeFields.application_date = DateToNumber(app.ApplicationDate);
                administrativeFields.application_status = app.ApplicationStatus;
                administrativeFields.dealer_identifier = app.DealerIdentifier;
            }
            catch (Exception ex)
            {
                throw new Exception("MapToAdministrativeFields " + ex.Message);
            }

        }

        private Int64 SsnToInt(string ssn)
        {
            if (String.IsNullOrEmpty(ssn))
            {
                return -999999;      // signal no value
            }
            ssn = ssn.Replace("-", String.Empty);
            ssn = ssn.Replace(" ", String.Empty);
            ssn = ssn.Trim();
            return Convert.ToInt64(ssn);
        }

        private Int64 IntToInt(int? value)
        {
            if (value == null)
            {
                return -999999;      // signal no value
            }
            return value.GetValueOrDefault();
        }

        private Int64 FloatToInt(float value)
        {
            return Convert.ToInt64(value);
        }

        private Int64 DecimalToInt(decimal? value)
        {
            if (value == null)
            {
                return -999999;      // signal no value
            }
            return Decimal.ToInt64(Math.Floor(value.GetValueOrDefault()));
        }

        private Int64 DecimalToInt(decimal value)
        {
            return Decimal.ToInt64(Math.Floor(value));
        }

        private Int64 StringToInt(string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return -999999;      // signal no value
            }
            value = value.Trim();
            return Int64.Parse(value);
        }

        private Int64 MoneyToWholeNumber(string amount)
        {
            try
            {
                amount = amount.Trim();
                // must remove the ".99"
                var decimaPos = amount.IndexOf('.');
                if (decimaPos > 0)
                {
                    amount = amount.Substring(0, decimaPos);
                }
                if (String.IsNullOrEmpty(amount))
                {
                    return -999999;      // signal no value
                }
                return Int64.Parse(amount);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Int64 PhoneToNumber(string phone)
        {
            if (String.IsNullOrEmpty(phone))
            {
                return -999999;      // signal no value
            }
            phone = phone.Replace("-", String.Empty);
            phone = phone.Replace(" ", String.Empty);
            phone = phone.Trim();
            return Int64.Parse(phone); ;
        }

        private Int64 DateToNumber(string date)
        {
            if (String.IsNullOrEmpty(date))
            {
                return -999999;
            }
            return Int64.Parse(date);
        }

        #endregion
    }
}

