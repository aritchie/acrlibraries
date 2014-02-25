using System;


namespace Acr.Ef.Auditing.Attributes {
    
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class NotAuditedAttribute : Attribute {}
}
