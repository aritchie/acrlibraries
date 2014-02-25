using System;
using System.Linq;
using Acr.Nh.Mapping;
using Acr.Nh.Tests.Maps;
using Acr.Nh.Tests.Models;
using NHibernate;
using NHibernate.Linq;
using Xunit;


namespace Acr.Nh.Tests {
    
    public class DbSchemaInterceptorTests {

        private readonly ISessionFactory sessionFactory;
        private string currentSchema = "my";


        public DbSchemaInterceptorTests() {
            this.sessionFactory = Config
                .GetDefaultNHConfiguration()
                .RegisterDbSchemaInterceptor(x => {
                    x.Schema = this.currentSchema;
                }, false, true)
                .AddCodeMap<AcrModelMapper, AttachmentMap>(new AttachmentMap())
                .BuildSessionFactory();            
        }


        [Fact]
        public void GetAndUpdate() {
            sessionFactory.UnitOfWork(s => {
                var a = s.Query<Attachment>().FirstOrDefault();
                a.FileName = "test." + Guid.NewGuid().ToString().Substring(0, 3);
                s.Update(a);
            });
        }


        [Fact]
        public void Querying() {
            
            sessionFactory.UnitOfWork(s => {
                this.currentSchema = "my";
                var list = s.Query<Attachment>()
                    .Where(x => x.FileName.Contains("a"))
                    .ToList();

                Assert.NotNull(list);

                this.currentSchema = "dbo";
                list = s.Query<Attachment>()
                    .Where(x => x.FileName.Contains("a"))
                    .ToList();

                Assert.NotNull(list);
            });               
        }


        [Fact]
        public void QueryWithJoins() {
            
        }
    }
}
