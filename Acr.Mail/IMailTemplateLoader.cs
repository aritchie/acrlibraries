using System;
using System.Globalization;


namespace Acr.Mail {
    
    public interface IMailTemplateLoader {

        MailTemplate Load(string templateName, CultureInfo cultureInfo);
    }
}
