using System;


namespace Acr.Nh.Auditing {
    
    public interface IEntityAuditEventListener {

        // allows you to update markers directly on main object (ie. DateCreated & DateUpdated, perhaps even the user)
        // TODO: set lastaudit
        void OnBeforeDatabaseAction(EntityAuditContext context);
        void OnAudit(EntityAuditContext context);
    }
}
