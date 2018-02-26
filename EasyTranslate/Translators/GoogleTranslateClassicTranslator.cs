using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using EasyTranslate.Enums;
using EasyTranslate.Words;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EasyTranslate.Translators
{
    public class GoogleTranslateClassicTranslator : ITranslator
    {
        private string _token;

        private void Initialize(TranslateWord word)
        {
            string tkk = new TkkGenerator().GetTKK();
            _token = new TokenGenerator().GetToken(word.Word, tkk);
        }

        public TranslateWord Translate(TranslateWord word, TranslateLanguages targetLanguage)
        {
            Initialize(word);

            string url = GetUrl(word, targetLanguage);

            string response = GetResponseString(url);

            string resultWorld = ExtractWord(response);
            var result = new TranslateWord(resultWorld, targetLanguage);

            return result;
        }

        public TranslateWord Detect(TranslateWord word)
        {
            throw new NotImplementedException();
        }

        private string GetUrl(TranslateWord word, TranslateLanguages lang)
        {
            UriBuilder builder = BuildUri(word);

            var queryString = builder.Query.ToString(); 
            var finalQuery = queryString.Insert(queryString.Length,
                "&dt=['at', 'bd', 'ex', 'ld', 'md', 'qca', 'rw', 'rm', 'ss', 't']");

            builder.Query = finalQuery;

            string langValue = new LanguageMap().Find(lang).Key;

            var modifiedUrl = builder.Uri
                .ToString()
                .Replace(
                    "tl=lang" + "&hl=lang" + "&dt=dtparameter",
                    $"tl={langValue}" + $"&hl={langValue}" + "&dt=dtparameter")
                .Replace(
                    "dtparameter" + "&ie=UTF-8",
                    "at&dt=bd&dt=ex&dt=ld&dt=md&dt=qca&dt=rw&dt=rm&dt=ss&dt=t" + "&ie=UTF-8")
                .Replace("single??", "single?");
                

            return modifiedUrl;
        }

        private UriBuilder BuildUri(TranslateWord word)
        {
            var builder = new UriBuilder("https://translate.google.com/translate_a/single");
            NameValueCollection query = HttpUtility.ParseQueryString("");


            query["client"] = "t";

            query["sl"] = "auto";

            query["tl"] = "lang";

            query["hl"] = "lang";

            query["dt"] = "dtparameter";

            query["ie"] = "UTF-8";

            query["oe"] = "UTF-8";

            query["otf"] = "2";

            query["ssel"] = "0";

            query["tsel"] = "4";

            query["kc"] = "20";

            query["tk"] = _token;

            query["q"] = word.Word;

            builder.Query = query.ToString();

            return builder;
        }

        private string GetResponseString(string url)
        {
            WebRequest request = WebRequest.CreateHttp(url);

            WebResponse response = request.GetResponse();

            Stream responseStream = response.GetResponseStream();

            var reader = new StreamReader(responseStream);
            string result = reader.ReadToEnd();

            return result;
        }

        private string ExtractWord(string xml)
        {
            string result = "";

            JToken translationInfo = JsonConvert.DeserializeObject<JToken>(xml)[0];

            bool isTranscriptionAvaliable = translationInfo.Count() > 1;

            var translate = new string[
                translationInfo.Count() - (isTranscriptionAvaliable ? 1 : 0)];

            for (int i = 0; i < translate.Length; i++)
            {
                result = (string)translationInfo[i][0];
            }

            return result;
        }

        private static void Main()
        {
            var builder = new UriBuilder("https://translate.google.com/translate_a/single");

            var languageMap = new LanguageMap();
            NameValueCollection query = HttpUtility.ParseQueryString(builder.Query);

            query["client"] = "t";

            query["sl"] = "auto";

            query["tl"] = "fr";

            query["hl"] = "fr";

            query["dt"] = "['at', 'bd', 'ex', 'ld', 'md', 'qca', 'rw', 'rm', 'ss', 't']";

            query["ie"] = "UTF-8";

            query["oe"] = "UTF-8";

            query["otf"] = "1";

            query["ssel"] = "0";

            query["tsel"] = "0";

            query["kc"] = "7";

            query["q"] = "hello";

            builder.Query = query.ToString();
            //https://translate.google.com/translate_a/single?client=t&sl=auto&tl=en&hl=en&dt=['at', 'bd', 'ex', 'ld', 'md', 'qca', 'rw', 'rm', 'ss', 't']&ie=UTF-8&oe=UTF-8&otf=1&ssel=0&tsel=0&kc=7&q=bonjour
            WebRequest W = WebRequest.Create(builder.Uri);
            WebResponse r = W.GetResponse();
        }
    }
}