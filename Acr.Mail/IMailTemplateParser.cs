using System;
using System.Net.Mail;


namespace Acr.Mail {
    
    public interface IMailTemplateParser {

        MailMessage Parse(IMailTemplate template, object model);
    }
}
