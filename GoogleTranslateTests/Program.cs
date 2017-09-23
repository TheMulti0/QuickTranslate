using System;
using GoogleTranslate.Enums;
using GoogleTranslate.Factories;
using GoogleTranslate.Translators;
using GoogleTranslate.Words;
using OpenQA.Selenium.Remote;

namespace GoogleTranslateTests
{
    public class Program
    {
        public static void Main()
        {
            var translator = new SeleniumClassicGoogleTranslator();

            RemoteWebDriver driver = SeleniumDriverFactory.Factory(DriverTypes.PhantomJSDriver,
                @"C:\Users\Raphael\Documents\Programming\C#\Packages\GoogleTranslate\GoogleTranslateTests");

            new Program().TranslateDetect(translator, driver);
        }

        public void TranslateDetect(ITranslator<TranslateLanguages, RemoteWebDriver> translator, RemoteWebDriver remoteWebDriver)
        {
            try
            {
                Console.WriteLine("Type a word to translate (right now automaticly translated to raphael's mind");
                var word = Console.ReadLine();
                var trans = translator.Translate(new TranslateWord(word), TranslateLanguages.French, remoteWebDriver);
                Console.WriteLine($"word: {trans.Word} language: {trans.Language}");
            }
            catch (Exception e)
            {
                Console.WriteLine(" Translate Failed " + e);
            }

            try
            {
                Console.WriteLine("Write a word to detect its language");
                var word2 = Console.ReadLine();
                var det = translator.Detect(new TranslateWord(word2), remoteWebDriver);
                Console.WriteLine($"word: {det.Word} language: {det.Language}");
            }
            catch (Exception e)
            {
                Console.WriteLine(" Translate Failed " + e);
            }

            remoteWebDriver.Quit();
            Console.ReadLine();
        }
    }
}
