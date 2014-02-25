using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using HibernatingRhinos.Profiler.Appender.NHibernate;


namespace Acr.Nh.Tests {

    public class StartupFixture {
        
        public void OnStartup() {
            NHibernateProfiler.Initialize();

            var cs = ConfigurationManager.ConnectionStrings["acrframework"].ConnectionString;
            using (var connection = new SqlConnection(cs)) {
                connection.Open();
                using (var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted)) {
                    using (var command = connection.CreateCommand()) {
                        command.Transaction = transaction;
                        command.CommandText = File.ReadAllText("Database.sql");
                        command.ExecuteNonQuery();
                        transaction.Commit();
                    }
                }
            }
        }
    }
}
