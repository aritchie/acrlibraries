using System;
using System.IO;
using System.Text;


namespace Acr.Mail {
    
    public static class Extensions {

        public static string ToStringContent(this IMailTemplate template) {
            using (var stream = template.GetStream()) 
                using (var sr = new StreamReader(stream))
                    return sr.ReadToEnd();
        }


        public static Encoding DetectEncoding(string path) {
            var bom = new byte[4];
            using (var file = new FileStream(path, FileMode.Open)) 
                file.Read(bom, 0, 4);

            if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76) 
                return Encoding.UTF7;

            if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf) 
                return Encoding.UTF8;

            if (bom[0] == 0xff && bom[1] == 0xfe) 
                return Encoding.Unicode; //UTF-16LE

            if (bom[0] == 0xfe && bom[1] == 0xff) 
                return Encoding.BigEndianUnicode; //UTF-16BE

            if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff) 
                return Encoding.UTF32;

            return Encoding.ASCII;            
        }


        //public static bool IsEmpty(this string @string) {
        //    return String.IsNullOrWhiteSpace(@string);
        //}


        //public static void Each<T>(this IEnumerable<T> en, Action<T> action) {
        //    if (en == null)
        //        return;

        //    var it = en.GetEnumerator();
        //    while (it.MoveNext()) {
        //        action(it.Current);
        //    }
        //}


        //public static bool IsEmpty<T>(this IEnumerable<T> en) {
        //    return (en == null || !en.Any());
        //}


        //public static string[] SplitTrim(this string value, char splitter) {
        //    return value
        //        .Split(splitter)
        //        .Select(x => x.Trim()) 
        //        .Where(x => !x.IsEmpty())
        //        .ToArray();
        //}
    }
}
