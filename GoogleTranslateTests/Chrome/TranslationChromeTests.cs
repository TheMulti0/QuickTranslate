using System.Security.Policy;
using GoogleTranslate.Enums;
using GoogleTranslate.Words;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GoogleTranslateTests.Chrome
{
    [TestClass]
    public class TranslationChromeTests
    {

        [TestMethod]
        public void Test1()
        {
            TranslateWrapper result = TestUtilities.Initialize(DriverTypes.ChromeDriver);

            TranslateWord translatedWord = result.Translator
                .Translate(new TranslateWord("Bonjour"), TranslateLanguages.English, result.Driver);

            Assert.AreEqual("Hello", translatedWord.Word);
        }
    }
}