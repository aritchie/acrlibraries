using System;
using System.Globalization;
using System.IO;


namespace Acr.Mail.Loaders {
    
    public class FileMailTemplateLoader : IMailTemplateLoader {

        public string TemplateDirectory { get; private set; }
        public string FileExtension { get; private set; }


        public FileMailTemplateLoader(string templateDirectory, string extension = "xml") {
            this.TemplateDirectory = templateDirectory;
            this.FileExtension = extension;
        }


        public IMailTemplate Load(string templateName, CultureInfo culture) {
            var fn = String.Format("{0}-{1}.{2}", templateName, culture, this.FileExtension);
            var path = Path.Combine(this.TemplateDirectory, fn);
            if (!File.Exists(path))
                throw new ArgumentException("Path not exit - " + path);

            return new FileMailTemplate(templateName, path, culture);
        }
    }
}