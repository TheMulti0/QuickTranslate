using EasyTranslate.TranslationData;
using EasyTranslate.Translators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EasyTranslate.Tests
{
    [TestClass]
    public class TranslateTests
    {
        [TestMethod]
        public void HebrewToEnglish()
        {
            TranslationSequence result =
                new GoogleTranslator().TranslateAsync(new TranslationSequence("שלום"), TranslateLanguages.English)
                    .Result;
            const string shouldBe = "hello";
            string lower = result.Word.ToLower();
            if (lower != shouldBe)
            {
                Assert.Fail($"Result was {lower} instead of {shouldBe}");
            }
        }

        [TestMethod]
        public void HebrewToEnglish2()
        {
            TranslationSequence result =
                new GoogleTranslator().TranslateAsync(new TranslationSequence("שלום!"), TranslateLanguages.English)
                    .Result;
            const string shouldBe = "peace!";
            string lower = result.Word.ToLower();
            if (lower != shouldBe)
            {
                Assert.Fail($"Result was {lower} instead of {shouldBe}");
            }
        }

        [TestMethod]
        public void HebrewToEnglish3()
        {
            TranslationSequence result =
                new GoogleTranslator().TranslateAsync(new TranslationSequence("מה נשמע"), TranslateLanguages.English)
                    .Result;
            const string shouldBe = "what's new";
            string lower = result.Word.ToLower();
            if (lower != shouldBe)
            {
                Assert.Fail($"Result was {lower} instead of {shouldBe}");
            }
        }
    }
}