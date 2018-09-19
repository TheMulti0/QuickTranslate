using System;

namespace EasyTranslate.Exceptions
{
    public class TokenGenerationException : Exception
    {
        public TokenGenerationException(string message = "Translation token generation failed.")
            : base(message)
        {
        }

        public TokenGenerationException(
            Exception inner,
            string message = "Translation token generation failed.")
            : base("", inner)
        {
        }
    }
}