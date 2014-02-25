using System;
using System.Collections.Generic;


namespace Acr.Ef.Tests.Models {
    
    public class Family {

        public virtual int ID { get; set; }
        public virtual string Name { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual ICollection<Person> Members { get; set; }
    }
}
