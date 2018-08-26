namespace EasyTranslate.TranslationData
{
    public class TranslationSequence
    {
        public string Word { get; }

        public TranslateLanguages? Language { get; }

        public string[] Description { get; }

        public TranslationType? Type { get; }

        public TranslationSequence[] Suggestions { get; }

        public TranslationSequence(
            string word,
            TranslateLanguages? language = null,
            string[] description = null,
            TranslationType? type = null,
            TranslationSequence[] suggestions = null)
        {
            Word = word;
            Language = language;
            Description = description;
            Type = type;
            Suggestions = suggestions;
        }
    }
}