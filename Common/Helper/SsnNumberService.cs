using Common.DTOs;
using Common.DTOs.Configurations.ApplicationWorker;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helper
{
    public class SsnNumberService
    {
        public ApplicationProcessingConfig _config;         // ApplicationWorker configuration object

        public SsnNumberService(ApplicationProcessingConfig config)
        {
            _config = config;
        }

        public static string EncriptSsn(string plainSsn)
        {
            return "1212wewew&&^%^%^%%^%";
        }

        private async Task<HttpGeneralResponse> DecryptSsn(string ssnEncrypted)
        {
            // _ppConfig
            var serviceTokenURL = _config.SsnDecryptUrl;

            var req = new
            {
                SsnEncrypted = ssnEncrypted
            };

            var jsonReq = JsonConvert.SerializeObject(req);
            var stringContent = new StringContent(jsonReq, UnicodeEncoding.UTF8, "application/json");
            HttpResponseMessage response;

            var client = new HttpClient();
            var httpGeneralResponse = new HttpGeneralResponse(serviceTokenURL, "Post");

            try
            {
                response = await client.PostAsync(serviceTokenURL, stringContent);
            }
            catch (Exception ex)
            {
                httpGeneralResponse.Errors.Add(ex.Message);
                httpGeneralResponse.ResponseData = "Invalid Protected SSN";
                return httpGeneralResponse;
            }
            var _ssnDecrypted = string.Empty;
            var jsonResponse = string.Empty;
            httpGeneralResponse.RequestData = ssnEncrypted;

            if (response.IsSuccessStatusCode)
            {
                jsonResponse = await response.Content.ReadAsStringAsync();
                _ssnDecrypted = JsonConvert.DeserializeObject<string>(jsonResponse);
                httpGeneralResponse.ResponseData = _ssnDecrypted;
            }
            else
            {
                httpGeneralResponse.ResponseData = "Invalid Protected SSN";
                httpGeneralResponse.StatusCode = (int)response.StatusCode;
                httpGeneralResponse.ReasonPhrase = response.ReasonPhrase;
            }

            httpGeneralResponse.IsSuccessStatusCode = response.IsSuccessStatusCode;

            return httpGeneralResponse;
        }

        public static string LastFourDigits(string plainSsn)
        {
            return plainSsn.Substring(Math.Max(0, plainSsn.Length - 4));
        }

    }
}
