using System;
using System.Collections.Generic;


namespace Acr.Nh.Tests.Models {
    
    public class User {

        public virtual int ID { get; set; }
        public virtual string UserName { get; set; }
        public virtual byte[] Password { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }

        public virtual ICollection<Todo> Todos { get; set; }
    }
}
