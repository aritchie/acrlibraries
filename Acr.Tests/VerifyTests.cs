using System;
using Xunit;


namespace Acr.Tests {

    public class VerifyTests {

        [Fact]
        public void Fail_IsNotNull_With_Set_Exception() {
            Assert.Throws<ApplicationException>(() => { Verify.IsNotNull<ApplicationException>(null, "Blah"); });
        }
    }
}
