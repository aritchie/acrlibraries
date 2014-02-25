using System;
using System.Data;
using System.Linq;

namespace Acr.Data {

    public static class DataRowExtensions {

        public static int GetInt(this DataRow row, string key) {
            return row.GetInt(key, 0);
        }


        public static int GetInt(this DataRow row, string key, int defaultValue) {
            return (row.IsNull(key) ? defaultValue : (int)row[key]);
        }


        public static string GetString(this DataRow row, string key) {
            return row.GetString(key, "");
        }


        public static bool HasColumnNamed(this DataRow row, string columnName) {
            return row.Table.Columns
                .Cast<DataColumn>()
                .Any(x => x.ColumnName == columnName);
        }


        public static T CastEnum<T>(this DataRow row, string key) {
            return (T)Enum.ToObject(typeof(T), row[key]);
        }


        public static string GetString(this DataRow row, string key, string defaultValue) {
            return (row.IsNull(key) ? defaultValue : row[key].ToString().Trim());
        }
    }
}
