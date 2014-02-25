using System;


namespace Acr.Nh.Auditing {

    public enum AuditType {
        Created = 1,
        Updated = 2,
        Deleted = 3
    }
    
    public sealed class EntityAuditContext {

        public object Entity { get; private set; }
        public AuditType AuditType { get; private set; }


        public EntityAuditContext(object entity, AuditType auditType) {
            this.Entity = entity;
            this.AuditType = auditType;
        }
    }
}
