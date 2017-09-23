using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace GoogleTranslate.Translators
{
    internal static class SeleniumExtensions
    {
        public static IWebElement FindElementByTraits(
            this RemoteWebDriver driver,
            List<By> traits)
        {
            var findElementByTraits = driver
                .FindElementListByTraits(traits)
                .FirstOrDefault();
            return findElementByTraits;
        }

        public static ReadOnlyCollection<IWebElement> FindElementsByTraits(
            this RemoteWebDriver driver,
            List<By> traits)
        {
            return driver
                .FindElementListByTraits(traits)
                .AsReadOnly();
        }

        private static List<IWebElement> FindElementListByTraits(
            this RemoteWebDriver driver,
            List<By> traits)
        {
            var list = new List<IWebElement>();
            var trait = traits.FirstOrDefault();
            if (trait == null)
            {
                return list;
            }
            try
            {
                var elements = driver.FindElements(trait).ToList();
                foreach (var otherTrait in traits.Skip(1))
                {
                    var otherElements = driver.FindElements(otherTrait);
                    elements = otherElements.Intersect(elements).ToList();
                    if (elements.Count == 0)
                    {
                        break;
                    }
                }
                list = elements;
            }
            catch (NoSuchElementException)
            {
                return list;
            }
            return list;
        }
    }
}