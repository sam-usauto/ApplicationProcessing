using ApplicationWorker.DTOs;
using Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationWorker.Helper
{
    public class AppValidator
    {
        public List<BrokenBusinessRule> ValidateApp(SaveShortAppWrapper saveShortAppWrapper)
        {
            var validationErrorList = new List<BrokenBusinessRule>();


            return validationErrorList;
        }
    }
}
