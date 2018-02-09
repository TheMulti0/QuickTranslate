using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            
            //var r = w.GetResponse();
            Console.WriteLine("Hello World!");
            GetValue();
            Console.ReadLine();
        }

        private static void GetValue()
        {
            //var builder = new UriBuilder("https://translate.google.com/translate_a/single?");
            //NameValueCollection query = HttpUtility.ParseQueryString("");

            //query["client"] = "t";

            //query["sl"] = "auto";

            //query["tl"] = "fr";

            //query["hl"] = "fr";

            //query["ie"] = "UTF-8";

            //query["oe"] = "UTF-8";

            //query["otf"] = "1";

            //query["ssel"] = "0";

            //query["tsel"] = "0";

            //query["kc"] = "7";

            //query["tk"] = "914062.761254";

            //query["q"] = "hi";

            //var queryString = query.ToString();
            //var finalQuery = queryString.Insert(queryString.Length,
            //    "&dt=['at', 'bd', 'ex', 'ld', 'md', 'qca', 'rw', 'rm', 'ss', 't']");
            //builder.Query = finalQuery;
            //https://translate.google.com/translate_a/single?client=t&sl=auto&tl=en&hl=en&dt=['at', 'bd', 'ex', 'ld', 'md', 'qca', 'rw', 'rm', 'ss', 't']&ie=UTF-8&oe=UTF-8&otf=1&ssel=0&tsel=0&kc=7&q=bonjour


            WebRequest request = WebRequest.CreateHttp("https://translate.google.com/translate_a/single?client=t&sl=auto&tl=iw&hl=en&dt=at&dt=bd&dt=ex&dt=ld&dt=md&dt=qca&dt=rw&dt=rm&dt=ss&dt=t&ie=UTF-8&oe=UTF-8&otf=1&ssel=0&tsel=0&kc=1&tk=684590.793873&q=d");
            WebResponse response = request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            var reader = new StreamReader(responseStream);
            var result = reader.ReadToEnd();
            JToken tmp = JsonConvert.DeserializeObject<JToken>(result);
            var a = tmp[0];
            string Eyyy = "";
            var i = a[a.Count() - 1];
            var count = i.Count();
            if (count == 3)
            {
                Eyyy = (string) i[count - 1];
            }
            else
            {
                if (i[count - 2] == null)
                {
                    Eyyy = i[count - 2].ToString();
                }
                else
                {
                    Eyyy = (string) i[count - 1];
                }
            }
            

            Console.ReadLine();
        }
    }
}
