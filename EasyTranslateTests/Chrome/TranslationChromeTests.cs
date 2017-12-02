using GoogleTranslate.Enums;
using GoogleTranslate.Words;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EasyTranslateTests.Chrome
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
            
            result.Driver.Quit();
            Assert.AreEqual("Hello", translatedWord.Word);
        }

        [TestMethod]
        public void Test2()
        {
            TranslateWrapper result = TestUtilities.Initialize(DriverTypes.ChromeDriver);

            TranslateWord translatedWord = result.Translator
                .Translate(new TranslateWord("bonjour"), TranslateLanguages.English, result.Driver);
            
            result.Driver.Quit();
            Assert.AreEqual("Hello", translatedWord.Word);
        }

        [TestMethod]
        public void Test3()
        {
            TranslateWrapper result = TestUtilities.Initialize(DriverTypes.ChromeDriver);

            TranslateWord translatedWord = result.Translator
                .Translate(new TranslateWord("asdfdsdgfgd"), TranslateLanguages.English, result.Driver);
            
            result.Driver.Quit();
            Assert.AreEqual("asdfdsdgfgd", translatedWord.Word);
        }

        [TestMethod]
        public void Test4()
        {
            TranslateWrapper result = TestUtilities.Initialize(DriverTypes.ChromeDriver);

            TranslateWord translatedWord = result.Translator
                .Translate(new TranslateWord("asdfdsdgfgd"), TranslateLanguages.English, result.Driver);
            
            result.Driver.Quit();
            Assert.AreNotEqual("", translatedWord.Word);
        }
    }
}