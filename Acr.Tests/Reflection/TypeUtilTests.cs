using System;
using Acr.Reflection;
using Xunit;


namespace Acr.Tests.Reflection {
    
    public class TypeUtilTests {

        [Fact]
        public void IsImplementationOf() {
            var obj = new TestObject();
            Assert.True(obj.GetType().IsImplementationOf<ITestObject>());
        }
    }
}
