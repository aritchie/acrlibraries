using System;
using System.Linq;


namespace Acr {

    public static class StringUtil {

        public static string ToFileSize(this long l) {
            return String.Format(new FileSizeFormatProvider(), "{0:fs}", l);
        }

        /// <summary>
        /// Splits a string and trims each item in the array
        /// </summary>
        /// <param name="value"></param>
        /// <param name="splitter"></param>
        /// <returns></returns>
        public static string[] SplitTrim(this string value, char splitter) {
            return value
                .Split(splitter)
                .Select(x => x.Trim()) 
                .Where(x => !x.IsEmpty())
                .ToArray();
        }


        public static bool LengthBetween(this string @string, int min, int max) {
            var l = (@string.IsEmpty() ? 0 : @string.Length);
            return (l >= min && l <= max);
        }


        public static bool IsEmpty(this string check) {
            return String.IsNullOrWhiteSpace(check);
        }
    }
}
