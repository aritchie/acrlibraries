using System;
using System.Globalization;
using System.Text;


namespace Acr.Mail {
    
    public class MailTemplate {

        public virtual string Key { get; set; }
        public virtual string Content { get; set; }
        public virtual CultureInfo CultureInfo { get; set; }
        public virtual DateTime LastModified { get; set; }
        public virtual Encoding Encoding { get; set; }

        public MailTemplate() {
            this.Encoding = Encoding.UTF8;
            this.LastModified = DateTime.UtcNow;
        }
    }
}
