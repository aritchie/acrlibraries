using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;


namespace Acr.Ef {
    
    public class EntityTimestampModule : DbContextModule {

        public override void OnPreInsert(DbContext context, DbEntityEntry entry) {
            SetDateTime(entry, "DateCreated");
            SetDateTime(entry, "DateUpdated");
            base.OnPreInsert(context, entry);
        }


        public override void OnPreUpdate(DbContext context, DbEntityEntry entry) {
            SetDateTime(entry, "DateUpdated");
            base.OnPreUpdate(context, entry);
        }


        private static void SetDateTime(DbEntityEntry entry, string propertyName) {
            var property = entry.TryGetProperty(propertyName);
            if (property != null) {
                if (property.CurrentValue is DateTime) {
                    var r = (DateTime)property.CurrentValue;
                    if (r == DateTime.MinValue) {
                        property.CurrentValue = DateTime.UtcNow;
                    }                  
                }
                else if (property.CurrentValue is DateTimeOffset) {
                    var r = (DateTimeOffset)property.CurrentValue;
                    if (r == DateTimeOffset.MinValue) {
                        property.CurrentValue = DateTimeOffset.UtcNow;
                    }
                }
            }
        }
    }
}
