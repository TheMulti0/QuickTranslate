using System;

namespace EasyTranslate.Exceptions
{
    public class NoInternetConnectionAvaliableException : Exception
    {
        public NoInternetConnectionAvaliableException()
        {
        }

        public NoInternetConnectionAvaliableException(string message)
            : base(message)
        {
        }

        public NoInternetConnectionAvaliableException(
            string message,
            Exception inner)
            : base(message, inner)
        {
        }
    }
}