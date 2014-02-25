using System;


namespace Acr.Ef.Auditing.Attributes {
    
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class EntityAuditAttribute : Attribute {}
}
