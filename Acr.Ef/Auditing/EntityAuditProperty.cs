using System;


namespace Acr.Ef.Auditing {
    
    public class EntityAuditProperty {

        public virtual long ID { get; set; }
        public virtual string PropertyName { get; set; }
        public virtual string OldValue { get; set; }
        public virtual string NewValue { get; set; }

        public virtual long EntityAuditID { get; set; }
        public virtual EntityAudit EntityAudit { get; set; }
    }
}
