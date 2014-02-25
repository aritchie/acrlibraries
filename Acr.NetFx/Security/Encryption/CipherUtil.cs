using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;


namespace Acr.Security.Encryption {

    public static class CipherUtil {
        private static readonly Encoding Code = Encoding.Unicode;

        public static string Encrypt<T>(string value, string password, string salt) where T : SymmetricAlgorithm, new() {
            using (var transformer = CreateTransformer<T>(password, salt, true)) {
                using (var buffer = new MemoryStream()) {
                    using (var stream = new CryptoStream(buffer, transformer, CryptoStreamMode.Write)) {
                        using (var writer = new StreamWriter(stream, Code)) {
                            writer.Write(value);
                        }
                    }
                    return Convert.ToBase64String(buffer.ToArray());
                }
            }
        }


        public static string Decrypt<T>(string encryptedValue, string password, string salt) where T : SymmetricAlgorithm, new() {
            var encBytes = Convert.FromBase64String(encryptedValue);

            using (var transformer = CreateTransformer<T>(password, salt, false)) {
                using (var buffer = new MemoryStream(encBytes)) {
                    using (var stream = new CryptoStream(buffer, transformer, CryptoStreamMode.Read)) {
                        using (var reader = new StreamReader(stream, Code)) {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
        }


        private static ICryptoTransform CreateTransformer<T>(string password, string salt, bool encrypting) where T : SymmetricAlgorithm, new() {
            var algorithm = new T();
            var rgb = new Rfc2898DeriveBytes(password, Code.GetBytes(salt));
            var rgbKey = rgb.GetBytes(algorithm.KeySize >> 3);
            var rgbIV = rgb.GetBytes(algorithm.BlockSize >> 3);

            return (encrypting 
                ? algorithm.CreateEncryptor(rgbKey, rgbIV)
                : algorithm.CreateDecryptor(rgbKey, rgbIV)
            );
        }
    }
}