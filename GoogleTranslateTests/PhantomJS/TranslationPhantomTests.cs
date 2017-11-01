using GoogleTranslate.Enums;
using GoogleTranslate.Words;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GoogleTranslateTests.PhantomJS
{
    [TestClass]
    public class TranslationPhantomTests
    {
        [TestMethod]
        public void Test1()
        {
            TranslateWrapper result = TestUtilities.Initialize();

            TranslateWord translatedWord = result.Translator
                .Translate(new TranslateWord("Bonjour"), TranslateLanguages.English, result.Driver);

            Assert.AreEqual("Hello", translatedWord.Word);
        }

        [TestMethod]
        public void Test2()
        {
            TranslateWrapper result = TestUtilities.Initialize();

            TranslateWord translatedWord = result.Translator
                .Translate(new TranslateWord("Bonjour"), TranslateLanguages.English, result.Driver);
            
            Assert.AreNotEqual("-", translatedWord.Word);
        }
        
    }
}
