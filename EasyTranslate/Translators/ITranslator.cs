using System.Threading;
using System.Threading.Tasks;
using EasyTranslate.Words;

namespace EasyTranslate.Translators
{
    public interface ITranslator
    {
        TranslateWord Translate(
            TranslateWord word,
            TranslateLanguages targetLanguage);

        Task<TranslateWord> TranslateAsync(
            TranslateWord word,
            TranslateLanguages targetLanguage,
            CancellationToken token = default(CancellationToken));

        TranslateWord Detect(
            TranslateWord word,
            TranslateLanguages randomLanguage = TranslateLanguages.French);

        Task<TranslateWord> DetectAsync(
            TranslateWord word,
            TranslateLanguages randomLanguage = TranslateLanguages.French,
            CancellationToken token = default(CancellationToken));
    }
}