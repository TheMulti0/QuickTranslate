using System;

namespace EasyTranslate.Exceptions
{
    public class NoInternetConnectionAvaliableExcpetion : Exception
    {
        public NoInternetConnectionAvaliableExcpetion()
        {
        }

        public NoInternetConnectionAvaliableExcpetion(string message)
            : base(message)
        {
        }

        public NoInternetConnectionAvaliableExcpetion(
            string message,
            Exception inner)
            : base(message, inner)
        {
        }
    }
}