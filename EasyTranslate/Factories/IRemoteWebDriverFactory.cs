using System;
using EasyTranslate.Enums;
using EasyTranslate.Implementations;

namespace EasyTranslate.Factories
{
    public interface IRemoteWebDriverFactory
    {
        IRemoteWebDriver Create(DriverTypes driverType, string path);

        IRemoteWebDriver Create(Type driverType, string path);
    }
}