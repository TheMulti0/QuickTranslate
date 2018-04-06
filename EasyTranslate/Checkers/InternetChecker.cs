using System.Net;
using EasyTranslate.Exceptions;

namespace EasyTranslate.Checkers
{
    internal static class InternetChecker
    {
        public static void CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                {
                    var url = "http://google.com";
                    using (client.OpenRead(url))
                    {
                    }
                }
            }
            catch
            {
                throw new NoInternetConnectionAvaliableException();
            }
        }
    }
}