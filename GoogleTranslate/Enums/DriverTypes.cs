using GoogleTranslate.Attributes;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.PhantomJS;

namespace GoogleTranslate.Enums
{
    public enum DriverTypes
    {
        [DriverTypeAttribute(typeof(ChromeDriver))]
        ChromeDriver,
        [DriverTypeAttribute(typeof(PhantomJSDriver))]
        PhantomJSDriver,
        [DriverTypeAttribute(typeof(FirefoxDriver))]
        FirefoxDriver,
        [DriverTypeAttribute(typeof(InternetExplorerDriver))]
        InternetExplorerDriver
    }
}
