using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using EasyTranslate.Exceptions;
using EasyTranslate.Extentions;
using EasyTranslate.Words;
using Newtonsoft.Json.Linq;

namespace EasyTranslate.Translators
{
    public class GoogleTranslator : ITranslator
    {
        private CancellationToken _cancellationToken;

        public TranslateSequence Translate(TranslateSequence sequence, TranslateLanguages targetLanguage)
            => TranslateAsync(sequence, targetLanguage).Result;

        public async Task<TranslateSequence> TranslateAsync(
            TranslateSequence sequence,
            TranslateLanguages targetLanguage,
            CancellationToken token = default(CancellationToken))
        {
            try
            {
                _cancellationToken = token;

                string url = await new UrlBuilder().GetUrl(sequence, targetLanguage);
                string response = await GetResponseStringAsync(url);

                var parser = new JsonParser(_cancellationToken);
                JToken json = parser.ExtractJson(response, out bool isTranscriptionAvaliable);
                string resultWord = parser.ExtractWord(json, isTranscriptionAvaliable);

                IEnumerable<TranslateSequence> suggestions = parser.ExtractSuggestions(json);
                TranslateSequence[] suggestionsArray = suggestions.ToArray();
                string[] description = suggestionsArray
                    .FirstOrDefault(w => w.Word == resultWord)
                    ?.Description;

                var result = new TranslateSequence(resultWord, targetLanguage, suggestionsArray.Skip(1), description);
                return result;
            }
            catch (Exception e)
            {
                throw new TranslationFailedException(e);
            }
        }

        public TranslateSequence Detect(TranslateSequence sequence)
            => DetectAsync(sequence).Result;

        public async Task<TranslateSequence> DetectAsync(
            TranslateSequence sequence,
            CancellationToken token = default(CancellationToken))
        {
            try
            {
                _cancellationToken = token;

                string url = await new UrlBuilder().GetUrl(sequence, TranslateLanguages.English);
                string response = await GetResponseStringAsync(url);

                var parser = new JsonParser(_cancellationToken);
                JToken json = parser.ExtractJson(response, out bool _);
                TranslateLanguages language = parser.ExtractLanguage(json);

                var result = new TranslateSequence(sequence.Word, language);
                return result;
            }
            catch (Exception e)
            {
                throw new DetectionFailedException(e);
            }
        }

        private async Task<string> GetResponseStringAsync(string url)
        {
            WebRequest request = WebRequest.CreateHttp(url);

            _cancellationToken.ThrowIfCancellationRequested();

            WebResponse response = await request.GetResponseAsync(_cancellationToken);
            //WebResponse response = request.GetResponse();
            Stream responseStream = response.GetResponseStream();

            var reader = new StreamReader(responseStream);
            string result = await reader.ReadToEndAsync();

            _cancellationToken.ThrowIfCancellationRequested();

            return result;
        }
    }
}