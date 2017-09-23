using System;
using System.Collections.Generic;
using System.Threading;
using GoogleTranslate.Checkers;
using GoogleTranslate.Enums;
using GoogleTranslate.Exceptions;
using GoogleTranslate.Words;
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using OpenQA.Selenium.Remote;

namespace GoogleTranslate.Translators
{
    public class SeleniumClassicGoogleTranslator : ITranslator<TranslateLanguages, RemoteWebDriver>
    {
        public TranslateWord Translate(
            TranslateWord word,
            TranslateLanguages targetLanguage,
            RemoteWebDriver driver)
        {
            if (!InternetChecker.CheckForInternetConnection())
            {
                throw new NoInternetConnectionAvaliableExcpetion();
            }

            try
            {
                var result = GetTranslateText(word, targetLanguage, driver);

                return WrapTranslateWord(word, driver, result);
            }
            catch (Exception e)
            {
                throw new TranslateFailedException("TranslateFailed", e);
            }
        }

        public TranslateWord Detect(
            TranslateWord word,
            RemoteWebDriver driver)
        {
            try
            {
                IWebElement detectButton = GetDetectLanguage(word, driver);
                Thread.Sleep(TimeSpan.FromSeconds(0.5));

                return WrapDetectWord(word, detectButton);
            }
            catch (Exception e)
            {
                throw new TranslateFailedException("TranslateFailed", e);
            }
        }

        private static string GetTranslateText(
            TranslateWord word, 
            TranslateLanguages targetLanguage,
            RemoteWebDriver driver)
        {
            const string url = "https://translate.google.com/";
            if (driver.Url != url)
            {
                driver.Url = url;
            }

            TypeWordInTextBox(word, driver);
            SelectTranslatedLanguage(targetLanguage, driver);

            IWebElement resultBox = driver.FindElementById("result_box");
            var result = resultBox.Text;
            return result;
        }

        private TranslateWord WrapTranslateWord(
            TranslateWord word, 
            RemoteWebDriver driver, 
            string result)
        {
            if (result == "")
            {
                IWebElement translateButton = driver.FindElementById("gt-submit");
                translateButton.Click();
            }

            var translateWord = new TranslateWord(result, Detect(word, driver).Language);
            return translateWord;
        }

        private static IWebElement GetDetectLanguage(
            TranslateWord word, 
            RemoteWebDriver driver)
        {
            driver.Url = "https://translate.google.com/";

            IWebElement detectButton = driver.FindElementByXPath("//*[@id=\"gt-sl-sugg\"]/div[5]");
            detectButton.Click();

            TypeWordInTextBox(word, driver);
            return detectButton;
        }

        private static TranslateWord WrapDetectWord(
            TranslateWord word, 
            IWebElement detectButton)
        {
            var lang = detectButton.Text?.Replace(" - detected", "");
            Enum @enum = EnumParser.GetEnum(lang, typeof(TranslateLanguages));

            var translateWord = new TranslateWord(word.Word, (TranslateLanguages) @enum);
            return translateWord;
        }

        private static void TypeWordInTextBox(
            TranslateWord word,
            IFindsById driver)
        {
            IWebElement textBox = driver.FindElementById("source");
            textBox.SendKeys(word.Word);
        }

        private static void SelectTranslatedLanguage(
            TranslateLanguages translateLanguage,
            RemoteWebDriver driver)
        {
            var lang = EnumParser.GetDescriptionAttributeString(translateLanguage);
            By langItem = By.XPath(
                $"//*[contains(text(), '{lang}')]");

            try
            {
                driver.FindElement(langItem);
            }
            catch (NoSuchElementException)
            {
                IWebElement moreButton = driver.FindElementById("gt-tl-gms");
                moreButton.Click();

                try
                {
                    var traits = new List<By>
                    {
                        langItem,
                        By.ClassName("goog-menuitem-content")
                    };
                    IWebElement langOption = driver.FindElementByTraits(traits);
                    langOption.Click();
                }
                catch (NoSuchElementException)
                {
                    throw new NoSuchLanguageExcpetion(
                        $"No language called '{EnumParser.GetDescriptionAttributeString(translateLanguage)}' found.",
                        translateLanguage);
                }
            }
        }
    }
}