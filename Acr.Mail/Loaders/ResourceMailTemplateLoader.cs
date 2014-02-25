using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;


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
       

        public MailTemplate Load(string templateName, CultureInfo culture) {
            var rn = String.Format("{0}.{1}.{2}", this.Namespace, templateName, this.Extension);
            var content = this.GetResourceContent(rn);

            return new MailTemplate {
                Key = rn,
                Content = content,
                Encoding = Encoding.UTF8,
                LastModified = this.dateStarted
            };
        }


        private string GetResourceContent(string rn) {
            using (var r = this.assembly.GetManifestResourceStream(rn)) {
                using (var sr = new StreamReader(r)) {
                    return sr.ReadToEnd();
                }
            }
        }
    }
}
