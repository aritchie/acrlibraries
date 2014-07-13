using System;
using System.Globalization;
using System.IO;
using System.Text;


namespace Acr.Mail.Loaders {
    
    public class FileMailTemplate : IMailTemplate {


        public FileMailTemplate(string templateName, string filePath, CultureInfo culture) {
            this.TemplateName = templateName;
            this.Location = filePath;
            this.Culture = culture;
        }


        public string TemplateName { get; private set; }
        public string Location { get; private set; }


        private Encoding encoding;
        public Encoding Encoding {
            get {
                if (this.encoding == null)
                    this.encoding = Extensions.DetectEncoding(this.Location);
                return this.encoding;
            }
        }


        public CultureInfo Culture { get; private set; }


        public DateTime LastModified {
            get { return File.GetLastWriteTimeUtc(this.Location); }
        }


        public Stream GetStream() {
            return new FileStream(this.Location, FileMode.Open);
        }
    }
}
