using System.Net;

namespace EasyTranslate.Checkers
{
    internal static class InternetChecker
    {
        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                {
                    var url = "http://google.com";
                    using (client.OpenRead(url))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
    }
}