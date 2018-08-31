using System.Collections.Generic;

namespace EasyTranslate.TranslationData
{
    public class TranslationSequence
    {
        public string Sequence { get; }

        public TranslateLanguages? Language { get; }

        public IEnumerable<string> Description { get; }

        public IEnumerable<ExtraTranslation> Suggestions { get; }

        public TranslationSequence(
            string sequence,
            TranslateLanguages? language = null,
            IEnumerable<string> description = null,
            IEnumerable<ExtraTranslation> suggestions = null)
        {
            Sequence = sequence;
            Language = language;
            Description = description;
            Suggestions = suggestions;
        }
    }
}