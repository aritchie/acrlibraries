using System;
using System.Xml.Serialization;


namespace Acr.Mail.Serialization {
    
    [Serializable]
    [XmlRoot("attachment")]
    public class XmlMailAttachment {

        [XmlElement("key")]
        public string FileName { get; set; }

        [XmlElement("path")]
        public string Path { get; set; }
    }
}
