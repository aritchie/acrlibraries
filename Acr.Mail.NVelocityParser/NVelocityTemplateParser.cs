using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public MailMessage Parse(IMailTemplate template, object model) {
            var context = new VelocityContext();
            var args = GetArgs(model);
            if (!args.ContainsKey("helper"))
                context.Put("helper", new TemplateHelper());

            foreach (var arg in args)
                context.Put(arg.Key, arg.Value);


            var result = "";
            using (var sw = new StringWriter()) {
                var content = template.ToStringContent();
                NVelocityDirectResourceLoader.CurrentEncoding = template.Encoding;
                var tmp = engine.GetTemplate(content, template.Encoding.EncodingName);
                tmp.Merge(context, sw);

                result = sw.ToString();
            }
            return XmlMailSerializer.Deserialize(result);
        }


        private IDictionary<string, object> GetArgs(object obj) {
            var dict = obj as IDictionary<string, object>;
            if (dict == null) {
                dict = new Dictionary<string, object>();

                foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(obj)) {
                    var value = property.GetValue(obj);
                    dict.Add(property.Name, value);
                }
            }
            return dict;
        }

        #endregion
    }
}
