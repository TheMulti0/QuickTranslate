using System;
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
    public static class TranslationInfoParser
    {
        public static JToken ExtractJson(string jsonString) 
            => JsonConvert.DeserializeObject<JToken>(jsonString);

        public static string ExtractWord(JToken json)
        {
            JToken translationInfo = json[0];
            if (!translationInfo.Any())
            {
                throw new TranslationFailedException("Translation info is empty.");
            }
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
            JToken transcriptionInfo = translationInfo.Last();
            int transcriptionCount = transcriptionInfo.Count();
            if (transcriptionCount < 3)
            {
                throw new TranslationFailedException("Transcription count is smaller than 3");
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

        public static TranslateLanguages ExtractLanguage(JToken json)
        {
            var result = (string) json[2];
            return (TranslateLanguages) result.GetEnumByDescription(typeof(TranslateLanguages));
        }

        public static IEnumerable<ExtraTranslation> NewSuggestions(JToken json)
        {
            if (json[1]?.HasValues == null)
            {
                return null;
            }

            JToken suggestions = json[1][0];
            if (suggestions?.HasValues == null)
            {
                return null;
            }
            if (suggestions.Count() == 1)
            {
                return null;
            }

            var type = (string) suggestions[0];
            object enumObject = Enum.Parse(typeof(TranslationType), type.FirstCharToUpper());
            var @enum = (TranslationType) enumObject;
            List<ExtraTranslation> finalSuggestions = new List<ExtraTranslation>();
            foreach (JToken suggestion in suggestions.Skip(1))
            {
                JToken firstOrDefault = suggestion.FirstOrDefault();
                if (!suggestion.HasValues ||
                    !firstOrDefault.HasValues)
                {
                    continue;
                }
                string name = null;
                foreach (JToken s in suggestion)
                {
                    if (!s.HasValues)
                    {
                        name = (string) s;
                        continue;
                    }
                    JArray array = JArray.Parse(s.ToString());
                    IEnumerable<string> description = array.ToObject<IEnumerable<string>>();
                    var extra = new ExtraTranslation
                    {
                        Type = @enum,
                        Name = name ?? throw new InvalidOperationException("Suggestion name was null."),
                        Words = description.ToArray()
                    };
                    finalSuggestions.Add(extra);
                }
            }
            return finalSuggestions;

        }

        //public IEnumerable<TranslationSequence> ExtractSuggestions(JToken json)
        //{
        //    JToken suggestions;
        //    Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
        //    try
        //    {
        //        suggestions = json[1]?[0]?[2] ?? json[1]?[0];
        //    }
        //    catch
        //    {
        //        return dictionary.ToExtraTranslationArray();
        //    }

        //    if (suggestions == null)
        //    {
        //        return dictionary.ToExtraTranslationArray();
        //    }
        //    foreach (JToken suggestion in suggestions)
        //    {
        //        if (!suggestion.HasValues)
        //        {
        //            continue;
        //        }

        //        FindWordProperties(suggestion, dictionary);
        //    }
        //    _cancellationToken.ThrowIfCancellationRequested();

        //    return dictionary.ToExtraTranslationArray();
        //}

        private static void FindWordProperties(JToken suggestion, IDictionary<string, List<string>> dictionary)
        {
            string key = suggestion[0].ToString();

            List<string> list =
                (
                    from item in suggestion[1]
                    where !string.IsNullOrWhiteSpace(item?.ToString())
                    select item.ToString()
                    ).ToList();

            dictionary.Add(key, list);
        }

        //public string[] ExtractSeeAlso(JToken json)
        //    => !json.HasValues ? new string[0] : 
    }
}