using System.Threading;
using System.Threading.Tasks;
using EasyTranslate.TranslationData;

namespace EasyTranslate.Translators
{
    public interface ITranslator
    {
        Task<TranslationSequence> TranslateAsync(
            TranslationSequence sequence,
            TranslateLanguages targetLanguage,
            CancellationToken token = default(CancellationToken));
        
        Task<TranslationSequence> DetectAsync(
            TranslationSequence sequence,
            CancellationToken token = default(CancellationToken));
    }
}