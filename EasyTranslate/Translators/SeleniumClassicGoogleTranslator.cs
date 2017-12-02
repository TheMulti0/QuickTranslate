using System;
using System.Collections.Generic;
using System.Threading;
using EasyTranslate.Checkers;
using EasyTranslate.Enums;
using EasyTranslate.Exceptions;
using EasyTranslate.Implementations;
using EasyTranslate.Words;
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;

namespace EasyTranslate.Translators
{
    public class SeleniumClassicGoogleTranslator : ITranslator
    {
        private readonly ErrorChecker _errorChecker;
        private readonly int _maxMethodLoop;
        private readonly double _timeFailsMilliseconds;
        private int _detectCounter;
        private int _translateCounter;

        private IRemoteWebDriver Driver { get; set; }

        private IWebElement ResultBox => Driver.FindElementById("result_box");

        public SeleniumClassicGoogleTranslator(int maxMethodLoop = 3, double timeFailsMilliseconds = 10000)
        {
            _maxMethodLoop = maxMethodLoop;
            _timeFailsMilliseconds = timeFailsMilliseconds;
            _errorChecker = new ErrorChecker();
        }

        public TranslateWord Translate(
            TranslateWord word,
            TranslateLanguages targetLanguage,
            IRemoteWebDriver driver)
        {
            Driver = driver;

            if (!InternetChecker.CheckForInternetConnection())
            {
                throw new NoInternetConnectionAvaliableExcpetion();
            }

            try
            {
                var result = GetTranslatedText(word, targetLanguage, driver);

                TranslateWord newWord = WrapTranslateWord(word, driver, result, targetLanguage);
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
            Driver = driver;

            try
            {
                var detectedLangString = GetDetectLanguageString(word, driver);

                TranslateWord newWord = WrapDetectWord(word, detectedLangString);
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

        private string GetTranslatedText(
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

            bool Condition()
            {
                return ResultBox.Text?.Length > 0;
            }

            CheckConditionWait(Condition);

            return ResultBox.Text;
        }

        private TranslateWord WrapTranslateWord(
            TranslateWord word,
            IRemoteWebDriver driver,
            string result,
            TranslateLanguages language)
        {
            if (result == "")
            {
                IWebElement translateButton = driver.FindElementById("gt-submit");
                translateButton.Click();
            }

            var translateWord = new TranslateWord(result, language);
            return translateWord;
        }

        private string GetDetectLanguageString(
            TranslateWord word,
            IRemoteWebDriver driver)
        {
            driver.Url = "https://translate.google.com/";

            IWebElement detectButton = driver.FindElementByXPath("//*[@id=\"gt-sl-sugg\"]/div[5]");
            detectButton.Click();

            if (ResultBox.Text != word.Word)
            {
                TypeWordInTextBox(word, driver);
            }

            bool Condition()
            {
                return !detectButton.Text.Contains("Detect language");
            }

            CheckConditionWait(Condition);

            return detectButton.Text;
        }

        private TranslateWord WrapDetectWord(
            TranslateWord word,
            string detectedLangString)
        {
            var lang = detectedLangString?.Replace(" - detected", "");

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
                IEnumerable<IWebElement> langButtons = driver.FindElements(langItem);
                foreach (IWebElement element in langButtons)
                {
                    if (element.Location.X >= 439)
                    {
                        element.Click();
                        return;
                    }
                }
                throw new NoSuchElementException();
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

        private void CheckConditionWait(Func<bool> condition)
        {
            var counter = 0;
            while (!condition())
            {
                Thread.Sleep(100);
                counter += 100;
                if (Math.Abs(counter - _timeFailsMilliseconds) < 0)
                {
                    return;
                }
            }
        }
    }
}