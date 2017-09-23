using GoogleTranslate.Words;
using OpenQA.Selenium.Remote;

namespace GoogleTranslate.Translators
{
    public interface ITranslator<T, in TE> where TE : RemoteWebDriver
    {
        TranslateWord Translate(
            TranslateWord word,
            T targetLanguage,
            TE driver);

        TranslateWord Detect(TranslateWord word, TE driver);
    }
}