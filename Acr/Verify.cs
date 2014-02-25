using System;
using System.Collections.Generic;
using Acr.Collections;


namespace Acr {

    public static class Verify {

        //public static void FileExists(string path, string message = null) {
        //    if (message == null)
        //        message = String.Format("File '{0}' does not exist", path);

        //    Verify.IsTrue(File.Exists(path), message);   
        //}


        //public static void FileExists(string path1, string path2, string message = null) {
        //    string path = Path.Combine(path1, path2);
        //    Verify.FileExists(path, message);   
        //}


        public static void IsTrue(bool expression, string message = "Expected true, but got false") {
            Verify.IsTrue<ArgumentException>(expression, message);
        }


        public static void IsTrue<T>(bool expression, string message = "Expected true, but got false") where T : Exception {
            if (!expression)
                throw (T)Activator.CreateInstance(typeof(T), new[] { message });
        }


        public static void IsFalse(bool expression, string message = "Expected false, but got true") {
            Verify.IsFalse<ArgumentException>(expression, message);
        }


        public static void IsFalse<T>(bool expression, string message = "Expected false, but got true") where T : Exception {
            if (expression)
                throw (T)Activator.CreateInstance(typeof(T), new[] { message });
        }


        public static void IsNotNull(object obj, string message = "Object is null") {
            Verify.IsNotNull<ArgumentException>(obj, message);
        }


        public static void IsNotNull<T>(object obj, string message = "Object is null") where T : Exception {
            if (obj == null)
                 throw (T)Activator.CreateInstance(typeof(T), new[] { message });
        }


        public static void IsNull(object obj, string message = "Object is not null") {
            Verify.IsNull<ArgumentException>(obj, message);
        }


        public static void IsNull<T>(object obj, string message = "Object is null") where T : Exception {
            if (obj != null)
                throw (T)Activator.CreateInstance(typeof(T), new[] { message });
        }


        public static void IsEmpty(string value, string message = "String is not empty") {
            Verify.IsEmpty<ArgumentException>(value, message);
        }


        public static void IsEmpty<T>(string value, string message = "String is not empty") where T : Exception {
            if (!value.IsEmpty())
                throw (T)Activator.CreateInstance(typeof(T), new[] { message });
        }


        public static void IsNotEmpty(string value, string message = "String is empty") {
            Verify.IsNotEmpty<ArgumentException>(value, message);
        }


        public static void IsNotEmpty<T>(string value, string message = "String is empty") where T : Exception {
            if (value.IsEmpty())
                throw (T)Activator.CreateInstance(typeof(T), new[] { message });
        } 


        public static void IsNotEmpty<T>(IEnumerable<T> en, string message = "Enumerable is empty") {
            if (en.IsEmpty())
                throw new ArgumentException(message);
        }
    }
}
