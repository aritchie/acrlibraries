using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Acr.Security.Encryption;


namespace Acr.Security {
    
    /// <summary>
    /// A general purpose token creator for use with security message passing
    /// </summary>
    public class TokenCreator {

        public char SplitToken { get; set; }
        public int ExpirationSeconds { get; private set; }

        private readonly string password;
        private readonly string salt;

        #region ctor

        public TokenCreator(string hashPassword, string salt, int expirationSeconds = 0) {
            this.password = hashPassword;
            this.salt = salt;
            this.SplitToken = '|';
            this.ExpirationSeconds = expirationSeconds;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public string Create(params string[] values) {
            if (values == null || values.Length == 0)
                throw new ArgumentNullException("values");

            var list = new List<string>(values);
            if (this.ExpirationSeconds > 0)
                list.Insert(0, DateTime.UtcNow.AddSeconds(this.ExpirationSeconds).ToString());

            var r = String.Join(this.SplitToken.ToString(), list.ToArray());
            return CipherUtil.Encrypt<AesManaged>(r, this.password, this.salt);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="expectedLength"></param>
        /// <returns></returns>
        public string[] Decode(string token, int expectedLength) {
            if (token.IsEmpty())
                throw new ArgumentNullException("token");

            var d = CipherUtil.Decrypt<AesManaged>(token, this.password, this.salt);
            if (d.IsEmpty())
                throw new ArgumentException("Token is empty");

            var r = d.Split(this.SplitToken);
            if (this.ExpirationSeconds > 0) 
                expectedLength++;

            if (expectedLength > 0 && (r == null || r.Length != expectedLength))
                throw new ArgumentException(String.Format("Invalid Token Length.  Expected: {0} - Received: {1}", expectedLength, (r == null ? "Nothing" : r.Length.ToString())));

            return this.CheckExpiration(r);
        }


        public bool TryDecode(string token, out string[] values, int expectedLength = 0) {
            values = null;
            try {
                values = this.Decode(token, expectedLength);
                return true;
            }
            catch {
                return false;
            }
        }

        #endregion

        #region Internals

        private string[] CheckExpiration(string[] tokens) {
            var r = tokens;
            if (this.ExpirationSeconds > 0) {
                DateTime expiryTime;
                if (!DateTime.TryParse(tokens[0], out expiryTime)) 
                    throw new ArgumentException("Invalid Expiration Value in Token");
                            
                if (expiryTime < DateTime.UtcNow)
                    throw new ArgumentException("Token has expired");

                var list = new List<string>(tokens);
                list.RemoveAt(0);
                r = list.ToArray();
            }
            return r;
        }

        #endregion
    }
}
