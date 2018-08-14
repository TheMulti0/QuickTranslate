using System.Threading;
using System.Threading.Tasks;
using EasyTranslate.Words;

namespace EasyTranslate.Translators
{
    public interface ITranslator
    {
        TranslateSequence Translate(
            TranslateSequence sequence,
            TranslateLanguages targetLanguage);

        Task<TranslateSequence> TranslateAsync(
            TranslateSequence sequence,
            TranslateLanguages targetLanguage,
            CancellationToken token = default(CancellationToken));

        TranslateSequence Detect(TranslateSequence sequence);

        Task<TranslateSequence> DetectAsync(
            TranslateSequence sequence,
            CancellationToken token = default(CancellationToken));
    }
}