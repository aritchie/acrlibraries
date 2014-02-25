using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using Acr.Mail.Serialization;
using Commons.Collections;
using NVelocity;
using NVelocity.App;
using NVelocity.Runtime;


namespace Acr.Mail.NVelocityParser {

    public class NVelocityTemplateParser : IMailTemplateParser {
        private readonly VelocityEngine engine;


        public NVelocityTemplateParser() {
            var props = new ExtendedProperties();
            props.SetProperty(RuntimeConstants.RESOURCE_LOADER, "direct");
            props.SetProperty("direct.resource.loader.class", "Acr.Mail.NVelocityParser.NVelocityDirectResourceLoader; Acr.Mail.NVelocityParser");

            engine = new VelocityEngine();
            engine.Init(props);            
        }

        #region IMailTemplateParser Members

        public MailMessage Parse(MailTemplate template, IDictionary<string, object> variables) {
            var context = new VelocityContext();
            context.Put("helper", new TemplateHelper());

            if (variables != null) {
                foreach (var key in variables.Keys) {
                    context.Put(key, variables[key]);
                }
            }

            var parse = String.Empty;
            using (var sw = new StringWriter()) {
                // nvelocity thinks it is parsing a file which is why it needs encoding.  just pass UTF-8
                var nvtemp = engine.GetTemplate(template.Content, "UTF-8");
                nvtemp.Merge(context, sw);

                parse = sw.ToString();
            }
            return XmlMailSerializer.Deserialize(parse);
        }

        #endregion
    }
}
