using GoogleTranslate.Enums;
using GoogleTranslate.Words;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EasyTranslateTests.Chrome
{
    [TestClass]
    public class DetectionChromeTests
    {

        [TestMethod]
        public void Test1()
        {
            TranslateWrapper result = TestUtilities.Initialize(DriverTypes.ChromeDriver);

            TranslateWord translatedWord = result.Translator
                .Detect(new TranslateWord("Hi"), result.Driver);
            
            result.Driver.Quit();
            Assert.AreEqual(TranslateLanguages.English ,translatedWord.Language);
        }

        [TestMethod]
        public void Test2()
        {
            TranslateWrapper result = TestUtilities.Initialize(DriverTypes.ChromeDriver);

            TranslateWord translatedWord = result.Translator
                .Detect(new TranslateWord("hi"), result.Driver);
            
            result.Driver.Quit();
            Assert.AreNotEqual("", translatedWord.Language);
        }

        [TestMethod]
        public void Test3()
        {
            TranslateWrapper result = TestUtilities.Initialize(DriverTypes.ChromeDriver);

            TranslateWord translatedWord = result.Translator
                .Detect(new TranslateWord("hi"), result.Driver);
            
            result.Driver.Quit();
            Assert.AreNotEqual(null, translatedWord.Language);
        }

        [TestMethod]
        public void Test4()
        {
            TranslateWrapper result = TestUtilities.Initialize(DriverTypes.ChromeDriver);
            
            TranslateWord translatedWord = result.Translator
                .Detect(new TranslateWord("asdfdsdgfgd"), result.Driver);
                        
            result.Driver.Quit();
            Assert.AreNotEqual("Detect language", translatedWord.Language);
        }
        
    }
}