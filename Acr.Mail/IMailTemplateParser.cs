using System;
using System.Collections.Generic;
using System.Net.Mail;


namespace Acr.Mail {
    
    public interface IMailTemplateParser {

        MailMessage Parse(MailTemplate template, IDictionary<string, object> args);
    }
}
