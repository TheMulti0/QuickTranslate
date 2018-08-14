using System.Collections.Generic;

namespace EasyTranslate.Words
{
    public class TranslateSequence
    {
        public string Word { get; }

        public TranslateLanguages? Language { get; }
        
        public IEnumerable<TranslateSequence> Suggestions { get; }
        
        public string[] Description { get; }

        public TranslateSequence(
            string word,
            TranslateLanguages? language = null,
            IEnumerable<TranslateSequence> suggestions = null,
            string[] description = null)
        {
            Word = word;
            Language = language;
            Suggestions = suggestions;
            Description = description;
        }
    }
}