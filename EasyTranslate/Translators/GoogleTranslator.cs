using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using EasyTranslate.Exceptions;
using EasyTranslate.Extentions;
using EasyTranslate.TranslationData;
using Newtonsoft.Json.Linq;

namespace EasyTranslate.Translators
{
    public class GoogleTranslator : ITranslator
    {
        private CancellationToken _cancellationToken;
        
        public async Task<TranslationSequence> TranslateAsync(
            TranslationSequence sequence,
            TranslateLanguages targetLanguage,
            CancellationToken token = default(CancellationToken))
        {
            try
            {
                _cancellationToken = token;

                string url = await new UrlBuilder().GetUrl(sequence, targetLanguage);
                string response = await GetResponseStringAsync(url);

                JToken json = TranslationInfoParser.ExtractJson(response);
                string resultWord = TranslationInfoParser.ExtractWord(json);

                (IEnumerable<ExtraTranslation> suggestions, IEnumerable<string> description) = (null, null); 
                try
                {
                    (suggestions, description) = GetSuggestions(json, resultWord);
                }
                catch
                {
                    // ignored
                }

                var result = new TranslationSequence(resultWord, targetLanguage, description, suggestions);
                return result;
            }
            catch (Exception e)
            {
                throw new TranslationFailedException(e);
            }
        }

        private static (IEnumerable<ExtraTranslation> suggestions, IEnumerable<string> description) GetSuggestions(JToken json, string resultWord)
        {
            IEnumerable<ExtraTranslation> suggestions = TranslationInfoParser.ExtractSuggestions(json);

            ExtraTranslation descriptionWord = suggestions
                .FirstOrDefault(desc => desc.Name == resultWord);
            IEnumerable<string> description = descriptionWord?.Words;
            if (descriptionWord != null)
            {
                suggestions = suggestions.Skip(1);
            }
            return (suggestions, description);
        }

        public async Task<TranslationSequence> DetectAsync(
            TranslationSequence sequence,
            CancellationToken token = default(CancellationToken))
        {
            try
            {
                _cancellationToken = token;

                string url = await new UrlBuilder().GetUrl(sequence, TranslateLanguages.English);
                string response = await GetResponseStringAsync(url);

                JToken json = TranslationInfoParser.ExtractJson(response);
                TranslateLanguages language = TranslationInfoParser.ExtractLanguage(json);

                var result = new TranslationSequence(sequence.Sequence, language);
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
            Stream responseStream = response.GetResponseStream();

            var reader = new StreamReader(responseStream);
            string result = await reader.ReadToEndAsync();

            _cancellationToken.ThrowIfCancellationRequested();

            return result;
        }
    }
}