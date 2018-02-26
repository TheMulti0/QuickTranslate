using System;
using EasyTranslate.Enums;
using EasyTranslate.Translators;
using EasyTranslate.Words;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EasyTranslateTests.Requests
{
    [TestClass]
    public class TranslationRequestsTests
    {
        [TestMethod]
        public void Test()
        {
            var translator = new GoogleTranslateClassicTranslator();
            translator.Translate(new TranslateWord("hello"), TranslateLanguages.French);

            Console.ReadLine();
        }
    }
}
