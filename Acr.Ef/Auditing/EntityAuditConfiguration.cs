using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Acr.Ef.Auditing.Attributes;


namespace Acr.Ef.Auditing {
    
    public class EntityAuditConfiguration {
        private readonly IDictionary<Type, EntityAuditPropertyConfiguration> auditConfigurations;

        public bool TrackInserts { get; set; }
        public bool TrackDeletes { get; set; }
        public bool SaveOldData { get; set; }
        public bool SaveValuesOnDelete { get; set; }
        //public string Schema { get; set; }
        //public string AuditTable { get; set; }
        //public string AuditPropertyTable { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="implicitAddAllProperties">If marked as true, all properties are added by additional config is searched for with AuditedAttribute.  To ignore auditing in this mode, mark a property as NotAudited</param>
        public void ConfigureByAttributes(Assembly assembly, bool implicitAddAllProperties) {
            assembly
                .GetTypes()
                .Where(x =>
                    x.IsClass &&
                    !x.IsAbstract &&
                    x.IsPublic &&
                    x.GetCustomAttributes(typeof(EntityAuditAttribute)).Any()
                )
                .ToList()
                .ForEach(x => {
                    var attributes = x.GetTypeInfo().DeclaredProperties;
                    //.GetCustomAttributes(typeof(AuditedAttribute));
                    //.GetCustomAttributes(typeof(NotAuditedAttribute));
                });
        }

        public void Audit<T>(Action<T> modelAudit = null) where T : class {
            
        }
    }
}
