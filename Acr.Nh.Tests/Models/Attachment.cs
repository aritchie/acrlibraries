using System;


namespace Acr.Nh.Tests.Models {
    
    public class Attachment {

        public virtual int ID { get; set; }
        public virtual string FileName { get; set; }
        public virtual long FileSize { get; set; }
        public virtual byte[] Data { get; set; }
    }
}
