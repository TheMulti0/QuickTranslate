namespace EasyTranslate.Words
{
    public class TranslateWord
    {
        public string Word { get; }

        public TranslateLanguages? Language { get; }
        
        public TranslateWord[] Suggestions { get; }
        
        public string[] Description { get; }

        public TranslateWord(
            string word,
            TranslateLanguages? language = null,
            TranslateWord[] suggestions = null,
            string[] description = null)
        {
            Word = word;
            Language = language;
            Suggestions = suggestions;
            Description = description;
        }
    }
}