using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;


namespace Acr.Nh.Components {
    
    public class HashPasswordComponent {

        public virtual DateTime? DateUpdated { get; set; }
        public virtual byte[] RawValue { get; set; }

        private string newPassword;

        public virtual string NewPassword {
            get { return this.newPassword; }
            set {
                this.newPassword = value;
                this.RawValue = ToHash(value);
                this.DateUpdated = DateTime.UtcNow;
            }
        }

        public virtual bool IsEqualTo(string passWordToCheck) {
            var encode = ToHash(passWordToCheck);
            return this.RawValue.SequenceEqual(encode);
        }


        public virtual string GenerateNewRandomPassword(int length = 10) {
            Verify.IsTrue(length <= 32, "Maximum length is 32 characters");
            string s = Guid.NewGuid().ToString().Replace("-", "").Substring(0, length);
            this.NewPassword = s;
            return s;
        }


        private static byte[] ToHash(string s) {
            using (var hash = MD5.Create()) {
                return hash.ComputeHash(Encoding.UTF8.GetBytes(s));
            }
        }
    }
}