using System;
using System.Globalization;
using System.IO;
using System.Text;


namespace Acr.Mail {
    
    public interface IMailTemplate {

        string TemplateName { get; }
        string Location { get; }
        Encoding Encoding { get; }
        CultureInfo Culture { get; }
        DateTime LastModified { get; }

        Stream GetStream();
    }
}
