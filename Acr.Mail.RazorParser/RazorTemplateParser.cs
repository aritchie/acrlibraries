using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Mail;
using Acr.Mail.Serialization;
using RazorEngine;
using RazorEngine.Templating;


namespace Acr.Mail.RazorParser {
    
    public class RazorTemplateParser : IMailTemplateParser {

        public MailMessage Parse(MailTemplate template, IDictionary<string, object> args) {
            var obj = new ExpandoObject() as IDictionary<string, object>;
            args.ToList().ForEach(obj.Add);

            var viewBag = new DynamicViewBag();
            viewBag.AddValue("Helper", new TemplateHelper());
            var result = Razor.Parse(template.Content, obj, viewBag, template.Key);

            return XmlMailSerializer.Deserialize(result);
        }
    }
}
