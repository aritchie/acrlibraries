using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Mail;
using System.Xml.Serialization;


namespace Acr.Mail.Serialization {
    
    [Serializable]
    [XmlRoot("mail")]
    public class XmlMailMessage {

        [XmlElement("from")]
        public XmlMailAddress From { get; set; }

        [XmlElement("priority")]
        [DefaultValue(MailPriority.Normal)]
        public MailPriority Priority { get; set; }

        [XmlElement("subject")] 
        public string Subject { get; set; }

        [XmlArray("to")]
        [XmlArrayItem("mailaddress")]
        public List<XmlMailAddress> To { get; set; }

        [XmlArray("cc")]
        [XmlArrayItem("mailaddress")]
        public List<XmlMailAddress> Cc { get; set; }

        [XmlArray("bcc")]
        [XmlArrayItem("mailaddress")]
        public List<XmlMailAddress> Bcc { get; set; }

        [XmlArray("reply-to")]
        [XmlArrayItem("mailaddress")]
        public List<XmlMailAddress> ReplyToList { get; set; }
        
        [XmlArray("headers")]
        [XmlArrayItem("header")]
        public List<XmlMailHeader> Headers {get; set; }
        
        [XmlArray("attachments")]
        [XmlArrayItem("attachment")]
        public List<XmlMailAttachment> Attachments { get; set; }
        
        [XmlElement("message-id")] 
        public string MessageID { get; set; }

        [XmlElement("reply-to-message-id")]
        public string ReplyToMessageID { get; set; }

        [XmlElement("html-content")]
        public string HtmlContent { get; set; }

        [XmlElement("text-content")]
        public string PlainTextContent { get; set; }

        [XmlElement("delivery-notification")]
        public DeliveryNotificationOptions DeliveryNotification { get; set; }

        public XmlMailMessage() {
            this.Priority = MailPriority.Normal;
            this.To = new List<XmlMailAddress>(5);
            this.Cc = new List<XmlMailAddress>(5);
            this.Bcc = new List<XmlMailAddress>(5);
            this.ReplyToList = new List<XmlMailAddress>(5);
            this.Headers = new List<XmlMailHeader>(5);
        }
    }
}
