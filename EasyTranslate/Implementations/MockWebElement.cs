using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using OpenQA.Selenium;

namespace EasyTranslate.Implementations
{
    internal class MockWebElement : IWebElement
    {
        public IWebElement FindElement(By by)
        {
            return null;
        }

        public ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            return 
                new ReadOnlyCollection<IWebElement>(
                    new List<IWebElement>
                    {
                    });
        }

        public void Clear()
        {
        }

        public void SendKeys(string text)
        {
        }

        public void Submit()
        {
        }

        public void Click()
        {
        }

        public string GetAttribute(string attributeName)
        {
            return "";
        }

        public string GetProperty(string propertyName)
        {
            return "";
        }

        public string GetCssValue(string propertyName)
        {
            return "";
        }

        public string TagName { get; }

        public string Text { get; }

        public bool Enabled { get; }

        public bool Selected { get; }

        public Point Location { get; }

        public Size Size { get; }

        public bool Displayed { get; }
    }
}