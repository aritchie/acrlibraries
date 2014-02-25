using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Xml.Serialization;


namespace Acr.Mail.Serialization {
    
    public static class XmlMailSerializer {

        public static string Serialize(MailMessage mail) {
            var xml = new XmlMailMessage {
                Subject = mail.Subject,
                Priority = mail.Priority,
                MessageID = mail.GetMessageID(),
                ReplyToMessageID = mail.GetInReplyTo(),
                From = ToXmlMailAddress(mail.From),
                To = mail.To.Select(ToXmlMailAddress).ToList(),
                Cc = mail.CC.Select(ToXmlMailAddress).ToList(),
                Bcc = mail.Bcc.Select(ToXmlMailAddress).ToList(),
                ReplyToList = mail.ReplyToList.Select(ToXmlMailAddress).ToList(),
                Attachments = mail
                    .Attachments
                    // TODO: streams?
                    .Select(x => new XmlMailAttachment {
                        FileName = x.Name,
                        Path = x.ContentDisposition.FileName
                    })
                    .ToList()
            };

            mail.Headers.AllKeys.ToList().ForEach(x => 
                xml.Headers.Add(new XmlMailHeader {
                    Key = x,
                    Value = mail.Headers[x]
                })
            );

            if (mail.IsBodyHtml) {
                xml.HtmlContent = mail.Body;
                var plainText = mail.AlternateViews.FirstOrDefault();
                if (plainText != null) {
                    using (var sr = new StreamReader(plainText.ContentStream)) {
                        xml.PlainTextContent = sr.ReadToEnd();
                    }
                }
            }
            else {
                xml.PlainTextContent = mail.Body;
                var htmlText = mail.AlternateViews.FirstOrDefault();
                if (htmlText != null) {
                    using (var sr = new StreamReader(htmlText.ContentStream)) {
                        xml.HtmlContent = sr.ReadToEnd();
                    }
                }
            }
            var serializer = new XmlSerializer(typeof(XmlMailMessage));
            using (var sw = new StringWriter()) {
                serializer.Serialize(sw, xml);
                return sw.ToString();
            }
        }


        public static MailMessage Deserialize(string content) {
            var serializer = new XmlSerializer(typeof(XmlMailMessage));
            XmlMailMessage xml;

            using (var sr = new StringReader(content)) {
                 xml = (XmlMailMessage)serializer.Deserialize(sr);
            }
            return ToMailMessage(xml);
        }


        #region Internals

        private static XmlMailAddress ToXmlMailAddress(MailAddress address) {
            return new XmlMailAddress {
                Address = address.Address,
                DisplayName = address.DisplayName
            };
        }


        private static MailAddress ToMailAddress(XmlMailAddress address, string groupName) {
            if (address.Address.IsEmpty()) 
                throw new ArgumentException(String.Format("Address is empty in {0} address list", groupName));

            return (address.DisplayName.IsEmpty()
                ? new MailAddress(address.Address)
                : new MailAddress(address.Address, address.DisplayName, Encoding.UTF8) 
            ); 
        }


        private static MailMessage ToMailMessage(XmlMailMessage xml) {
            var mail = new MailMessage {
                Subject = xml.Subject,
                Priority = xml.Priority,
                IsBodyHtml = (xml.HtmlContent != null),
                Body = xml.HtmlContent ?? xml.PlainTextContent
            };
            if (xml.HtmlContent != null && xml.PlainTextContent != null) {
                mail.AlternateViews.Add(
                    AlternateView.CreateAlternateViewFromString(
                        xml.PlainTextContent, 
                        null, 
                        MediaTypeNames.Text.Plain
                    )
                );
            }

            if (xml.From != null) {
                mail.From = ToMailAddress(xml.From, "<from>");
            }
            xml.Headers.ForEach(x => mail.Headers.Add(x.Key, x.Value));
            xml.To.ForEach(x => mail.To.Add(ToMailAddress(x, "<to>")));
            xml.Cc.ForEach(x => mail.CC.Add(ToMailAddress(x, "<cc>")));
            xml.Bcc.ForEach(x => mail.Bcc.Add(ToMailAddress(x, "<bcc>")));
            xml.ReplyToList.ForEach(x => mail.ReplyToList.Add(ToMailAddress(x, "<replyto>")));

            if (!xml.MessageID.IsEmpty()) {
                mail.SetMessageID(xml.MessageID);
            }
            if (!xml.ReplyToMessageID.IsEmpty()) {
                mail.SetInReplyTo(xml.ReplyToMessageID);
            }

            //// TODO: XML always in UTF-8
            ////mail.HeadersEncoding = Encoding.UTF8;
            ////mail.BodyEncoding = Encoding.UTF8;
            ////mail.SubjectEncoding = Encoding.UTF8;
            ////mail.Sender = null; // send on behalf?
            return mail;
        }

        #endregion
    }
}
/*

    MailMessage m = new MailMessage(sender, recipient);
    m.Subject = "Test Message";

    // Define the plain text alternate view and add to message
    string plainTextBody =
        "You must use an email client that supports HTML messages";

    AlternateView plainTextView =
        AlternateView.CreateAlternateViewFromString(
            plainTextBody, null, MediaTypeNames.Text.Plain);

    m.AlternateViews.Add(plainTextView);

    // Define the html alternate view with embedded image and
    // add to message. To reference images attached as linked
    // resources from your HTML message body, use "cid:contentID"
    // in the <img> tag...
    string htmlBody =
        "<html><body><h1>Picture</h1><br>" +
        "<img src=\"cid:SampleImage\"></body></html>";

    AlternateView htmlView =
        AlternateView.CreateAlternateViewFromString(
            htmlBody, null, MediaTypeNames.Text.Html);

    // ...and then define the actual LinkedResource matching the
    // ContentID property as found in the image tag. In this case,
    // the HTML message includes the tag
    // <img src=\"cid:SampleImage\"> and the following
    // LinkedResource.ContentId is set to "SampleImage"
    LinkedResource sampleImage =
        new LinkedResource("sample.jpg",
            MediaTypeNames.Image.Jpeg);
    sampleImage.ContentId = "SampleImage";

    htmlView.LinkedResources.Add(sampleImage);*/