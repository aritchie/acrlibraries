using System;
using System.IO;
using System.Text;
using Commons.Collections;
using NVelocity.Runtime.Resource;
using NVelocity.Runtime.Resource.Loader;


namespace Acr.Mail.NVelocityParser {

    public class NVelocityDirectResourceLoader : ResourceLoader {

     
        public override long GetLastModified(Resource resource) {
            return 0;
        }


        public override Stream GetResourceStream(string source) {
            var encSplit = source.IndexOf(Environment.NewLine);            
            var encString = source.Substring(0, encSplit);
            var enc = Encoding.GetEncoding(encString);
            var body = source.Substring(encSplit);
            return new MemoryStream(enc.GetBytes(body));
        }


        public override void Init(ExtendedProperties configuration) {
        }


        public override bool IsSourceModified(Resource resource) {
            return false;
        }
    }
}
