using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;


namespace Acr.Ef {

    public class EntityVersionModule : DbContextModule {

        public override void OnPreInsert(DbContext context, DbEntityEntry entry) {
            TrySetVersion(entry, x => 1);
            base.OnPreInsert(context, entry);
        }


        public override void OnPreUpdate(DbContext context, DbEntityEntry entry) {
            TrySetVersion(entry, x => x + 1);
            base.OnPreUpdate(context, entry);
        }


        private static void TrySetVersion(DbEntityEntry entry, Func<int, int> versionSet) {
            var v = entry.TryGetProperty("Version");
            if (v != null && v.CurrentValue is int) {
                v.CurrentValue = versionSet((int)v.CurrentValue);
            }
        }
    }
}
