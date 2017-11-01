using System;
using System.Collections.Generic;
using System.Threading;
using GoogleTranslate.Checkers;
using GoogleTranslate.Enums;
using GoogleTranslate.Exceptions;
using GoogleTranslate.Implementations;
using GoogleTranslate.Words;
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;

namespace GoogleTranslate.Translators
{
    public class SeleniumClassicGoogleTranslator : ITranslator
    {
        private readonly ErrorChecker _errorChecker;
        private readonly int _maxMethodLoop;
        private int _detectCounter;
        private int _translateCounter;

        public SeleniumClassicGoogleTranslator(int maxMethodLoop = 3)
        {
            _maxMethodLoop = maxMethodLoop;
            _errorChecker = new ErrorChecker();
        }

        public TranslateWord Translate(
            TranslateWord word,
            TranslateLanguages targetLanguage,
            IRemoteWebDriver driver)
        {
            if (!InternetChecker.CheckForInternetConnection())
            {
                throw new NoInternetConnectionAvaliableExcpetion();
            }

            try
            {
                var result = GetTranslatedText(word, targetLanguage, driver);

                TranslateWord newWord = WrapTranslateWord(word, driver, result);
                if (_errorChecker.CheckIfError(newWord))
                {
                    if (_translateCounter == _maxMethodLoop)
                    {
                        throw new TranslateFailedException("Too many errors translating");
                    }

                    _translateCounter++;
                    Translate(word, targetLanguage, driver);
                    return newWord;
                }
                return newWord;
            }
            catch (Exception e)
            {
                throw new TranslateFailedException(e);
            }
        }

        public TranslateWord Detect(
            TranslateWord word,
            IRemoteWebDriver driver)
        {
            try
            {
                IWebElement detectButton = GetDetectLanguage(word, driver);
                Thread.Sleep(TimeSpan.FromMilliseconds(word.Word.Length * 125));

                TranslateWord newWord = WrapDetectWord(word, detectButton);
                if (_errorChecker.CheckIfError(newWord))
                {
                    if (_detectCounter == _maxMethodLoop)
                    {
                        throw new DetectFailedException();
                    }
                    _detectCounter++;
                    Detect(word, driver);
                    return newWord;
                }
                return newWord;
            }
            catch (Exception e)
            {
                throw new DetectFailedException(e);
            }
        }

        private static string GetTranslatedText(
            TranslateWord word,
            TranslateLanguages targetLanguage,
            IRemoteWebDriver driver)
        {
            const string url = "https://translate.google.com/";
            if (driver.Url != url)
            {
                driver.Url = url;
            }

            TypeWordInTextBox(word, driver);
            SelectTranslatedLanguage(targetLanguage, driver);
            
            Thread.Sleep(TimeSpan.FromMilliseconds(word.Word.Length * 125));
            
            IWebElement resultBox = driver.FindElementById("result_box");
            var result = resultBox.Text;
            return result;
        }

        private TranslateWord WrapTranslateWord(
            TranslateWord word,
            IRemoteWebDriver driver,
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
            IRemoteWebDriver driver)
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
            IRemoteWebDriver driver)
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