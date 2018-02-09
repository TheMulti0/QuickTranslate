using EasyTranslate.Enums;
using EasyTranslate.Words;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EasyTranslateTests.Phantom
{
    [TestClass]
    public class DetectionPhantomTests
    {

        [TestMethod]
        public void Test1()
        {
            TranslateWrapper result = TestUtilities.Initialize(DriverTypes.PhantomJSDriver);

            TranslateWord translatedWord = result.Translator
                .Detect(new TranslateWord("Hi"), result.Driver);
            
            result.Driver.Quit();
            Assert.AreEqual(TranslateLanguages.English ,translatedWord.Language);
        }

        [TestMethod]
        public void Test2()
        {
            TranslateWrapper result = TestUtilities.Initialize(DriverTypes.PhantomJSDriver);

            TranslateWord translatedWord = result.Translator
                .Detect(new TranslateWord("hi"), result.Driver);
            
            result.Driver.Quit();
            Assert.AreNotEqual("", translatedWord.Language);
        }

        [TestMethod]
        public void Test3()
        {
            TranslateWrapper result = TestUtilities.Initialize(DriverTypes.PhantomJSDriver);

            TranslateWord translatedWord = result.Translator
                .Detect(new TranslateWord("hi"), result.Driver);
            
            result.Driver.Quit();
            Assert.AreNotEqual(null, translatedWord.Language);
        }

        [TestMethod]
        public void Test4()
        {
            TranslateWrapper result = TestUtilities.Initialize(DriverTypes.PhantomJSDriver);
            
            TranslateWord translatedWord = result.Translator
                .Detect(new TranslateWord("asdfdsdgfgd"), result.Driver);
                        
            result.Driver.Quit();
            Assert.AreNotEqual("Detect language", translatedWord.Language);
        }
        
    }
}