using System;
using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;


namespace Acr.Ef {
    
    public class SchemaDbCommandInterceptor : DbCommandInterceptor {
        
        /// <summary>
        /// This must match what you put in the schema mapping
        /// </summary>
        public string SchemaPlaceHolder { get; set; }

        private readonly Func<string> locatorVisitor;


        public SchemaDbCommandInterceptor(Func<string> locatorVisitor) {
            this.SchemaPlaceHolder = "[=SPH=]";
            this.locatorVisitor = locatorVisitor;
        }


        public override void NonQueryExecuting(DbCommand command, DbCommandInterceptionContext<int> interceptionContext) {
            this.TransformCommand(command);
            base.NonQueryExecuting(command, interceptionContext);
        }


        public override void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext) {
            this.TransformCommand(command);
            base.ReaderExecuting(command, interceptionContext);
        }


        public override void ScalarExecuting(DbCommand command, DbCommandInterceptionContext<object> interceptionContext) {
            this.TransformCommand(command);
            base.ScalarExecuting(command, interceptionContext);
        }


        private void TransformCommand(DbCommand command) {
            var schema = this.locatorVisitor();

            // TODO: if schema is null and default schema is set, this could be an issue?
            if (schema != null) {
                command.CommandText = command.CommandText.Replace(this.SchemaPlaceHolder, schema);
            }
        }
    }
}
