using System;
using System.Globalization;
using System.IO;
using System.Text;


namespace Acr.Mail.Loaders {
    
    public class ResourceMailTemplate : IMailTemplate {

        public ResourceMailTemplate(DateTime appDomainStart, string templateName, CultureInfo culture) {
            this.TemplateName = templateName;
            this.Culture = culture;
            this.LastModified = appDomainStart;
        }


        public string TemplateName { get; private set; }
        public string Location { get; private set; }
        public CultureInfo Culture { get; private set; }
        public DateTime LastModified { get; private set; }


        public Encoding Encoding {
            get { return Encoding.UTF8; }
        }


        public Stream GetStream() {
            throw new NotImplementedException();
        }
    }
}
