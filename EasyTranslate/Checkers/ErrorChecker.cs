using System.Collections.Generic;
using EasyTranslate.Enums;
using EasyTranslate.Words;

namespace EasyTranslate.Checkers
{
    internal class ErrorChecker
    {
        private List<string> StringErrorsList { get; }

        public ErrorChecker()
        {
            StringErrorsList = new List<string>
            {
                "Detect language",
                "Detect",
                "Language",
                " - detected",
                " -",
                "-",
                " ",
                ""
            };
        }

        public bool CheckIfError(TranslateWord word)
        {
            foreach (var item in StringErrorsList)
            {
                if (word.Word == item)
                {
                    return true;
                }
                if (EnumParser.GetDescriptionAttributeString(word.Language) == item)
                {
                    return true;
                }
            }
            return false;
        }
    }
}