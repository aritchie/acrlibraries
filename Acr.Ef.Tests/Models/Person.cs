using System;


namespace Acr.Ef.Tests.Models {
    
    public class Person {

        public virtual int ID { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }

        public virtual int FamilyID { get; set; }
        public virtual Family Family { get; set; }

        public virtual int Version { get; set; }
        public virtual DateTimeOffset DateUpdated { get; set; }
        public virtual DateTimeOffset DateCreated { get; set; }
    }
}
