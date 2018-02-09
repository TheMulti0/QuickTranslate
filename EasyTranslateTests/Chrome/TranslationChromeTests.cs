using System;
using System.Collections.Specialized;
using System.Net;
using System.Web;
using EasyTranslate.Enums;
using EasyTranslate.Words;
using EasyTranslate.Translators;
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

        [TestMethod]
        public void Test5()
        {
            var builder = new UriBuilder("https://translate.google.com/translate_a/single");
            NameValueCollection query = HttpUtility.ParseQueryString(builder.Query);

            query["client"] = "t";

            query["sl"] = "auto";

            query["tl"] = "fr";

            query["hl"] = "fr";

            query["dt"] = "[\'at\', \'bd\', \'ex\', \'ld\', \'md\', \'qca\', \'rw\', \'rm\', \'ss\', \'t\']";

            query["ie"] = "UTF-8";

            query["oe"] = "UTF-8";

            query["otf"] = "1";

            query["ssel"] = "0";

            query["tsel"] = "0";

            query["kc"] = "7";

            query["q"] = "hi";

            builder.Query = query.ToString();
            //https://translate.google.com/translate_a/single?client=t&sl=auto&tl=en&hl=en&dt=['at', 'bd', 'ex', 'ld', 'md', 'qca', 'rw', 'rm', 'ss', 't']&ie=UTF-8&oe=UTF-8&otf=1&ssel=0&tsel=0&kc=7&q=bonjour
            WebRequest w = WebRequest.Create(builder.Uri);
            var r = w.GetResponse();
            Console.ReadLine();
        }
    }
}