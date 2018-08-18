using System.Collections.Generic;
using System.Linq;
using System.Threading;
using EasyTranslate.Exceptions;
using EasyTranslate.Extentions;
using EasyTranslate.TranslationData;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EasyTranslate.Translators
{
    internal class TranslationInfoParser
    {
        private readonly CancellationToken _cancellationToken;

        public TranslationInfoParser(CancellationToken token)
        {
            _cancellationToken = token;
        }

        public JToken ExtractJson(string jsonString)
        {
            var json = JsonConvert.DeserializeObject<JToken>(jsonString);

            _cancellationToken.ThrowIfCancellationRequested();
            return json;
        }

        public string ExtractWord(JToken json)
        {
            JToken translationInfo = json[0];
            int infoCount = translationInfo.Count();
            bool isTranscriptionAvaliable = infoCount > 1;
            string[] translate = new string[infoCount - (isTranscriptionAvaliable ? 1 : 0)];

            for (var i = 0; i < translate.Length; i++)
            {
                var info = (string) translationInfo[i][0];
                translate[i] = info;
            }

            string result = translate.First() ?? translate.FirstOrDefault(s => !string.IsNullOrEmpty(s));
            if (!isTranscriptionAvaliable)
            {
                return result;
            }
            var transcription = (string) GetTranscription(translationInfo);
            return result.EndsWith(" ")
                ? result + transcription
                : result + " " + transcription;
        }

        private static JToken GetTranscription(JToken translationInfo)
        {
            if (!translationInfo.Any())
            {
                throw new TranslationFailedException();
            }
            JToken transcriptionInfo = translationInfo.Last();
            int transcriptionCount = transcriptionInfo.Count();
            if (transcriptionCount < 3)
            {
                throw new TranslationFailedException();
            }

            JToken last = transcriptionInfo[transcriptionCount - 1];
            JToken secondLast = transcriptionInfo[transcriptionCount - 2];

            if (last != null &&
                (secondLast == null || transcriptionCount == 3))
            {
                return last;
            }

            return transcriptionInfo.FirstOrDefault(token => token != null);
        }

        public TranslateLanguages ExtractLanguage(JToken json)
        {
            var result = (string) json[2];

            var language = (TranslateLanguages) result.GetEnumByDescription(typeof(TranslateLanguages));
            _cancellationToken.ThrowIfCancellationRequested();
            return language;
        }

        public IEnumerable<TranslateSequence> ExtractSuggestions(JToken json)
        {
            JToken suggestions;
            Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
            try
            {
                suggestions = json[1]?[0]?[2] ?? json[1]?[0];
            }
            catch
            {
                return dictionary.ToTranslateSequence();
            }

            if (suggestions == null)
            {
                return dictionary.ToTranslateSequence();
            }
            foreach (JToken suggestion in suggestions)
            {
                if (!suggestion.HasValues)
                {
                    continue;
                }

                FindWordProperties(suggestion, dictionary);
            }
            _cancellationToken.ThrowIfCancellationRequested();

            return dictionary.ToTranslateSequence();
        }

        private void FindWordProperties(JToken suggestion, IDictionary<string, List<string>> dictionary)
        {
            string key = suggestion[0].ToString();

            List<string> list =
                (
                    from item in suggestion[1]
                    where !string.IsNullOrWhiteSpace(item?.ToString())
                    select item.ToString()
                    ).ToList();

            _cancellationToken.ThrowIfCancellationRequested();
            dictionary.Add(key, list);
        }

        //public string[] ExtractSeeAlso(JToken json)
        //    => !json.HasValues ? new string[0] : 
    }
}