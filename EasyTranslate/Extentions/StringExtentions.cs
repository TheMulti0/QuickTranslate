using System;
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

        internal static string GetTextBetween(this string source, string from, string to, int startIndex = 0)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (from == null)
            {
                throw new ArgumentNullException(nameof(@from));
            }

            if (to == null)
            {
                throw new ArgumentNullException(nameof(to));
            }

            if (startIndex < 0 || startIndex > source.Length - 1)
            {
                throw new ArgumentOutOfRangeException(nameof(to));
            }

            var index = source.IndexOf(from, startIndex, StringComparison.Ordinal);
            if (index == -1)
            {
                return null;
            }

            var index2 = source.IndexOf(to, index, StringComparison.Ordinal);
            if (index2 == -1)
            {
                return null;
            }

            var result = source.Substring(index + from.Length, index2 - index - from.Length);
            return result;
        }

    }
}
