using EasyTranslate.Enums;
using EasyTranslate.Factories;
using EasyTranslate.Implementations;
using EasyTranslate.Translators;

namespace EasyTranslateTests
{
    public static class TestUtilities
    {
        public static TranslateWrapper Initialize(
            DriverTypes driverType = DriverTypes.PhantomJSDriver,
            string path = @"C:\Users\Raphael\Documents\Programming\C#\Packages\EasyTranslate\EasyTranslateTests")
        {           
            ITranslator translator = new SeleniumClassicGoogleTranslator();

            var driverFactory = new RemoteWebDriverFactory();

            IRemoteWebDriver driver = driverFactory.Create(driverType, path);

            var wrapper = new TranslateWrapper
            {
                Translator = translator,
                Driver = driver
            };

            return wrapper;
        }
    }
}