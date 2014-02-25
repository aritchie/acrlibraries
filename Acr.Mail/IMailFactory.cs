using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Mail;
using System.Threading.Tasks;


namespace Acr.Mail {
    
    public interface IMailFactory {

        Task Send(MailTemplate template, IDictionary<string, object> args, Action<MailMessage> onBeforeSend = null);
        Task Send(string content, IDictionary<string, object> args, Action<MailMessage> onBeforeSend = null);
        Task Send(string templateName, CultureInfo culture, IDictionary<string, object> args, Action<MailMessage> onBeforeSend = null);
    }
}
