﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Common.DTOs.Application
{
    public class SaveShortAppWrapper
    {
        public ShortApp Data { get; set; }

        public string GRecaptchaResponse { get; set; }
    }
}
