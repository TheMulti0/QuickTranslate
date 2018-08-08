using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EasyTranslate.Translators
{
    internal class TkkGenerator
    {
        public async Task<string> GetTkkAsync()
        {
            //string response = @"TKK=eval('((function(){var a\x3d2377932982;var b\x3d-1426326282;return 408506+\x27.\x27+(a+b)})())');";
            string response = await GetResponseAsync();

            if (string.IsNullOrEmpty(response))
            {
                return null;
            }

            var regex = new Regex(@"TKK.*?\)\}\)\(\)\)\'\);");
            Match match = regex.Match(response);

            string tkkString = match.Value;

            regex = new Regex(@"\\x3d-?\d*"); // 可能出现负值

            MatchCollection matches = regex.Matches(tkkString);
            long tkkFloat = 0;

            foreach (Match m in matches)
            {
                tkkFloat += Convert.ToInt64(m.Value.Remove(0, 4));
            }

            //string TKKInt = Math.Floor((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds / 3600000).toString();   // 如果电脑时间差距太多算取的时间戳则稍微不一样

            regex = new Regex(@"return\s\d*");
            string tkkInt = regex.Match(tkkString)
                                 .Value.Remove(0, 7);

            return tkkInt + "." + tkkFloat;
        }

        private static async Task<string> GetResponseAsync()
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("Accept", "text/html");
            client.DefaultRequestHeaders.Add("Accept-Charset", "utf-8");

            try
            {
                var uri = new Uri("https://translate.google.com");
                return await client.GetStringAsync(uri);
            }
            catch
            {
                client.DefaultRequestHeaders.Add("Host", "translate.google.com");

                var uri = new Uri("https://203.208.39.255:443");
                return await client.GetStringAsync(uri);
            }
        }
    }
}