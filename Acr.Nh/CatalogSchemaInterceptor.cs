using System;
using NHibernate;
using NHibernate.SqlCommand;


namespace Acr.Nh {
    
    public class CatalogSchemaInterceptor : EmptyInterceptor {

        /// <summary>
        /// This must match what you put in the catalog mapping or NHibernate session factory mapping
        /// </summary>
        public string CatalogPlaceHolder { get; set; }

        /// <summary>
        /// This must match what you put in the schema mapping or NHibernate session factory mapping
        /// </summary>
        public string SchemaPlaceHolder { get; set; }

        private readonly Action<CatalogSchemaLocator> locatorVisitor;
        private readonly bool interceptCatalog;
        private readonly bool interceptSchema;


        public CatalogSchemaInterceptor(Action<CatalogSchemaLocator> locatorVisitor, bool interceptCatalog, bool interceptSchema) {
            this.interceptCatalog = interceptCatalog;
            this.interceptSchema = interceptSchema;
            this.CatalogPlaceHolder = "[=DPH=]";
            this.SchemaPlaceHolder = "[=SPH=]";
            this.locatorVisitor = locatorVisitor;
        }
 

        public override SqlString OnPrepareStatement(SqlString sql) {
            var info = new CatalogSchemaLocator();
            this.locatorVisitor(info);

            if (this.interceptCatalog) {
                if (info.Catalog.IsEmpty())
                    throw new ArgumentException("Catalog was not supplied");

                sql = sql.Replace(this.CatalogPlaceHolder, info.Catalog);
            }
            if (this.interceptSchema) {
                // TODO: what about querying all schemas?  might screw up nhibernate due to duplicate keys going into 1st level cache
                if (info.Schema.IsEmpty())
                    throw new ArgumentException("Schema was not supplied");

                sql = sql.Replace(this.SchemaPlaceHolder, info.Schema);    
            }
            return base.OnPrepareStatement(sql);
        }
    }
}
