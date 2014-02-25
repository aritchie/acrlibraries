using System;


namespace Acr.Ef.Tests.Models {
    
    public class MyFile {

        public virtual int ID { get; set; }
        public virtual string Name { get; set; }
        public virtual byte[] Content { get; set; }
    }
}
