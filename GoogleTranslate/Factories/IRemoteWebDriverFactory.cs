using System;
using GoogleTranslate.Enums;
using GoogleTranslate.Implementations;

namespace GoogleTranslate.Factories
{
    public interface IRemoteWebDriverFactory
    {
        IRemoteWebDriver Create(DriverTypes driverType, string path);

        IRemoteWebDriver Create(Type driverType, string path);
    }
}