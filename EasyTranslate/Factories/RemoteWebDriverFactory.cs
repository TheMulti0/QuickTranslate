using System;
using EasyTranslate.Enums;
using EasyTranslate.Implementations;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.PhantomJS;

namespace EasyTranslate.Factories
{
    public class RemoteWebDriverFactory : IRemoteWebDriverFactory
    {
        public IRemoteWebDriver Create(DriverTypes driverType, string path)
        {
            Type type = EnumParser.GetDriverTypeAttributeType(driverType);

            return CreateDriver(type, path);
        }

        public IRemoteWebDriver Create(Type driverType, string path)
        {
            return CreateDriver(driverType, path);
        }

        private static IRemoteWebDriver CreateDriver(Type driverType, string path)
        {
            DriverService driverService = CreateService(driverType, path);

            if (driverType == typeof(ChromeDriver))
            {
                return new RealRemoteWebDriver(new ChromeDriver((ChromeDriverService) driverService));
            }

            if (driverType == typeof(FirefoxDriver))
            {
                return new RealRemoteWebDriver(new FirefoxDriver((FirefoxDriverService) driverService));
            }

            if (driverType == typeof(InternetExplorerDriverService))
            {
                return new RealRemoteWebDriver(new InternetExplorerDriver((InternetExplorerDriverService) driverService));
            }

            //if (driverType == typeof(MockRemoteWebDriver))
            //{
            //    return new MockRemoteWebDriver()
            //}

            return new RealRemoteWebDriver(new PhantomJSDriver((PhantomJSDriverService) driverService));
        }

        private static DriverService CreateService(Type driverType, string path)
        {
            DriverService service = null;
            if (driverType == typeof(ChromeDriver))
            {
                service = ChromeDriverService.CreateDefaultService(path);
            }

            if (driverType == typeof(FirefoxDriver))
            {
                service = FirefoxDriverService.CreateDefaultService(path);
            }

            if (driverType == typeof(InternetExplorerDriver))
            {
                service = InternetExplorerDriverService.CreateDefaultService(path);
            }

            if (service == null)
            {
                service = PhantomJSDriverService.CreateDefaultService(path);
            }

            service.HideCommandPromptWindow = true;
            return service;
        }
    }
}