using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using EasyTranslate.Extentions;
using EasyTranslate.Words;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EasyTranslate.Translators
{
    internal class JsonParser
    {
        private readonly CancellationToken _cancellationToken;

        public JsonParser(CancellationToken token)
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
            bool isTranscriptionAvaliable = translationInfo.Count() > 1;
            string[] translate = new string[translationInfo.Count() - (isTranscriptionAvaliable ? 1 : 0)];

            for (var i = 0; i < translate.Length; i++)
            {
                try
                {
                    var info = (string)translationInfo[i][0];
                    translate[i] = info;
                }
                catch
                {
                    // ignored
                }
            }

            string result = translate[0] ?? translate.FirstOrDefault(s => !string.IsNullOrEmpty(s));
            if (!isTranscriptionAvaliable)
            {
                return result;
            }

            GetTranscription(translationInfo, out result);

            _cancellationToken.ThrowIfCancellationRequested();

            return result;
        }

        private static void GetTranscription(JToken translationInfo, out string translatedWord)
        {
            JToken transcriptionInfo = translationInfo[translationInfo.Count() - 1];
            int transcriptionCount = transcriptionInfo.Count();

            JToken last = transcriptionInfo[transcriptionCount - 1];
            if (transcriptionCount == 3)
            {
                translatedWord = (string) last;
            }
            else
            {
                JToken secondLast = transcriptionInfo[transcriptionCount - 2];
                if (secondLast != null)
                {
                    translatedWord = (string) secondLast;
                }
                else
                {
                    translatedWord = (string) last;
                }
            }
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
                return ConvertDictionaryToTranslateWord(dictionary);
            }

            if (suggestions == null)
            {
                return ConvertDictionaryToTranslateWord(dictionary);
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

            return ConvertDictionaryToTranslateWord(dictionary);
        }

        private IEnumerable<TranslateSequence> ConvertDictionaryToTranslateWord(Dictionary<string, List<string>> dictionary)
        {
            List<TranslateSequence> words = new List<TranslateSequence>();
            foreach (KeyValuePair<string, List<string>> pair in dictionary)
            {
                var word = new TranslateSequence(pair.Key, description: pair.Value.ToArray());
                words.Add(word);
            }
            _cancellationToken.ThrowIfCancellationRequested();
            return words;
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