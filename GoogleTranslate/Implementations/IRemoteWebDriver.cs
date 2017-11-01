using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;

namespace GoogleTranslate.Implementations
{
    public interface IRemoteWebDriver : IFindsById
    {
        string Url { get; set; }

        IWebElement FindElementByXPath(string xpath);

        IWebElement FindElement(By by);

        IEnumerable<IWebElement> FindElements(By trait);

        void Quit();
    }
}