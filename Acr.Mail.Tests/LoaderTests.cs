using System;
using System.Globalization;
using Acr.Mail.Loaders;
using NUnit.Framework;


namespace Acr.Mail.Tests {
    
    [TestFixture]
    public class LoaderTests {

        [Test]
        public void ResourceLoaderTest() {
            var loader = new ResourceMailTemplateLoader(Config.TemplateNamespace, Config.TemplateExtension);
            var tmpl = loader.Load("resource", CultureInfo.CurrentCulture);
            Assert.NotNull(tmpl);
        }


        [Test]
        public void FileLoaderTest() {
            var loader = new FileMailTemplateLoader(Config.TemplateDirectory, Config.TemplateExtension);
            var tmpl = loader.Load("serialization", CultureInfo.CurrentCulture);
            Assert.NotNull(tmpl);
        }
    }
}
