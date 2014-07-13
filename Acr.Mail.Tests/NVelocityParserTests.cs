using System;
using System.Collections.Generic;
using System.IO;
using Acr.Mail.NVelocityParser;
using NUnit.Framework;


namespace Acr.Mail.Tests {
    
    [TestFixture]
    public class NVelocityParserTests {
        
        [Test]
        public void Test() {
            var parser = new NVelocityTemplateParser();
            //var mail = parser.Parse(
            //    new MailTemplate {
            //        Content = File.ReadAllText("nvelocity.xml")
            //    }, 
            //    new Dictionary<string, object> {
            //        { "model", new { Subject = "Test", Test = String.Empty }} 
            //    }
            //);
            //Assert.False(mail.IsBodyHtml);
            //Assert.AreEqual("Test", mail.Subject);
            //Assert.True(mail.Body.Contains("POOF"));
            //Assert.True(mail.Body.Contains("WORKS"));
        }
    }
}