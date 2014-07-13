using System;
using System.Globalization;
using System.Reflection;


namespace Acr.Mail.Loaders {
    
    public class ResourceMailTemplateLoader : IMailTemplateLoader {
        private readonly Assembly assembly;
        private readonly DateTime dateStarted;

        public string Namespace { get; private set; }
        public string Extension { get; private set; }


        public ResourceMailTemplateLoader(string @namespace, string extension = "xml") : this(Assembly.GetCallingAssembly(), @namespace, extension) { }

        public ResourceMailTemplateLoader(Assembly assembly, string @namespace, string extension = "xml") {
            this.Extension = extension;
            this.Namespace = @namespace;
            this.assembly = assembly;
            this.dateStarted = DateTime.UtcNow;
        }
       

        public IMailTemplate Load(string templateName, CultureInfo culture) {
            var rn = String.Format("{0}.{1}.{2}", this.Namespace, templateName, this.Extension);
            
            //return new MailTemplate {
            //    Key = rn,
            //    Content = content,
            //    Encoding = Encoding.UTF8,
            //    LastModified = this.dateStarted
            //};
            return null;
        }
    }
}
