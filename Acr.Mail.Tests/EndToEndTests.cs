using System;
using Acr.Mail.Loaders;
using Acr.Mail.NVelocityParser;
using Acr.Mail.Senders;
using NUnit.Framework;


namespace Acr.Mail.Tests {
    
    [TestFixture]
    public class EndToEndTests {

        [Test]
        public void MailFactoryTest() {
            var mailFactory = new MailFactory(
                new FileMailTemplateLoader(Config.TemplateDirectory),
                new NVelocityTemplateParser(),
                new VoidMailSender()
            );

        }
    }
}
