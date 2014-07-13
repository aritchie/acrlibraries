using System;
using System.IO;
using System.Net.Mail;
using Acr.Mail.Serialization;
using NUnit.Framework;


namespace Acr.Mail.Tests {
    
    [TestFixture]
    public class XmlParserTests {

        //[Test]
        //public void SerializeMailMessage() {
            
        //    var address = new MailAddress("test@test.com", "test test");
        //    var other = new MailAddress("test2@test2.com", "test2 test2");
            
        //    var mail = new MailMessage(address, address);

        //    //    HtmlContent = "<html><body><h1>Testing HTML Body</h1></body></html>",
        //    //    PlainTextContent = "Test"
        //    mail.To.Add(other);
        //    mail.CC.Add(address);
        //    mail.Bcc.Add(address);
        //    mail.Priority = MailPriority.High;
        //    mail.Subject = "Test Subject";
        //    mail.Headers.Add("Message-ID", "test");
        //    mail.Headers.Add("Reply-To-Message-ID", "test");

        //    var @string = XmlMailSerializer.Serialize(mail);
        //    Assert.NotNull(@string);
        //}


        [Test]
        public void DeserializeMailMessage() {
            var @string = File.ReadAllText("serialization.xml");

            var mail = XmlMailSerializer.Deserialize(@string);
            Assert.AreEqual(mail.Priority, MailPriority.Low);
            Assert.True(mail.IsBodyHtml);
        }
    }
}
