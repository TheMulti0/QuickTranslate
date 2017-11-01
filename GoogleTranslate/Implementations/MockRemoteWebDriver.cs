using System.Collections.Generic;
using System.Collections.ObjectModel;
using OpenQA.Selenium;

namespace GoogleTranslate.Implementations
{
    public class MockRemoteWebDriver : IRemoteWebDriver
    {
        public IWebElement FindElementById(string id)
        {
            return new MockWebElement();
        }

        public ReadOnlyCollection<IWebElement> FindElementsById(string id)
        {
            return new ReadOnlyCollection<IWebElement>(new List<IWebElement>());
        }

        public string Url { get; set; }

        public IWebElement FindElementByXPath(string xpath)
        {
            return new MockWebElement();
        }

        public IWebElement FindElement(By by)
        {
            return new MockWebElement();
        }

        public IEnumerable<IWebElement> FindElements(By trait)
        {
            return new List<IWebElement>();
        }

        public void Quit()
        {

        }
    }
}