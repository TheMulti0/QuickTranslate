using System;

namespace EasyTranslate.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class DriverTypeAttribute : Attribute
    {
        public Type DriverType { get; }

        public DriverTypeAttribute(Type driverType)
        {
            DriverType = driverType;
        }
    }
}