using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using EasyTranslate.Enums;
using EasyTranslate.Translators;
using EasyTranslate.Words;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {

            ////var r = w.GetResponse();
            //Console.WriteLine("Hello World!");
            //GetValue();
            //Console.ReadLine();

            //TranslateWord a = new GoogleTranslateClassicTranslator().Translate(new TranslateWord("hello"), TranslateLanguages.French);
            TranslateWord a = new GoogleTranslateClassicTranslator().Detect(new TranslateWord("שלום"));
            var b = a.Word;
        }

        private static void GetValue()
        {
            var builder = new UriBuilder("https://translate.google.com/translate_a/single");
            NameValueCollection query = HttpUtility.ParseQueryString("");
            string word = "hi";
            string token = new TokenGenerator().GetToken(word, new TkkGenerator().GetTKK());

            query["client"] = "t";

            query["sl"] = "auto";

            query["tl"] = "fr";

            query["hl"] = "fr";

            query["dt"] = "dtparameter";

            query["ie"] = "UTF-8";

            query["oe"] = "UTF-8";

            query["otf"] = "2";

            query["ssel"] = "0";

            query["tsel"] = "4";

            query["kc"] = "20";

            query["tk"] = token;

            query["q"] = word;

            var queryString = query.ToString();
            var finalQuery = queryString.Insert(queryString.Length,
                "&dt=['at', 'bd', 'ex', 'ld', 'md', 'qca', 'rw', 'rm', 'ss', 't']");
            builder.Query = finalQuery;
            //https://translate.google.com/translate_a/single?client=t&sl=auto&tl=en&hl=en&dt=['at', 'bd', 'ex', 'ld', 'md', 'qca', 'rw', 'rm', 'ss', 't']&ie=UTF-8&oe=UTF-8&otf=1&ssel=0&tsel=0&kc=7&q=bonjour

            string modifiedUrl = builder.Uri.ToString()
                .Replace("dtparameter", "at&dt=bd&dt=ex&dt=ld&dt=md&dt=qca&dt=rw&dt=rm&dt=ss&dt=t");
            //var requestUriString = "https://translate.google.com/translate_a/single?client=t&sl=auto&tl=fr&hl=fr&dt=at&dt=bd&dt=ex&dt=ld&dt=md&dt=qca&dt=rw&dt=rm&dt=ss&dt=t&ie=UTF-8&oe=UTF-8&ssel=0&tsel=4&kc=0&tk=token&q=word";
            //requestUriString.Replace("token", token);
            //requestUriString.Replace("word", word);
            WebRequest request = WebRequest.Create(modifiedUrl);
            WebResponse response = request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            var reader = new StreamReader(responseStream);
            string result = reader.ReadToEnd();





            string finalResult = "";
            JToken translationInfo = JsonConvert.DeserializeObject<JToken>(result)[0];

            bool transcriptionAvaliable = translationInfo.Count() > 1;
            string[] translate = new string[translationInfo.Count() - (transcriptionAvaliable ? 1 : 0)];

            for (int i = 0; i < translate.Length; i++)
            {
                translate[i] = (string)translationInfo[i][0];
            }

            //var mainInfo = translationInfo[translationInfo.Count() - 1];
            //var count = mainInfo.Count();
            //if (count == 3)
            //{
            //    finalResult = (string)mainInfo[count - 1];
            //}
            //else
            //{
            //    if (mainInfo[count - 2] != null)
            //    {
            //        finalResult = mainInfo[count - 2].ToString();
            //    }
            //    else
            //    {
            //        finalResult = (string)mainInfo[count - 1];
            //    }
            //}


            Console.ReadLine();
        }
    }
}
