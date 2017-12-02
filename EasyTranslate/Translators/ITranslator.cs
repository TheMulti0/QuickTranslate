using EasyTranslate.Enums;
using EasyTranslate.Implementations;
using EasyTranslate.Words;

namespace EasyTranslate.Translators
{
    public interface ITranslator
    {
        TranslateWord Translate(
            TranslateWord word,
            TranslateLanguages targetLanguage,
            IRemoteWebDriver driver);

        TranslateWord Detect(
            TranslateWord word, 
            IRemoteWebDriver driver);
    }
}