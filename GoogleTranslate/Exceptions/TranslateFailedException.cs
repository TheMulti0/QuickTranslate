using System;

namespace GoogleTranslate.Exceptions
{
    public class TranslateFailedException : Exception
    {
        public TranslateFailedException()
        {
        }

        public TranslateFailedException(string message)
            : base(message)
        {
        }

        public TranslateFailedException(
            string message, 
            Exception inner)
            : base(message, inner)
        {
        }
    }
}
