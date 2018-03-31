using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EasyTranslate.Translators
{
    internal class TkkGenerator
    {
        private string GetResponse()
        {
            var client = new WebClient();
            var headers = new WebHeaderCollection
            {
                {"Accept", "text/html"},
                { "Accept-Charset", "utf-8"}
            };
            
            client.Headers = headers;

            var task = new Task<string>(() =>
            {
                try
                {
                    return client.DownloadString(new Uri("https://translate.google.com"));
                }
                catch
                {
                    headers.Add("Host", "translate.google.com");

                    return client.DownloadString(new Uri("https://203.208.39.255:443"));
                }
            });

            task.Start();
            task.Wait();

            return task.Result;
        }

        public string GetTKK()
        {
            //string response = @"TKK=eval('((function(){var a\x3d2377932982;var b\x3d-1426326282;return 408506+\x27.\x27+(a+b)})())');";
            string response = GetResponse();

            if (string.IsNullOrEmpty(response))
            {
                return null;
            }

            var regex = new Regex(@"TKK.*?\)\}\)\(\)\)\'\);");
            Match match = regex.Match(response);

            string TkkString = match.Value;

            regex = new Regex(@"\\x3d-?\d*"); // 可能出现负值

            MatchCollection matches = regex.Matches(TkkString);
            long TKKFloat = 0;

            foreach (Match m in matches)
            {
                TKKFloat += Convert.ToInt64(m.Value.Remove(0, 4));
            }

            //string TKKInt = Math.Floor((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds / 3600000).toString();   // 如果电脑时间差距太多算取的时间戳则稍微不一样

            regex = new Regex(@"return\s\d*");
            string TKKInt = regex.Match(TkkString).Value.Remove(0, 7);

            return TKKInt + "." + TKKFloat;
        }
    }
}