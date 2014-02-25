using System;


namespace Acr.Nh.Tests.Models {

    public class Todo {

        public virtual int ID { get; set; }
        public virtual string Details { get; set; }
        public virtual DateTimeOffset DateCreated { get; set; }
        public virtual DateTimeOffset? DateCompleted { get; set; }
        public virtual User User { get; set; }
    }
}
