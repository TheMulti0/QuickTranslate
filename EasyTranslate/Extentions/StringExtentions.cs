﻿using System;
using System.Linq;

namespace EasyTranslate.Extentions
{
    public static class StringExtensions
    {
        public static string CapitalizeFirstLetter(this string input)
        {
            switch (input)
            {
                case null:
                    throw new ArgumentNullException(nameof(input));
                case "":
                    throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default:
                    return input.First().ToString().ToUpper() + input.Substring(1);
            }
        }
    }
}