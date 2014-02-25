using System;
using System.Configuration;
using System.Reflection;
using Acr.Nh.Mapping;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;
using Configuration = NHibernate.Cfg.Configuration;


namespace Acr.Nh.Tests {
    
    public static class Config {

        public static Configuration GetDefaultNHConfiguration(bool autoMapAll = false) {
            var cfg = new Configuration();

            cfg
                .CurrentSessionContext<ThreadStaticSessionContext>()
                .DataBaseIntegration(x => {
                    x.FullDriver(ConfigurationManager.AppSettings["nhibernate.dbdriver"] ?? "mssql2012");
                    x.ConnectionStringName = "acrframework";
                });

            if (autoMapAll) {
                cfg.AddAssemblyByCodeMap<AcrModelMapper>(Assembly.GetExecutingAssembly());
            }
            return cfg;
        }


        public static ISessionFactory BuildDefaultSessionFactory() {
            return GetDefaultNHConfiguration(true).BuildSessionFactory();
        }
    }
}
