using System.Collections.Generic;
using System.Collections.ObjectModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace GoogleTranslate.Implementations
{
    public class RealRemoteWebDriver : IRemoteWebDriver
    {
        private readonly RemoteWebDriver _inner;

        public RealRemoteWebDriver(RemoteWebDriver inner)
        {
            _inner = inner;
        }

        public string Url
        {
            get => _inner.Url;
            set => _inner.Url = value;
        }

        public IWebElement FindElementById(string id)
        {
            return _inner.FindElementById(id);
        }

        public ReadOnlyCollection<IWebElement> FindElementsById(string id)
        {
            return _inner.FindElementsById(id);
        }

        public IWebElement FindElementByXPath(string xpath)
        {
            return _inner.FindElementByXPath(xpath);
        }

        public IWebElement FindElement(By by)
        {
            return _inner.FindElement(by);
        }

        public IEnumerable<IWebElement> FindElements(By trait)
        {
            return _inner.FindElements(trait);
        }

        public void Quit()
        {
            _inner.Quit();
        }
    }
}