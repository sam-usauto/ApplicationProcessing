using ApplicationProcessing.Service.ScoringSolution.Repositories;
using ApplicationProcessing.Service.TrustScienceService.DTOs;
using ApplicationProcessing.Service.TrustScienceService.DTOs.Configuration;
using ApplicationProcessing.Service.TrustScienceService.DTOs.Request;
using ApplicationProcessing.Service.TrustScienceService.DTOs.Responses;
using Common.DTOs;
using Common.DTOs.Application;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationProcessing.Service.TrustScienceService.Services
{
    public class TrustScienceService : ITrustScienceService
    {
        //private IConfiguration _config;
        private readonly TrustScienceConfiguration _config;
        private readonly TrustScienceSection _trustScienceConfigsSettings;
        private readonly ITrustScienceRepository _trustScienceRepository;
        private readonly string _scoringDbConn = string.Empty;
        private readonly bool _production = false;

        public TrustScienceService(TrustScienceConfiguration config, ITrustScienceRepository trustScienceRepository)
        {
            _config = config;

            _trustScienceRepository = trustScienceRepository;

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
                _trustScienceConfigsSettings = _config.TrustScienceConfigsPROD;
            }
            else
            {
                _trustScienceConfigsSettings = _config.TrustScienceConfigsUAT;
            }
        }


        // Proccess all submitted application that with out reports
        public async Task<int> FetchReportsFromTrustScience()
        {
            // create list of all apps that needed report
            var appList = await _trustScienceRepository.GetListOfMissingReport();

            foreach(var app in appList)
            {
                await ReprocessScoringReport(app);
            }

            return await Task.FromResult(1);
        }

        public async Task<HttpGeneralResponse> CreateFullScoringRequest(TrustScienceBatchItem item, ApplicationStepInput appInfo)
        {
            try
            {
                // convert TrustScienceBatchItem to CreateFullScoringRequest
                var reqObject = MapInputToCreateFullScoringRequest(item);

                var trustScienceResp = await SendCreateFullScoringRequest(reqObject, item, appInfo);



                return trustScienceResp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // get selected report from Trust science
        public async Task<HttpGeneralResponse> GetScoringReportByRequestID(string requestID)
        {
            try
            {
                var serviceURL = _trustScienceConfigsSettings.GetScoringReportUrl;
                var apiKey = _trustScienceConfigsSettings.ApiKey;
                // format url to contain the request ID
                var formattedUrl = String.Format(serviceURL, requestID);

                //HttpResponseMessage getResp;

                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("x-api-key", apiKey);
                client.DefaultRequestHeaders.Add("Accept", "*/*");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
                {
                    NoCache = true
                };

                var httpGeneralResponse = new HttpGeneralResponse(formattedUrl, "Get");

                try
                {
                    var result = await client.GetAsync(formattedUrl);
                    string content = await result.Content.ReadAsStringAsync();
                    httpGeneralResponse.RequestData = $"RequestId = {requestID}";
                    httpGeneralResponse.ResponseData = content;
                    httpGeneralResponse.IsSuccessStatusCode = result.IsSuccessStatusCode;
                    httpGeneralResponse.ReasonPhrase = result.ReasonPhrase;
                    httpGeneralResponse.StatusCode = (int)result.StatusCode;
                    return httpGeneralResponse;
                }
                catch (Exception ex)
                {
                    httpGeneralResponse.Errors.Add(ex.Message);
                    httpGeneralResponse.ResponseData = "Trust Science Get Scoring Report Request Failed!";
                    return httpGeneralResponse;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Private helper methods

        private async Task<int> ReprocessScoringReport(ReportReq req)
        {
            try
            {
                try
                {
                    // get report for selected application from Trust Science web
                    var getScoringReportResp = await GetScoringReportByRequestID(req.RequestId);

                    var jsonResponse = getScoringReportResp.ResponseData;

                    // check if we got an error
                    if (getScoringReportResp.IsSuccessStatusCode)
                    {
                        var responseOk = JsonConvert.DeserializeObject<ScoringReportResp>(jsonResponse);
                        // save respose data to log
                        _trustScienceRepository.SaveGetScoringReportResp(req.RequestId, req.ID, jsonResponse, responseOk, "OK");
                        return 1;
                    }
                    else
                    {
                        var responseBad = JsonConvert.DeserializeObject<ScoringReportBadResponse>(jsonResponse);
                        // save respose data to log
                        _trustScienceRepository.SaveGetScoringReportResp(req.RequestId, req.ID, jsonResponse, new ScoringReportResp(), "Bad Request");
                        return -1;
                    }
                }
                catch (Exception ex)
                {
                     throw ex; 
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Post Full Scoring request
        // remove all fields with no values
        private async Task<HttpGeneralResponse> SendCreateFullScoringRequest(CreateFullScoringRequest reqData, TrustScienceBatchItem item, ApplicationStepInput appInfo)
        {
            try
            {
                var serviceURL = _trustScienceConfigsSettings.CreateFullScoringRequestUrl;
                var apiKey = _trustScienceConfigsSettings.ApiKey;


                // convert CreateFullScoringRequest to json.. remove empty fields
                var cleanJson = JsonRemoveEmptyProperties(reqData, item);

                //var jsonReq = JsonConvert.SerializeObject(reqData);
                var stringContent = new StringContent(cleanJson, UnicodeEncoding.UTF8, "application/json");

                HttpResponseMessage response;

                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("x-api-key", apiKey);
                client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
                {
                    NoCache = true
                };

                var httpGeneralResponse = new HttpGeneralResponse(serviceURL, "Post");

                try
                {
                    response = await client.PostAsync(serviceURL, stringContent);
                }
                catch (Exception ex)
                {
                    httpGeneralResponse.Errors.Add(ex.Message);
                    httpGeneralResponse.ResponseData = "Trust Science Create Full Scoring Request Failed!";
                    return httpGeneralResponse;
                }
                var jsonResponse = string.Empty;
                var task = Task.Run(() => response.Content.ReadAsStringAsync());
                task.Wait();
                jsonResponse = task.Result;
                httpGeneralResponse.ResponseData = jsonResponse;
                httpGeneralResponse.IsSuccessStatusCode = response.IsSuccessStatusCode;
                httpGeneralResponse.ReasonPhrase = response.ReasonPhrase;
                httpGeneralResponse.StatusCode = (int)response.StatusCode;
                httpGeneralResponse.RequestData = cleanJson;

                return httpGeneralResponse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // remove all emply properties from the request
        private string JsonRemoveEmptyProperties(CreateFullScoringRequest reqData, TrustScienceBatchItem item)
        {
            var cleanSection = "";
            try
            {
                var json = JsonConvert.SerializeObject(reqData);
                JObject jo = JObject.Parse(json);
                cleanSection = "Clean Data.Person fields";
                var joBuyer = (JObject)jo.SelectToken("data.person");
                if (String.IsNullOrEmpty(reqData.data.person.birthday))
                {
                    joBuyer.Remove("birthday");
                }

                var isEmptyCurrentEmployment = false;
                var joCurrentEmployment = (JObject)jo.SelectToken("data.currentEmployment");
                if (String.IsNullOrEmpty(reqData.data.currentEmployment.employer.name) && String.IsNullOrEmpty(reqData.data.currentEmployment.employer.phone))
                {
                    joCurrentEmployment.Remove("employer");
                    isEmptyCurrentEmployment = true;
                }

                if (String.IsNullOrEmpty(reqData.data.currentEmployment.jobTitle))
                {
                    joCurrentEmployment.Remove("jobTitle");
                }

                if (isEmptyCurrentEmployment == false)
                {
                    var joEmployer = (JObject)jo.SelectToken("data.currentEmployment.employer");
                    if (String.IsNullOrEmpty(reqData.data.currentEmployment.employer.name))
                    {
                        joCurrentEmployment.Remove("name");
                    }

                    if (String.IsNullOrEmpty(reqData.data.currentEmployment.employer.phone))
                    {
                        joCurrentEmployment.Remove("phone");
                    }
                }

                var joPerson = (JObject)jo.SelectToken("data.person.addresses[0]");

                if (reqData.data.person.addresses[0].monthlyHousingCost == 0)
                {
                    joPerson.Remove("monthlyHousingCost");
                }

                var newJson = jo.ToString();
                return newJson;
            }
            catch (Exception ex)
            {
                throw new Exception("JsonRemoveEmptyProperties - " + cleanSection + ex.Message);
            }
        }

        // convert TrustScienceBatchItem to CreateFullScoringRequest
        private CreateFullScoringRequest MapInputToCreateFullScoringRequest(TrustScienceBatchItem item)
        {
            try
            {
                var requestObj = new CreateFullScoringRequest();

                requestObj.schemaVersion = "2.0.0";

                requestObj.scoring.type = "Scoring";
                requestObj.scoring.useCase = "Lending-AUTO-Preapproval";
                requestObj.scoring.package = "Regular";
                requestObj.scoring.jurisdiction.country = "USA";
                requestObj.scoring.jurisdiction.state = item.jurisdictionState;

                requestObj.scoring.generateReport = true;

                requestObj.control.tag = new List<string> { "sample" };

                requestObj.user = "US Auto Sales";
                requestObj.source = "Portal";
                requestObj.batchName = "single";
                requestObj.sendMobileInvite = false;

                requestObj.data.person.firstName = item.firstName;
                requestObj.data.person.lastName = item.lastName;
                requestObj.data.person.middleName = item.middleName;
                requestObj.data.person.birthday = item.birthday;

                //////////////////////////////
                // Create list of addresses
                //////////////////////////////
                var addressList = new List<Address>();

                if (!String.IsNullOrEmpty(item.streetAddress1) && !String.IsNullOrEmpty(item.city1) && !String.IsNullOrEmpty(item.state1))
                {
                    var address = new Address
                    {
                        streetAddress = item.streetAddress1,
                        city = item.city1,
                        state = item.state1,
                        postalCode = item.postalCode1,
                        country = item.country1,
                        residenceStatus = item.residenceStatus1,
                        monthlyHousingCost = 0,
                        isCurrentResidence = bool.Parse(item.isCurrentResidence),
                        monthsAtResidence = item.monthsAtResidence1
                    };
                    addressList.Add(address);
                }

                if (!String.IsNullOrEmpty(item.streetAddress2) && !String.IsNullOrEmpty(item.city2) && !String.IsNullOrEmpty(item.state2))
                {
                    var address = new Address
                    {
                        streetAddress = item.streetAddress2,
                        city = item.city2,
                        state = item.state2,
                        postalCode = item.postalCode2,
                        country = item.country2,
                        residenceStatus = item.residenceStatus2,
                        monthlyHousingCost = 0,
                        isCurrentResidence = false,
                        monthsAtResidence = 0
                    };
                    addressList.Add(address);
                }

                requestObj.data.person.addresses = addressList;

                ///////////////////////////////////////////
                // Create list of phones to a dictinary
                ///////////////////////////////////////////
                var phoneDic = new Dictionary<string, string>();
                if (!string.IsNullOrEmpty(item.mobilePhone))
                {
                    phoneDic.Add("mobile", FormatPhone(item.mobilePhone));
                }

                if (!string.IsNullOrEmpty(item.homePhone))
                {
                    phoneDic.Add("home", FormatPhone(item.homePhone));
                }

                if (!string.IsNullOrEmpty(item.workPhone))
                {
                    phoneDic.Add("work", FormatPhone(item.workPhone));
                }

                // see solution in https://dotnetfiddle.net/JAeLoH
                // https://stackoverflow.com/questions/43618859/how-to-create-array-of-key-value-pair-in-c
                var enumerablePhones = phoneDic.Select(p => new Dictionary<string, string>() { { p.Key, p.Value } });
                requestObj.data.person.phones = enumerablePhones;

                ////////////////////////////////////////////
                // Create list of emails to a dictinary
                ////////////////////////////////////////////
                var emailDic = new Dictionary<string, string>();
                if (!string.IsNullOrEmpty(item.email))
                {
                    emailDic.Add("primary", item.email);
                }

                // see solution in https://dotnetfiddle.net/JAeLoH
                // https://stackoverflow.com/questions/43618859/how-to-create-array-of-key-value-pair-in-c
                var enumerableEmail = emailDic.Select(p => new Dictionary<string, string>() { { p.Key, p.Value } });
                requestObj.data.person.emails = enumerableEmail;

                requestObj.data.person.birthday = item.birthday;

                if (!string.IsNullOrEmpty(item.SSN))
                {

                    var identifierList = new List<Ssn>();

                    var ssnItem = new Ssn();
                    ssnItem.type = "SSN";
                    ssnItem.value = item.SSN;
                    ssnItem.issuer = "USA";

                    identifierList.Add(ssnItem);


                    //if (!string.IsNullOrEmpty(item.driverLicenseNumber))
                    //{
                    //    identifier = new Identifier { type = "driverLicenseNumber", value = item.driverLicenseNumber };
                    //    identifierList.Add(identifier);
                    //}
                    //if (!string.IsNullOrEmpty(item.issueState))
                    //{
                    //    identifier = new Identifier { type = "issueState", value = item.issueState };
                    //    identifierList.Add(identifier);
                    //}

                    //if (!string.IsNullOrEmpty(item.driverlicenseExpirationDate))
                    //{
                    //    identifier = new Identifier { type = "driverlicenseExpirationDate", value = item.driverlicenseExpirationDate };
                    //    identifierList.Add(identifier);
                    //}

                    requestObj.data.person.identifiers = identifierList;
                }


                //var identifier = new Identifier { type = "ApplicationID", value = item.applicationID.ToString() };
                //identifierList.Add(identifier);
                //requestObj.data.person.identifiers = identifierList;

                //////////////////////////////
                // Update Employer
                //////////////////////////////
                requestObj.data.currentEmployment.employer.name = item.employerName;
                //requestObj.data.currentEmployment.employer.address.streetAddress = "";
                //requestObj.data.currentEmployment.employer.address.city = "";
                //requestObj.data.currentEmployment.employer.address.state = "";
                //requestObj.data.currentEmployment.employer.address.country = "USA";
                requestObj.data.currentEmployment.jobTitle = "";
                requestObj.data.currentEmployment.isCurrentlyEmployed = string.IsNullOrEmpty(item.isCurrentlyEmployed) ? false : bool.Parse(item.isCurrentlyEmployed);
                requestObj.data.currentEmployment.monthlyIncomeNet = Convert.ToInt32(item.monthlyIncomeNet);    //int.Parse(item.monthlyIncomeNet.ToString());
                requestObj.data.currentEmployment.employmentMonthCount = item.employmentMonthCount;
                requestObj.data.currentEmployment.paymentsPerYear = int.Parse(item.paymentsPerYear);

                //////////////////////////////
                // Update Loan Application
                //////////////////////////////
                requestObj.data.loanApplication.dateOriginated = item.dateOriginated;
                requestObj.data.loanApplication.clientCustomerId = item.clientCustomerId.ToString();
                requestObj.data.loanApplication.clientLoanReferenceId = item.clientLoanReferenceId.ToString();
                requestObj.data.loanApplication.principalAmount = Double.Parse(item.principalAmount.ToString());
                requestObj.data.loanApplication.annualInterestRate = item.annualInterestRate;
                requestObj.data.loanApplication.paymentsPerYear = int.Parse(item.paymentsPerYear);
                requestObj.data.loanApplication.totalNumberOfPayments = int.Parse(item.term);
                requestObj.data.loanApplication.termMonth = int.Parse(item.term);
                requestObj.data.loanApplication.paymentAmount = (int)item.maxMonthlyPayment;

                // Custom section
                var customList = new List<Custom>();
                var customObj = new Custom
                {
                    clientApplicationId = item.clientApplicationId.ToString(),
                    minDownPaymentPercent = item.minDownPaymentPercent.ToString(),
                    nonNormalizedIncome = item.nonNormalizedIncome,
                    applicantType = item.applicantType
                };
                customList.Add(customObj);

                requestObj.data.loanApplication.custom = customList;

                /////////////////////////////////
                // Update Auto Loan
                // We skip the vehicle section
                /////////////////////////////////
                requestObj.data.autoLoan.dealer.name = item.lotName;
                requestObj.data.autoLoan.dealer.type = "independent";    // franchise
                requestObj.data.autoLoan.dealer.address.streetAddress = item.streetAddress;
                requestObj.data.autoLoan.dealer.address.city = item.city;
                requestObj.data.autoLoan.dealer.address.postalCode = item.postalCode;
                requestObj.data.autoLoan.dealer.address.state = item.state;
                requestObj.data.autoLoan.dealer.address.country = "USA";

                /////////////////////////////////
                // Update Bureau
                /////////////////////////////////
                requestObj.data.bureau.source = "gdsLink";
                requestObj.data.bureau.dataType = "XML";
                requestObj.data.bureau.data = item.outputxml;

                return requestObj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string FormatPhone(String phone)
        {
            phone = phone.Trim();
            if (phone.StartsWith("+1"))
            {
                return phone;
            }
            return "+1" + phone;
        }



        #endregion


    }
}
