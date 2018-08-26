using System.Collections.Generic;
using EasyTranslate.TranslationData;

namespace EasyTranslate.Extentions
{
    public static class DictionaryExtentions
    {
        public static IEnumerable<TranslationSequence> ToTranslationSequence(this Dictionary<string, List<string>> dictionary)
        {
            List<TranslationSequence> words = new List<TranslationSequence>();
            foreach (KeyValuePair<string, List<string>> pair in dictionary)
            {
                var word = new TranslationSequence(pair.Key, description: pair.Value.ToArray());
                words.Add(word);
            }
            return words;
        }
    }
}