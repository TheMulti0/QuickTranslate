using EasyTranslate.Attributes;
using EasyTranslate.Implementations;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.PhantomJS;

namespace EasyTranslate.Enums
{
    public enum DriverTypes
    {
        [DriverType(typeof(ChromeDriver))]
        ChromeDriver,
        [DriverType(typeof(PhantomJSDriver))]
        PhantomJSDriver,
        [DriverType(typeof(FirefoxDriver))]
        FirefoxDriver,
        [DriverType(typeof(InternetExplorerDriver))]
        InternetExplorerDriver,
        [DriverType(typeof(MockRemoteWebDriver))]
        MockDriver
    }
}
