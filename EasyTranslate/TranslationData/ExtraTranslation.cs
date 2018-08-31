using System.Collections.Generic;

namespace EasyTranslate.TranslationData
{
    public class ExtraTranslation
    {
        public TranslationType Type { get; }

        public string Name { get; internal set; }

        public IEnumerable<string> Words { get; internal set; }

        public ExtraTranslation(TranslationType type, string name, IEnumerable<string> words)
        {
            Type = type;
            Name = name;
            Words = words;
        }
    }
}