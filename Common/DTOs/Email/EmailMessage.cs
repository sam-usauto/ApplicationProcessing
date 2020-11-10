using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DTOs.Email
{
    public class EmailMessage
    {
        public EmailMessage()
        {
            Attachments = new List<EmailAttachment>();
            Recipients = new List<string>();
            CcRecipients = new List<string>();
        }

        public string Body { get; set; }
        public string Subject { get; set; }
        public List<string> Recipients { get; set; }
        public List<string> CcRecipients { get; set; }
        public string Sender { get; set; }
        public bool IsBodyHtml { get; set; }
        public string CreateUserId { get; set; }

        public List<EmailAttachment> Attachments { get; set; }
    }
}
