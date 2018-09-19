using System;
using EasyTranslate.TranslationData;
using EasyTranslate.Translators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EasyTranslate.Tests
{
    [TestClass]
    public class WordTranslationTests
    {
        private static string GetSequence(
            string sequence,
            TranslateLanguages language,
            TranslateLanguages? source = null)
        {
            ITranslator translator = new GoogleTranslator();
            return translator.TranslateAsync(new TranslationSequence(sequence), language, source).Result.Sequence;
        }

        [TestMethod]
        public void EnglishToFrench()
        {
            var sequence = GetSequence("hello", TranslateLanguages.French);
            if (sequence.ToLower() != "bonjour ")
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void SourceEnglishToFrench()
        {
            var sequence = GetSequence("hello", TranslateLanguages.French, TranslateLanguages.English);
            if (sequence.ToLower() != "bonjour ")
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void HebrewToEnglish()
        {
            var sequence = GetSequence("שלום", TranslateLanguages.English);
            if (sequence.ToLower() != "peace")
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void SourceHebrewToEnglish()
        {
            var sequence = GetSequence("שלום", TranslateLanguages.English, TranslateLanguages.Hebrew);
            if (sequence.ToLower() != "peace")
            {
                Assert.Fail();
            }
        }
    }
}
