using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationWorker.DTOs
{
    public class SaveShortAppWrapper
    {
        [Required]
        public ShortApp Data { get; set; }

        public string GRecaptchaResponse { get; set; }
    }
}
