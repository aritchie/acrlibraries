using System;
using System.Collections.Generic;
using System.IO;
using Acr.Mail.RazorParser;
using NUnit.Framework;


namespace Acr.Mail.Tests {
    
    [TestFixture]
    public class RazorParserTests {
        
        [Test]
        public void Test() {
            var parser = new RazorTemplateParser();
            //var mail = parser.Parse(
            //    new MailTemplate {
            //        Content = File.ReadAllText("razor.xml")
            //    }, 
            //    new Dictionary<string, object> {
            //        { "Test", "Test" },
            //        { "Subject", "Test" }
            //    }
            //);

            //Assert.False(mail.IsBodyHtml);
            //Assert.AreEqual("Test", mail.Subject);
            //Assert.True(mail.Body.Contains("POOF"));
            //Assert.True(mail.Body.Contains("WORKS"));
        }
    }
}