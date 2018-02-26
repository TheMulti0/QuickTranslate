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
            var wc = new WebClient();

            var whc = new WebHeaderCollection();

            whc.Add("Accept", "text/html");

            whc.Add("Accept-Charset", "utf-8");

            wc.Headers = whc;


            var t = new Task<string>(() =>

            {
                try

                {
                    whc.Add("Host", "translate.google.com");

                    return wc.DownloadString(new Uri("https://203.208.39.255:443"));
                }

                catch

                {
                    return wc.DownloadString(new Uri("https://translate.google.com"));
                }
            });

            t.Start();

            try

            {
                t.Wait();
            }

            catch

            {
                throw;
            }
            return t.Result;
        }


        /// <summary>
        ///     获取TKK值
        /// </summary>
        /// <returns></returns>
        public string GetTKK()

        {
            //string response = @"TKK=eval('((function(){var a\x3d2377932982;var b\x3d-1426326282;return 408506+\x27.\x27+(a+b)})())');";

            var response = GetResponse();

            if (string.IsNullOrEmpty(response))
            {
                return null;
            }


            var reg = new Regex(@"TKK.*?\)\}\)\(\)\)\'\);"); //获取TKK字符串

            Match match = reg.Match(response);

            var TKKStr = match.Value;


            reg = new Regex(@"\\x3d-?\d*"); // 可能出现负值

            MatchCollection matches = reg.Matches(TKKStr); // 取得ab两值

            long TKKFloat = 0;

            foreach (Match m in matches)

            {
                TKKFloat += Convert.ToInt64(m.Value.Remove(0, 4)); // 计算 a+b 的值
            }


            //string TKKInt = Math.Floor((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds / 3600000).toString();   // 如果电脑时间差距太多算取的时间戳则稍微不一样

            reg = new Regex(@"return\s\d*");

            var TKKInt = reg.Match(TKKStr).Value.Remove(0, 7); // 取得时间戳处理值

            return TKKInt + "." + TKKFloat;
        }
    }
}