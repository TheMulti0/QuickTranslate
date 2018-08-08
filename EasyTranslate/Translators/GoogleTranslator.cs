using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using EasyTranslate.Checkers;
using EasyTranslate.Extentions;
using EasyTranslate.Words;
using Newtonsoft.Json.Linq;

namespace EasyTranslate.Translators
{
    public class GoogleTranslator : ITranslator
    {
        private string _token;
        private CancellationToken _cancellationToken;
        private JsonParser _parser;

        public async Task<TranslateWord> TranslateAsync(
            TranslateWord word,
            TranslateLanguages targetLanguage,
            CancellationToken token = default(CancellationToken))
        {
            _cancellationToken = token;
            _parser = new JsonParser();

            Initialize(word);

            string url = GetUrl(word, targetLanguage);
            string response = await GetResponseStringAsync(url);
            var isTranscriptionAvaliable = false;

            JToken json = _parser.ExtractJson(response, ref isTranscriptionAvaliable);
            string resultWord = _parser.ExtractWord(json, isTranscriptionAvaliable);

            TranslateWord[] suggestions = _parser.ExtractSuggestions(json);
            string[] description = suggestions.FirstOrDefault(w => w.Word == resultWord)
                                              ?.Description;
            var result = new TranslateWord(resultWord, targetLanguage, suggestions, description);

            return result;
        }

        public async Task<TranslateWord> DetectAsync(
            TranslateWord word,
            TranslateLanguages randomLanguage = TranslateLanguages.French,
            CancellationToken token = default(CancellationToken))
        {
            _cancellationToken = token;
            _parser = new JsonParser();

            Initialize(word);

            string url = GetUrl(word, randomLanguage);

            string response = await GetResponseStringAsync(url);

            var isTranscriptionAvaliable = false;

            JToken json = _parser.ExtractJson(response, ref isTranscriptionAvaliable);

            TranslateLanguages language = _parser.ExtractLanguage(json);

            var result = new TranslateWord(word.Word, language);

            return result;
        }

        private void CancelIfRequested(CancellationToken token = default(CancellationToken))
        {
            if (token == default(CancellationToken))
            {
                if (_cancellationToken == default(CancellationToken))
                {
                    return;
                }
                token = _cancellationToken;
            }
            if (token.IsCancellationRequested)
            {
                token.ThrowIfCancellationRequested();
            }
        }

        private async void Initialize(TranslateWord word)
        {
            var tkkGenerator = new TkkGenerator();
            string tkk = await tkkGenerator.GetTkkAsync();

            CancelIfRequested();
            _token = new TokenGenerator().GetToken(word.Word, tkk);

            CancelIfRequested();
            InternetChecker.CheckForInternetConnection();

            CancelIfRequested();
        }

        private string GetUrl(TranslateWord word, TranslateLanguages lang)
        {
            UriBuilder builder = BuildUri(word);

            string queryString = builder.Query;
            string finalQuery = queryString.Insert(
                queryString.Length,
                "&dt=['at', 'bd', 'ex', 'ld', 'md', 'qca', 'rw', 'rm', 'ss', 't']");
            builder.Query = finalQuery;

            string langValue = lang.GetDescriptionAttributeString();

            string modifiedUrl = builder.Uri.ToString()
                                        .Replace(
                                            "tl=lang" + "&hl=lang" + "&dt=dtparameter",
                                            $"tl={langValue}" + $"&hl={langValue}" + "&dt=dtparameter")
                                        .Replace(
                                            "dtparameter" + "&ie=UTF-8",
                                            "at&dt=bd&dt=ex&dt=ld&dt=md&dt=qca&dt=rw&dt=rm&dt=ss&dt=t" + "&ie=UTF-8")
                                        .Replace("single??", "single?");

            CancelIfRequested();
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
            query["ie"] = query["oe"] = "UTF-8";
            query["otf"] = "2";
            query["ssel"] = "0";
            query["tsel"] = "4";
            query["kc"] = "20";
            query["tk"] = _token;
            query["q"] = word.Word;

            builder.Query = query.ToString();

            CancelIfRequested();
            return builder;
        }

        private async Task<string> GetResponseStringAsync(string url)
        {
            WebRequest request = WebRequest.CreateHttp(url);

            CancelIfRequested();
            //WebResponse response = await request.GetResponseAsync(_cancellationToken);
            WebResponse response = request.GetResponse();
            Stream responseStream = response.GetResponseStream();

            CancelIfRequested();
            var reader = new StreamReader(responseStream);
            string result = await reader.ReadToEndAsync();

            CancelIfRequested();
            return result;
        }
    }
}