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

        public JToken ExtractJson(string jsonString, out bool isTranscriptionAvaliable)
        {
            var json = JsonConvert.DeserializeObject<JToken>(jsonString);
            isTranscriptionAvaliable = json[0].Count() > 1;

            _cancellationToken.ThrowIfCancellationRequested();
            return json;
        }

        public string ExtractWord(JToken json, bool isTranscriptionAvaliable)
        {
            var result = "";
            JToken translationInfo = json[0];

            string[] translate = new string[                json.Count() - (isTranscriptionAvaliable ? 1 : 0)];

            for (var i = 0; i < translate.Length; i++)
            {
                result = FindWord(translationInfo, i);
            }

            _cancellationToken.ThrowIfCancellationRequested();
            return result;
        }

        private string FindWord(JToken translationInfo, int i)
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

            var result = (string) wordJToken;
            _cancellationToken.ThrowIfCancellationRequested();
            return result;
        }

        public IEnumerable<TranslateSequence> ExtractSuggestions(JToken json)
        {
            JToken suggestions = json[1]?[0]?[2];
            Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();

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

        public TranslateLanguages ExtractLanguage(JToken json)
        {
            var result = (string) json[2];

            var language = (TranslateLanguages) result.GetEnumByDescription(typeof(TranslateLanguages));
            _cancellationToken.ThrowIfCancellationRequested();
            return language;
        }
    }
}