using System;
using System.Collections.Generic;


namespace Acr.Ef.Auditing {

    public enum EntityAuditType {
        Insert = 1,
        Update = 2,
        Delete = 3
    }

    
    public class EntityAudit {

        public virtual long ID { get; set; }
        public virtual string EntityID { get; set; }
        public virtual string EntityType { get; set; }
        public virtual EntityAuditType Type { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual IList<EntityAuditProperty> Properties { get; set; }
    }
}
