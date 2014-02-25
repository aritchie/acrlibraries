using System;
using System.Xml.Serialization;


namespace Acr.Mail.Serialization {
    
    [Serializable]
    [XmlRoot("mailaddress")]
    public class XmlMailHeader {

        [XmlElement("key")]
        public string Key { get; set; }

        [XmlElement("value")] 
        public string Value { get; set; }
    }
}
