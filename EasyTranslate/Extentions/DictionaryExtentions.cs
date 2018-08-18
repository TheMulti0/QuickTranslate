using System.Collections.Generic;
using EasyTranslate.TranslationData;

namespace EasyTranslate.Extentions
{
    public static class DictionaryExtentions
    {
        public static IEnumerable<TranslateSequence> ToTranslateSequence(this Dictionary<string, List<string>> dictionary)
        {
            List<TranslateSequence> words = new List<TranslateSequence>();
            foreach (KeyValuePair<string, List<string>> pair in dictionary)
            {
                var word = new TranslateSequence(pair.Key, description: pair.Value.ToArray());
                words.Add(word);
            }
            return words;
        }
    }
}