using EasyTranslate.TranslationData;
using EasyTranslate.Translators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EasyTranslate.Tests
{
    [TestClass]
    public class MassTranslationTests
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
        public void HebrewToEnglish()
        {
            var sequence =
                "שלום לכולם זו היא אפליקצית התרגום שלי שבניתי בעצמי בעזרת האתר של גוגל, גוגל תרגום.\r\nהאפליקציה מציעה מגוון שירותי תרגום מהירים במיוחד וקלים לשימוש. תודה רבה!";
            var result =
                "Hello everyone This is my own translation application that I built myself using Google\'s website, Google Translation.\r\nThe application offers a variety of fast, easy-to-use translation services. Thank you!";
            if (GetSequence(sequence, TranslateLanguages.English) != result)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void SourceHebrewToEnglish()
        {
            var sequence =
                "שלום לכולם זו היא אפליקצית התרגום שלי שבניתי בעצמי בעזרת האתר של גוגל, גוגל תרגום.\r\nהאפליקציה מציעה מגוון שירותי תרגום מהירים במיוחד וקלים לשימוש. תודה רבה!";
            var result =
                "Hello everyone This is my own translation application that I built myself using Google\'s website, Google Translation.\r\nThe application offers a variety of fast, easy-to-use translation services. Thank you!";
            if (GetSequence(sequence, TranslateLanguages.English, TranslateLanguages.Hebrew) != result)
            {
                Assert.Fail();
            }
        }
    }
}