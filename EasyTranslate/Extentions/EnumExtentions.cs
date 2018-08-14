using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;

namespace EasyTranslate.Extentions
{
    public static class EnumExtentions
    {
        public static string GetDescriptionAttributeString(this Enum @enum)
        {
            FieldInfo fieldInfo = @enum.GetType().GetField(@enum.ToString());
            DescriptionAttribute[] attributes =
                (DescriptionAttribute[]) fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0
                ? attributes[0].Description
                : throw new CustomAttributeFormatException($"No DescriptionAttribute found. {attributes}");
        }

        public static Enum GetEnumByDescription(this string value, Type enumType)
        {
            IEnumerable<string> names = Enum.GetNames(enumType);
            foreach (string name in names)
            {
                var enumValue = (Enum) Enum.Parse(enumType, name);

                if (GetDescriptionAttributeString(enumValue) == value)
                {
                    return enumValue;
                }
            }

            throw new ArgumentException("The string does not exist in the enum.");
        }
    }
}