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

        /// <summary>
        /// Translation of a sequence from language to language
        /// </summary>
        /// <param name="sequence">The desired sequence to translate</param>
        /// <param name="targetLanguage">The language desired to translate the sequence to</param>
        /// <exception cref="TranslationFailedException">Thrown if the translation operation fails</exception>
        /// <returns>A new sequence containing the translated sequence, its language and additional information</returns>
        /// <seealso cref="TranslateAsync"/>
        public TranslateSequence Translate(TranslateSequence sequence, TranslateLanguages targetLanguage)
            => TranslateAsync(sequence, targetLanguage).Result;

        /// <summary>
        /// Asynchronous translation of a sequence from language to language
        /// </summary>
        /// <param name="sequence">The desired sequence to translate</param>
        /// <param name="targetLanguage">The language desired to translate the sequence to</param>
        /// <param name="token">An optional cancellation token to cancel the operation</param>
        /// <exception cref="TranslationFailedException">Thrown if the translation operation fails</exception>
        /// <returns>A new sequence containing the translated sequence, its language and additional information</returns>
        /// <seealso cref="Translate"/>
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

                var parser = new TranslationInfoParser(_cancellationToken);
                JToken json = parser.ExtractJson(response);
                string resultWord = parser.ExtractWord(json);

                IEnumerable<TranslateSequence> suggestions = parser.ExtractSuggestions(json);
                TranslateSequence[] suggestionsArray = suggestions.ToArray();

                TranslateSequence descriptionWord = suggestionsArray
                    .FirstOrDefault(w => w.Word == resultWord) ?? suggestionsArray.FirstOrDefault();
                string[] description = descriptionWord?.Description;

                var result = new TranslateSequence(resultWord, targetLanguage/*, suggestionsArray.Skip(1), description*/);
                return result;
            }
            catch (Exception e)
            {
                throw new TranslationFailedException(e);
            }
        }

        /// <summary>
        /// Sequence language detection
        /// </summary>
        /// <param name="sequence">The desired sequence to detect its language</param>
        /// <exception cref="DetectionFailedException">Thrown if the detection operation fails</exception>
        /// <returns>A new sequence containing the original word, and the detected language</returns>
        /// <seealso cref="DetectAsync"/>
        public TranslateSequence Detect(TranslateSequence sequence)
            => DetectAsync(sequence).Result;

        /// <summary>
        /// Asynchronous sequence language detection
        /// </summary>
        /// <param name="sequence">The desired sequence to detect its language</param>
        /// <param name="token">An optional cancellation token to cancel the operation</param>
        /// <exception cref="DetectionFailedException">Thrown if the detection operation fails</exception>
        /// <returns>A new sequence containing the original word, and the detected language</returns>
        /// <seealso cref="Detect"/>
        public async Task<TranslateSequence> DetectAsync(
            TranslateSequence sequence,
            CancellationToken token = default(CancellationToken))
        {
            try
            {
                _cancellationToken = token;

                string url = await new UrlBuilder().GetUrl(sequence, TranslateLanguages.English);
                string response = await GetResponseStringAsync(url);

                var parser = new TranslationInfoParser(_cancellationToken);
                JToken json = parser.ExtractJson(response);
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
            Stream responseStream = response.GetResponseStream();

            var reader = new StreamReader(responseStream);
            string result = await reader.ReadToEndAsync();

            _cancellationToken.ThrowIfCancellationRequested();

            return result;
        }
    }
}