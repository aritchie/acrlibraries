using System;
using System.Globalization;
using System.IO;
using System.Text;


namespace Acr.Mail.Loaders {
    
    public class SqlMailTemplate : IMailTemplate {
        private readonly string content;


        public SqlMailTemplate(string content, DateTime lastModified, string templateName, CultureInfo culture) {
            this.content = content;
            this.TemplateName = templateName;
            this.Culture = culture;
            this.LastModified = lastModified;
        }


        public string TemplateName { get; private set; }
        public string Location { get; private set; }
        public CultureInfo Culture { get; private set; }
        public DateTime LastModified { get; private set; }


        public Encoding Encoding {
            get { return Encoding.UTF8; }
        }


        public Stream GetStream() {
            return new MemoryStream(this.Encoding.GetBytes(this.content));
        }
    }
}
