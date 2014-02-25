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


        public MailTemplate Load(string templateName, CultureInfo culture) {
            var fn = String.Format("{0}-{1}.{2}", templateName, culture, this.FileExtension);
            var path = Path.Combine(this.TemplateDirectory, fn);
            if (!File.Exists(path))
                throw new ArgumentException("Path not exit - " + path);

            // TODO: what about encoding?
            return new MailTemplate {
                Key = path,
                Content = File.ReadAllText(path),
                LastModified = File.GetLastWriteTimeUtc(path)
            };
        }
    }
}

/*
 public static Encoding DetectEncoding(byte[] fileContent)
    {
        if (fileContent == null)
            throw new ArgumentNullException();

        if (fileContent.Length < 2)
            return Encoding.ASCII;      // Default fallback

        if (fileContent[0] == 0xff
            && fileContent[1] == 0xfe
            && (fileContent.Length < 4
                || fileContent[2] != 0
                || fileContent[3] != 0
                )
            )
            return Encoding.Unicode;

        if (fileContent[0] == 0xfe
            && fileContent[1] == 0xff
            )
            return Encoding.BigEndianUnicode;

        if (fileContent.Length < 3)
            return null;

        if (fileContent[0] == 0xef && fileContent[1] == 0xbb && fileContent[2] == 0xbf)
            return Encoding.UTF8;

        if (fileContent[0] == 0x2b && fileContent[1] == 0x2f && fileContent[2] == 0x76)
            return Encoding.UTF7;

        if (fileContent.Length < 4)
            return null;

        if (fileContent[0] == 0xff && fileContent[1] == 0xfe && fileContent[2] == 0 && fileContent[3] == 0)
            return Encoding.UTF32;

        if (fileContent[0] == 0 && fileContent[1] == 0 && fileContent[2] == 0xfe && fileContent[3] == 0xff)
            return Encoding.GetEncoding(12001);

        String probe;
        int len = fileContent.Length;

        if( fileContent.Length >= 128 ) len = 128;
        probe = Encoding.ASCII.GetString(fileContent, 0, len);

        MatchCollection mc = Regex.Matches(probe, "^<\\?xml[^<>]*encoding[ \\t\\n\\r]?=[\\t\\n\\r]?['\"]([A-Za-z]([A-Za-z0-9._]|-)*)", RegexOptions.Singleline);
        // Add '[0].Groups[1].Value' to the end to test regex

        if( mc.Count == 1 && mc[0].Groups.Count >= 2 )
        {
            // Typically picks up 'UTF-8' string
            Encoding enc = null;

            try {
                enc = Encoding.GetEncoding( mc[0].Groups[1].Value );
            }catch (Exception ) { }

            if( enc != null )
                return enc;
        }

        return Encoding.ASCII;      // Default fallback
    }*/