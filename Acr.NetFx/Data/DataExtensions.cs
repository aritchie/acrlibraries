using System;
using System.Data;
using System.Threading.Tasks;


namespace Acr.Data {

    public static class DataExtensions {

        public static bool OpenIf(this IDbConnection connection) {
            if (connection.State != ConnectionState.Open) {
                connection.Open();
                return true;
            }
            return false;
        }


        public static IDbCommand Command(this IDbConnection connection, string sql) {
            var cmd = connection.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = (sql.Contains(" ") ? CommandType.Text : CommandType.StoredProcedure);
            return cmd;
        }


        public static IDbCommand SetParameter(this IDbCommand command, string name, object value = null) {
            var param = command.CreateParameter();

            param.ParameterName = name;
            param.Value = value;

            command.Parameters.Add(param);
            return command;
        }


        public static IDbCommand SetParameter(this IDbCommand command, string name, object value, DbType dbType) {
            var param = command.CreateParameter();

            param.ParameterName = name;
            param.Value = value;
            param.DbType = dbType;

            command.Parameters.Add(param);
            return command;
        }


        public static IDataReader GetDataReader(this IDbCommand command) {
            bool close = command.Connection.OpenIf();
            return command.ExecuteReader(close ? CommandBehavior.CloseConnection : CommandBehavior.Default);
        }


        public static int Execute(this IDbCommand command) {
            int count = 0;
            bool close = true;
            try {
                close = command.Connection.OpenIf();
                count = command.ExecuteNonQuery();
            }
            finally {
                if (close) {
                    command.Connection.Close();
                }
            }
            return count;
        }


        public static Task<int> ExecuteAsync(this IDbCommand command) {
            return Task.Factory.StartNew(() => command.Execute());
        }


        public static long Count(this IDbCommand command) {
            var obj = command.Scalar();
            if (obj == null)
                return 0;

            return Convert.ToInt32(obj);
        }


        public static Task<long> CountAsync(this IDbCommand command) {
            return Task<long>.Factory.StartNew(command.Count);
        }


        public static object Scalar(this IDbCommand command) {
            object scalar = null;
            bool close = true;
            try {
                close = command.Connection.OpenIf();
                scalar = command.ExecuteScalar();
            }
            finally {
                if (close) {
                    command.Connection.Close();
                }
            }
            if (scalar != null && scalar == DBNull.Value) {
                scalar = null;
            }
            return scalar;
        }


        public static Task<object> ScalarAsync(this IDbCommand command) {
            return Task<object>.Factory.StartNew(command.Scalar);
        }


        public static DataTable GetDataTable<T>(this IDbCommand command) where T : class, IDbDataAdapter, new() {
            var adapter = new T();
            var ds = new DataSet();

            adapter.SelectCommand = command;
            adapter.Fill(ds);

            return (ds.Tables.Count == 0 ? new DataTable() : ds.Tables[0]);
        }


        public static Task<DataTable> GetDataTableAsync<T>(this IDbCommand command) where T : class, IDbDataAdapter, new() {
            return Task<DataTable>.Factory.StartNew(command.GetDataTable<T>);
        }
    }
}
