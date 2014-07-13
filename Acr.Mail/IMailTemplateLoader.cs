using System;
using System.Globalization;


namespace Acr.Mail {
    
    public interface IMailTemplateLoader {

        IMailTemplate Load(string templateName, CultureInfo cultureInfo);
    }
}
