using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;

namespace EasyTranslate.Enums
{
    public static class EnumParser
    {
        public static string GetDescriptionAttributeString(this Enum @enum)
        {
            FieldInfo fieldInfo = @enum.GetType().GetField(@enum.ToString());
            var attributes =
                (DescriptionAttribute[]) fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0
                ? attributes[0].Description
                : throw new CustomAttributeFormatException($"No DescriptionAttribute found. {attributes}");
        }

        public static Enum GetEnum(this string value, Type enumType)
        {
            IEnumerable<string> names = Enum.GetNames(enumType);
            foreach (var name in names)
            {
                var enumValue = (Enum) Enum.Parse(enumType, name);

                if (GetDescriptionAttributeString(enumValue) == value)
                {
                    return enumValue;
                }
            }

            Debug.WriteLine(value);
            throw new ArgumentException("The string does not exist in the enum.");
        }
    }
}