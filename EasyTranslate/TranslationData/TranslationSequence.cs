using System.Collections.Generic;

namespace EasyTranslate.TranslationData
{
    public class TranslationSequence
    {
        public string Word { get; }

        public TranslateLanguages? Language { get; }

        public string[] Description { get; }

        public IEnumerable<ExtraTranslation> Suggestions { get; }

        public TranslationSequence(
            string word,
            TranslateLanguages? language = null,
            string[] description = null,
            IEnumerable<ExtraTranslation> suggestions = null)
        {
            Word = word;
            Language = language;
            Description = description;
            Suggestions = suggestions;
        }
    }
}