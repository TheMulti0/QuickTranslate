using GoogleTranslate.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GoogleTranslate.Factories;

namespace GoogleTranslateTests
{
    [TestClass]
    public class TranslateTests
    {

        [TestMethod]
        public void TestMethod1()
        {
            var a = SeleniumDriverFactory.Factory(DriverTypes.PhantomJSDriver,
                @"C:\Users\Raphael\Documents\Programming\C#\Packages\GoogleTranslate\GoogleTranslateTests");
        }
    }
}
