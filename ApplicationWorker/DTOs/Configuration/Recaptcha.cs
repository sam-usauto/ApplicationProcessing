using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationWorker.DTOs.Configuration
{
    public class Recaptcha
    {
        public string EnableRecaptcha { get; set; }
        public string GoogleReCaptchaProvateKey { get; set; }
        public string GoogleReCaptchaVerificationUrl { get; set; }
    }
}
