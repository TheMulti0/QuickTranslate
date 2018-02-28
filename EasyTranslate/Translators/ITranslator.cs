using EasyTranslate.Enums;
using EasyTranslate.Words;

namespace EasyTranslate.Translators
{
    public interface ITranslator
    {
        TranslateWord Translate(
            TranslateWord word,
            TranslateLanguages targetLanguage);

        TranslateWord Detect(TranslateWord word, TranslateLanguages randomLanguage = TranslateLanguages.French);
    }
}