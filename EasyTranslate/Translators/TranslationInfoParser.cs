using System;
using System.Collections.Generic;
using System.Linq;
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

        public static IEnumerable<ExtraTranslation> ExtractSuggestions(JToken json)
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
            object enumObject = Enum.Parse(typeof(TranslationType), type.CapitalizeFirstLetter());
            var @enum = (TranslationType) enumObject;

            return GetSuggestions(suggestions, @enum);

        }

        private static IEnumerable<ExtraTranslation> GetSuggestions(JToken suggestions, TranslationType @enum)
        {
            List<ExtraTranslation> finalSuggestions = new List<ExtraTranslation>();
            foreach (JToken s in suggestions.Skip(1))
            {
                if (!s.HasValues ||
                    !s.FirstOrDefault()
                        .HasValues)
                {
                    continue;
                }
                List<string> names = new List<string>();

                foreach (JToken suggestion in s)
                {
                    foreach (JToken small in suggestion)
                    {
                        if (small.HasValues)
                        {
                            JArray array = JArray.Parse(small.ToString());
                            IEnumerable<string> descriptions = array.ToObject<IEnumerable<string>>();
                            names.ForEach(
                                name => ValidateSuggestions(new ExtraTranslation(@enum, name, descriptions), finalSuggestions));
                            break;
                        }
                        names.Add((string) small);
                    }
                }
            }
            return finalSuggestions;
        }

        private static void ValidateSuggestions(
            ExtraTranslation extra,
            ICollection<ExtraTranslation> finalSuggestions)
        {
            extra.Name = extra.Name.CapitalizeFirstLetter();
            if (finalSuggestions.All(final => final.Name != extra.Name))
            {
                finalSuggestions.Add(extra);
            }
//            else
//            {
//                if (!finalSuggestions.Any(final => final.Words.Count() >= extra.Words.Count()))
//                {
//                    return;
//                }
//                ExtraTranslation prev = finalSuggestions.FirstOrDefault(final => final.Name == extra.Name);
//                if (prev == null)
//                {
//                    return;
//                }
//                prev.Words = prev.Words
//                    .Concat(extra.Words)
//                    .Distinct()
//                    .ToArray();
//            }
        }
    }
}