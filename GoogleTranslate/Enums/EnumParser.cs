using System;
using System.ComponentModel;
using System.Reflection;
using GoogleTranslate.Attributes;

namespace GoogleTranslate.Enums
{
    internal static class EnumParser
    {
        public static string GetDescriptionAttributeString(Enum @enum)
        {
            FieldInfo fieldInfo = @enum.GetType().GetField(@enum.ToString());
            var attributes =
                (DescriptionAttribute[]) fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0
                ? attributes[0].Description
                : throw new CustomAttributeFormatException($"No DescriptionAttribute found. {attributes}");
        }

        public static Type GetDriverTypeAttributeType(Enum @enum)
        {
            FieldInfo fieldInfo = @enum.GetType().GetField(@enum.ToString());
            var attributes =
                (DriverTypeAttribute[])fieldInfo.GetCustomAttributes(typeof(DriverTypeAttribute), false);
            return attributes.Length > 0
                ? attributes[0].DriverType
                : throw new CustomAttributeFormatException($"No DriverTypeAttribute found. {attributes}");
        }

        public static Enum GetEnum(string value, Type @enum)
        {
            var names = Enum.GetNames(@enum);
            foreach (var name in names)
            {
                var enumValue = (Enum) Enum.Parse(@enum, name);

                if (GetDescriptionAttributeString(enumValue) == value)
                {
                    return enumValue;
                }
            }

            throw new ArgumentException("The string does not exist in the enum.");
        }

    }
}