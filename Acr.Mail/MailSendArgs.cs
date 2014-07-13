using System;
using System.Globalization;


namespace Acr.Mail {
    
    public class MailSendArgs {

        public string TemplateName { get; set; }
        public CultureInfo Culture { get; set; }
        public object Model { get; set; }
    }
}
