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

            JToken json = ExtractJson(response, ref isTranscriptionAvaliable);

            string resultWord = ExtractWord(json, isTranscriptionAvaliable);

            var result = new TranslateWord(resultWord, targetLanguage);

            return result;
        }

        public TranslateWord Detect(TranslateWord word, TranslateLanguages randomLanguage = TranslateLanguages.French)
        {
            Initialize(word);

            string url = GetUrl(word, randomLanguage);

            string response = GetResponseString(url);

            var isTranscriptionAvaliable = false;

            JToken json = ExtractJson(response, ref isTranscriptionAvaliable);

            TranslateLanguages language = ExtractLanguage(json);

            var result = new TranslateWord(word.Word, language);

            return result;
        }

        private string GetUrl(TranslateWord word, TranslateLanguages lang)
        {
            var builder = BuildUri(word);

            var queryString = builder.Query.ToString(); 
            var finalQuery = queryString.Insert(queryString.Length,
                "&dt=['at', 'bd', 'ex', 'ld', 'md', 'qca', 'rw', 'rm', 'ss', 't']");

            builder.Query = finalQuery;

            var langValue = lang.GetDescriptionAttributeString();

            string modifiedUrl = builder.Uri
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
            var query = HttpUtility.ParseQueryString("");


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

            var response = request.GetResponse();

            var responseStream = response.GetResponseStream();

            var reader = new StreamReader(responseStream);
            var result = reader.ReadToEnd();

            return result;
        }

        private JToken ExtractJson(string jsonString, ref bool isTranscriptionAvaliable)
        {
            var json = JsonConvert.DeserializeObject<JToken>(jsonString);

            isTranscriptionAvaliable = json[0].Count() > 1;

            return json;
        }

        private string ExtractWord(JToken json, bool isTranscriptionAvaliable)
        {
            var result = "";
            JToken translationInfo = json[0];

            var translate = new string[
                json.Count() - (isTranscriptionAvaliable ? 1 : 0)];

            for (var i = 0; i < translate.Length; i++)
            {
                JToken wordJToken;
                try
                {
                    wordJToken = translationInfo[i];
                    if (wordJToken.HasValues)
                    {
                        wordJToken = translationInfo[i][0];
                    }
                }
                catch (Exception)
                {
                    wordJToken = translationInfo[0][0];
                }

                result = (string)wordJToken;
            }

            return result;
        }
        private TranslateLanguages ExtractLanguage(JToken json)
        {
            var result = "";

            result = (string) json[2];

            TranslateLanguages language = _map.Find(result).Value;

            return language;
        }
    }
}