using System;
using System.Xml.Serialization;


namespace Acr.Mail.Serialization {
    
    [Serializable]
    [XmlRoot("mailaddress")]
    public class XmlMailAddress {

        [XmlElement("address")]
        public string Address { get; set; }

        [XmlElement("display")] 
        public string DisplayName { get; set; }
    }
}
