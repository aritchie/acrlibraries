using System;
using System.Security.Cryptography;
using Acr.Security.Encryption;
using Xunit;


namespace Acr.Tests.Security {
    
    public class CipherUtilTests {

        private const string TEST_VALUE = "this is my test";
        private const string ENCRYPT_VALUE = "SN2oTWWfqMch+totemeuCav5qGIarKd6FaIJgh4DSg+hljtDrP62codlhqRmuWiO";
        private const string PASSWORD = "onlyimallowed";
        private const string SALT = "pepper";


        [Fact]
        public void Encrypt() {
            var value = CipherUtil.Encrypt<AesManaged>(TEST_VALUE, PASSWORD, SALT);
            Assert.Equal(value, ENCRYPT_VALUE);
        }


        [Fact]
        public void Decrypt() {
            var value = CipherUtil.Decrypt<AesManaged>(ENCRYPT_VALUE, PASSWORD, SALT);
            Assert.Equal(value, TEST_VALUE);
        }

    }
}