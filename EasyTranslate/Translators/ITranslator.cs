using System.Threading;
using System.Threading.Tasks;
using EasyTranslate.Words;

namespace EasyTranslate.Translators
{
    public interface ITranslator
    {
        Task<TranslateWord> TranslateAsync(
            TranslateWord word,
            TranslateLanguages targetLanguage,
            CancellationToken token = default(CancellationToken));

        Task<TranslateWord> DetectAsync(
            TranslateWord word,
            TranslateLanguages randomLanguage = TranslateLanguages.French,
            CancellationToken token = default(CancellationToken));
    }
}