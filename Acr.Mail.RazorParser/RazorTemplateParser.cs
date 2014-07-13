using System;
using System.Net.Mail;
using Acr.Mail.Serialization;
using RazorEngine;
using RazorEngine.Templating;


namespace Acr.Mail.RazorParser {
    
    public class RazorTemplateParser : IMailTemplateParser {

        public MailMessage Parse(IMailTemplate template, object model) {
            var viewBag = new DynamicViewBag();
            viewBag.AddValue("Helper", new TemplateHelper());

            var content = template.ToStringContent();
            var result = Razor.Parse(content, model, viewBag, template.Location);

            return XmlMailSerializer.Deserialize(result);
        }
    }
}
