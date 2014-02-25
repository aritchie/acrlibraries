using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;


namespace Acr.Data {

    public static class DataReaderExtensions {

        public static dynamic GetDynamic(this IDataReader reader) {
            dynamic data = new ExpandoObject();
            var dict = (Dictionary<string, object>)data;

            for (var i = 0; i < reader.FieldCount; i++) {
                dict.Add(reader.GetName(i), reader[i]);
            }
            return data;
        }


        public static dynamic GetDynamicList(this IDataReader reader) {
            var list = new List<dynamic>();
            while (reader.Read()) {
                list.Add(reader.GetDynamic());
            }
            return list;
        }


        public static int GetInt(this IDataReader reader, string fieldName) {
            object o = reader.GetValue(fieldName);
            return (o == null ? 0 : Convert.ToInt32(o));
        }


        public static long GetLong(this IDataReader reader, string fieldName) {
            object o = reader.GetValue(fieldName);
            return (o == null ? 0 : Convert.ToInt64(o));
        }


        public static decimal GetDecimal(this IDataReader reader, string fieldName) {
            object o = reader.GetValue(fieldName);
            return (o == null ? 0 : Convert.ToDecimal(o));
        }


        public static string GetString(this IDataReader reader, string fieldName) {
            return reader.GetString(fieldName, null);
        }


        public static T CastEnum<T>(this IDataReader reader, string key) {
            return (T)Enum.ToObject(typeof(T), reader.GetValue(key));
        }


        public static DateTime? GetDateTime(this IDataReader reader, string fieldName) {
            object value = reader.GetValue(fieldName);
            DateTime? dateTime = null;
            if (value != null)
                dateTime = (DateTime)value;
            return dateTime;
        }


        public static DateTime GetDateTime(this IDataReader reader, string fieldName, DateTime defaultValue) {
            object value = reader.GetValue(fieldName);
            return (value == null ? defaultValue : (DateTime)value);
        }


        public static string GetString(this IDataReader reader, string fieldName, string defaultValue) {
            object value = reader.GetValue(fieldName);
            return (value == null ? defaultValue : (string)value);
        }


        public static int GetIndex(this IDataReader reader, string fieldName) {
            int index = -1;
            for (int i = 0; i < reader.FieldCount; i++) {
                string fn = reader.GetName(i);
                if (fn == fieldName) {
                    index = i;
                    break;
                }
            }
            return index;
        }


        public static object GetValue(this IDataReader reader, string fieldName) {
            object value = null;
            int index = reader.GetIndex(fieldName);
            if (index > -1 && !reader.IsDBNull(index))
                value = reader.GetValue(index);
            return value;
        }
    }
}
