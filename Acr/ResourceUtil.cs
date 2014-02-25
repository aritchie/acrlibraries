using System;
using System.IO;
using System.Reflection;


namespace Acr {
    
    public static class ResourceUtil {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetEmbeddedResourceAsString(this Assembly assembly, string name) {
            using (var stream = assembly.GetManifestResourceStream(name)) {
                using (var sr = new StreamReader(stream)) {
                    return sr.ReadToEnd();
                }
            }
        }
    }
}
