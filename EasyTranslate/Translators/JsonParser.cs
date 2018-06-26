using System;
using System.Collections.Generic;
using System.Linq;
using EasyTranslate.Enums;
using EasyTranslate.Words;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EasyTranslate.Translators
{
    public static class JsonParser
    {
        public static JToken ExtractJson(string jsonString, ref bool isTranscriptionAvaliable)
        {
            var json = JsonConvert.DeserializeObject<JToken>(jsonString);
            isTranscriptionAvaliable = json[0].Count() > 1;

            return json;
        }

        public static string ExtractWord(JToken json, bool isTranscriptionAvaliable)
        {
            var result = "";
            JToken translationInfo = json[0];

            string[] translate = new string[
                json.Count() - (isTranscriptionAvaliable ? 1 : 0)];

            for (var i = 0; i < translate.Length; i++)
            {
                result = FindWord(translationInfo, i);
            }

            return result;
        }

        private static string FindWord(JToken translationInfo, int i)
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
            return result;
        }

        public static TranslateWord[] ExtractSuggestions(JToken json)
        {
            JToken suggestions = json[1]?[0]?[2];
            Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();

            foreach (JToken suggestion in suggestions)
            {
                if (!suggestion.HasValues)
                {
                    continue;
                }

                FindWordProperties(suggestion, dictionary);
            }

            List<TranslateWord> words = ConvertDictionaryToTranslateWord(dictionary);
            return words.ToArray();
        }

        private static List<TranslateWord> ConvertDictionaryToTranslateWord(Dictionary<string, List<string>> dictionary)
        {
            List<TranslateWord> words = new List<TranslateWord>();
            foreach (KeyValuePair<string, List<string>> pair in dictionary)
            {
                var word = new TranslateWord(pair.Key,
                                             description: pair.Value.ToArray());
                words.Add(word);
            }

            return words;
        }

        private static void FindWordProperties(JToken suggestion, Dictionary<string, List<string>> dictionary)
        {
            string key = suggestion[0]
                .ToString();

            List<string> list =
                (
                from item in suggestion[1]
                where item != null && !string.IsNullOrWhiteSpace(item.ToString())
                select item.ToString()
                ).ToList();

            dictionary.Add(key, list);
        }

        public static TranslateLanguages ExtractLanguage(JToken json)
        {
            var result = (string) json[2];

            var language = (TranslateLanguages) result.GetEnum(typeof(TranslateLanguages));
            return language;
        }
    }
}