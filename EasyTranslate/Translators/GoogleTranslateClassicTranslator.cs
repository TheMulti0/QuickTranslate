using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using EasyTranslate.Checkers;
using EasyTranslate.Enums;
using EasyTranslate.Words;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EasyTranslate.Translators
{
    public class GoogleTranslateClassicTranslator : ITranslator
    {
        private string _token;

        public TranslateWord Translate(TranslateWord word, TranslateLanguages targetLanguage)
        {
            Initialize(word);

            string url = GetUrl(word, targetLanguage);

            string response = GetResponseString(url);

            var isTranscriptionAvaliable = false;

            JToken json = JsonParser.ExtractJson(response, ref isTranscriptionAvaliable);

            string resultWord = JsonParser.ExtractWord(json, isTranscriptionAvaliable);

            TranslateWord[] suggestions = JsonParser.ExtractSuggestions(json);
            string[] description = suggestions.FirstOrDefault(w => w.Word == resultWord)?.Description;
            var result = new TranslateWord(
                resultWord,
                targetLanguage, 
                suggestions,
                description);

            return result;
        }

        public TranslateWord Detect(TranslateWord word, TranslateLanguages randomLanguage = TranslateLanguages.French)
        {
            Initialize(word);

            string url = GetUrl(word, randomLanguage);

            string response = GetResponseString(url);

            var isTranscriptionAvaliable = false;

            JToken json = JsonParser.ExtractJson(response, ref isTranscriptionAvaliable);

            TranslateLanguages language = JsonParser.ExtractLanguage(json);

            var result = new TranslateWord(word.Word, language);

            return result;
        }

        private void Initialize(TranslateWord word)
        {
            var tkk = new TkkGenerator().GetTKK();
            _token = new TokenGenerator().GetToken(word.Word, tkk);

            InternetChecker.CheckForInternetConnection();
        }

        private string GetUrl(TranslateWord word, TranslateLanguages lang)
        {
            UriBuilder builder = BuildUri(word);

            var queryString = builder.Query;
            var finalQuery = queryString.Insert(queryString.Length,
                                                "&dt=['at', 'bd', 'ex', 'ld', 'md', 'qca', 'rw', 'rm', 'ss', 't']");
            builder.Query = finalQuery;

            string langValue = lang.GetDescriptionAttributeString();

            string modifiedUrl = builder.Uri
                .ToString()
                .Replace("tl=lang" + "&hl=lang" + "&dt=dtparameter",
                        $"tl={langValue}" + $"&hl={langValue}" + "&dt=dtparameter")
                .Replace("dtparameter" + "&ie=UTF-8",
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
    }
}