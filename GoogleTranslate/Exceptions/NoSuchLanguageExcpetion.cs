using System;
using GoogleTranslate.Enums;

namespace GoogleTranslate.Exceptions
{
    public class NoSuchLanguageExcpetion : Exception
    {
        public TranslateLanguages FailedLanguage { get; }
        
        public NoSuchLanguageExcpetion()
        {
        }

        public NoSuchLanguageExcpetion(string message)
            : base(message)
        {
        }

        public NoSuchLanguageExcpetion(
            string message, 
            Exception inner)
            : base(message, inner)
        {
        }

        public NoSuchLanguageExcpetion(
            string message, 
            TranslateLanguages failedLanguage)

            : base(message)
        {
            FailedLanguage = failedLanguage;
        }

        public NoSuchLanguageExcpetion(
            string message, 
            Exception inner, 
            TranslateLanguages failedLanguage)

            : base(message, inner)
        {
            FailedLanguage = failedLanguage;
        }
    }
}
