using System;
using GoogleTranslate.Enums;
using GoogleTranslate.Implementations;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.PhantomJS;

namespace GoogleTranslate.Factories
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
                return new RealRemoteWebDriver(new ChromeDriver((ChromeDriverService) driverService)
                {
                    Url = "https://translate.google.com/"
                });
            }

            if (driverType == typeof(ChromeDriver))
            {
                return new MockRemoteWebDriver
                {
                    Url = "https://translate.google.com/"
                };
            }

            return new RealRemoteWebDriver(new PhantomJSDriver((PhantomJSDriverService) driverService)
            {
                Url = "https://translate.google.com/"
            });
        }

        private static DriverService CreateService(Type driverType, string path)
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