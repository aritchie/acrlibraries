using System;
using System.Configuration;


namespace Acr.Mail.Tests {
    
    public static class Config {

        public static string TemplateDirectory { get; private set; }
        public static string TemplateNamespace { get; private set; }
        public static string TemplateExtension { get; private set; }
        public static string DebugEmailAddress { get; private set; }

        static Config() {
            TemplateExtension = "xml";
            TemplateDirectory = AppDomain.CurrentDomain.BaseDirectory;
            TemplateNamespace = "Acr.Mail.Tests";
            DebugEmailAddress = "allan.ritchie@gmail.com";
        }
    }
}
