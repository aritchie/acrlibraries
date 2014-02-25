using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;


namespace Acr.Mail {
    
    public class MailFactory : IMailFactory {
        private readonly IMailTemplateLoader loader;
        private readonly IMailTemplateParser parser;
        private readonly IMailSender sender;


        public MailFactory(IMailTemplateLoader loader, IMailTemplateParser parser, IMailSender sender) {
            this.loader = loader;
            this.parser = parser;
            this.sender = sender;
        }

        #region IMailFactory Members

        public Task Send(MailTemplate template, IDictionary<string, object> args, Action<MailMessage> onBeforeSend = null) {
            var mail = this.parser.Parse(template, args);
            if (onBeforeSend != null) {
                onBeforeSend(mail);
            }
            return this.sender.Send(mail);          
        }


        public Task Send(string content, IDictionary<string, object> args, Action<MailMessage> onBeforeSend = null) {
            return this.Send(new MailTemplate {
                Content = content,
                Encoding = Encoding.Default,
                CultureInfo = CultureInfo.CurrentCulture
            }, args, onBeforeSend);
        }


        public Task Send(string templateName, CultureInfo culture, IDictionary<string, object> args, Action<MailMessage> onBeforeSend = null) {
            var template = this.loader.Load(templateName, culture);
            return this.Send(template, args, onBeforeSend);
        }

        #endregion
    }
}
