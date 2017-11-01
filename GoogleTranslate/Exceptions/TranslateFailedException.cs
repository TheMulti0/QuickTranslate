using System;

namespace GoogleTranslate.Exceptions
{
    public class TranslateFailedException : Exception
    {
        public TranslateFailedException(string message = "Translate Operation Failed.")
            : base(message)
        {
        }

        public TranslateFailedException(
            Exception inner, 
            string message = "Translate Operation Failed.")
            : base(message, inner)
        {
        }
    }
}
