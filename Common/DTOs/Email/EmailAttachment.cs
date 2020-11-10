using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DTOs.Email
{
    public class EmailAttachment
    {
        public string FileName { get; set; }

        public byte[] ContentBytes { get; set; }
    }
}
