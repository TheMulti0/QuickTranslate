using System;

namespace EasyTranslate.Exceptions
{
    public class TranslationFailedException : Exception
    {
        public TranslationFailedException(string message = "Translate Operation Failed.")
            : base(message)
        {
        }

        public TranslationFailedException(
            Exception inner, 
            string message = "Translate Operation Failed.")
            : base(message, inner)
        {
        }
    }
}
