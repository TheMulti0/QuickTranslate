using System.Threading;
using System.Threading.Tasks;
using EasyTranslate.Exceptions;
using EasyTranslate.TranslationData;

namespace EasyTranslate.Translators
{
    public interface ITranslator
    {
        /// <summary>
        /// Asynchronous translation of a sequence from language to language
        /// </summary>
        /// <param name="sequence">The desired sequence to translate</param>
        /// <param name="targetLanguage">The language desired to translate the sequence to</param>
        /// <param name="token">An optional cancellation token to cancel the operation</param>
        /// <exception cref="TranslationFailedException">Thrown if the translation operation fails</exception>
        /// <returns>A new sequence containing the translated sequence, its language and additional information</returns>
        /// <seealso cref="DetectAsync"/>
        Task<TranslationSequence> TranslateAsync(
            TranslationSequence sequence,
            TranslateLanguages targetLanguage,
            CancellationToken token = default(CancellationToken));
        
        /// <summary>
        /// Asynchronous sequence language detection
        /// </summary>
        /// <param name="sequence">The desired sequence to detect its language</param>
        /// <param name="token">An optional cancellation token to cancel the operation</param>
        /// <exception cref="DetectionFailedException">Thrown if the detection operation fails</exception>
        /// <returns>A new sequence containing the original word, and the detected language</returns>
        /// <seealso cref="TranslateAsync"/>
        Task<TranslationSequence> DetectAsync(
            TranslationSequence sequence,
            CancellationToken token = default(CancellationToken));
    }
}