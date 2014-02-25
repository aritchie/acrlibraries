using System;
using System.Threading;
using Acr.Security;
using Xunit;


namespace Acr.Tests.Security {
    
    public class TokenCreatorTests {

        private const string PASSWORD = "onlyimallowed";
        private const string SALT = "pepper";


        [Fact]
        public void StandardNoExpirationTest() {
            var tc = new TokenCreator(PASSWORD, SALT);
            var enc = tc.Create("1", "2", "3");
            var values = tc.Decode(enc, 3);
            Assert.Equal(values[0], "1");
            Assert.Equal(values[1], "2");
            Assert.Equal(values[2], "3");
        }


        [Fact]
        public void ExpiryTest() {
            var tc = new TokenCreator(PASSWORD, SALT, 1);
            var enc = tc.Create("1");
            Thread.Sleep(2000); // wait for token to expire
            Assert.Throws<ArgumentException>(() => { tc.Decode(enc, 1); });
        }
    }
}
