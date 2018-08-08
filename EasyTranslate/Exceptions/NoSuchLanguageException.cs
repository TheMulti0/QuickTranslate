using System;
using EasyTranslate.Words;

namespace EasyTranslate.Exceptions
{
    public class NoSuchLanguageException : Exception
    {
        public TranslateLanguages FailedLanguage { get; }
        
        public NoSuchLanguageException()
        {
        }

        public NoSuchLanguageException(string message)
            : base(message)
        {
        }

        public NoSuchLanguageException(
            string message, 
            Exception inner)
            : base(message, inner)
        {
        }

        public NoSuchLanguageException(
            string message, 
            TranslateLanguages failedLanguage)

            : base(message)
        {
            FailedLanguage = failedLanguage;
        }

        public NoSuchLanguageException(
            string message, 
            Exception inner, 
            TranslateLanguages failedLanguage)

            : base(message, inner)
        {
            FailedLanguage = failedLanguage;
        }
    }
}
