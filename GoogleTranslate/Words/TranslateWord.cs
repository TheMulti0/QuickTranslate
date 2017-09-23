using GoogleTranslate.Enums;

namespace GoogleTranslate.Words
{
    public class TranslateWord
    {
        public string Word { get; }

        public TranslateLanguages? Language { get; }

        public TranslateWord(string word, TranslateLanguages? language = null)
        {
            Word = word;
            Language = language;
        }
    }
}