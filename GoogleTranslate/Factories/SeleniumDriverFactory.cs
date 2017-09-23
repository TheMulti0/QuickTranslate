using System;
using GoogleTranslate.Enums;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Remote;

namespace GoogleTranslate.Factories
{
    public static class SeleniumDriverFactory
    {
        public static RemoteWebDriver Factory(DriverTypes driverType, string path = @"")
        {
            Type type = EnumParser.GetDriverTypeAttributeType(driverType);
            return DriverFactory(type, path);
        }

        public static RemoteWebDriver Factory(Type driverType, string path = @"")
        {
            return DriverFactory(driverType, path);
        }

        private static RemoteWebDriver DriverFactory(Type driverType, string path)
        {
            var driverService = ServiceFactory(driverType, path);

            if (driverType == typeof(ChromeDriver))
            {
                return new ChromeDriver((ChromeDriverService)driverService)
                {
                    Url = "https://translate.google.com/"
                };
            }

            return new PhantomJSDriver((PhantomJSDriverService)driverService)
            {
                Url = "https://translate.google.com/"
            };
        } 

        private static DriverService ServiceFactory(Type driverType, string path)
        {
            DriverService service = null;
            if (driverType == typeof(ChromeDriver))
            {
                service = ChromeDriverService.CreateDefaultService(path);
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
