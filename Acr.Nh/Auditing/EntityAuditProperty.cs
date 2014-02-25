using System;


namespace Acr.Ef.Auditing {
    
    public class EntityAuditProperty {

        public virtual long ID { get; set; }
        public virtual EntityAudit Entity { get; set; }
        public virtual string OldValue { get; set; }
        public virtual string NewValue { get; set; }
    }
}
