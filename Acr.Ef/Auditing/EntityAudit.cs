using System;
using System.Collections.Generic;


namespace Acr.Ef.Auditing {

    public enum EntityAuditAction {
        Insert = 1,
        Update = 2,
        Delete = 3
    }

    
    public class EntityAudit {

        public virtual long ID { get; set; }
        public virtual string EntityID { get; set; }
        public virtual string EntityType { get; set; }
        public virtual EntityAuditAction Action { get; set; }
        public virtual DateTime DateCreatedUtc { get; set; }

        public virtual string IdentityType { get; set; }
        public virtual string UserName { get; set; }
        public virtual string AppDomain { get; set; }

        public virtual IList<EntityAuditProperty> Properties { get; set; }
    }
}
