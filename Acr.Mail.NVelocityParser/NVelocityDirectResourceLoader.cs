using System;
using System.IO;
using System.Text;
using Commons.Collections;
using NVelocity.Runtime.Resource;
using NVelocity.Runtime.Resource.Loader;


namespace Acr.Mail.NVelocityParser {

    public class NVelocityDirectResourceLoader : ResourceLoader {

        #region ResourceLoader Implementation

        public override long GetLastModified(Resource resource) {
            return 0;
        }


        public override Stream GetResourceStream(string source) {
            return new MemoryStream(Encoding.UTF8.GetBytes(source));
        }


        public override void Init(ExtendedProperties configuration) {
        }


        public override bool IsSourceModified(Resource resource) {
            return false;
        }

        #endregion
    }
}
