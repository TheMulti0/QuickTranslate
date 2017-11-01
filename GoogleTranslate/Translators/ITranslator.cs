using GoogleTranslate.Enums;
using GoogleTranslate.Implementations;
using GoogleTranslate.Words;

namespace GoogleTranslate.Translators
{
    public interface ITranslator
    {
        TranslateWord Translate(
            TranslateWord word,
            TranslateLanguages targetLanguage,
            IRemoteWebDriver driver);

        TranslateWord Detect(TranslateWord word, IRemoteWebDriver driver);
    }
}