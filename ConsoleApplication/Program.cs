using System;
using System.Threading;
using System.Threading.Tasks;
using EasyTranslate.TranslationData;
using EasyTranslate.Translators;

namespace ConsoleApplication
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            ITranslator translator = new GoogleTranslator();
            Translate(translator);
        }

        private static async void Translate(ITranslator translator)
        {
            var tokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = tokenSource.Token;
            Task<TranslationSequence> a = translator.TranslateAsync(new TranslationSequence("שלום"), TranslateLanguages.English, token:cancellationToken);
            Console.WriteLine(a.Result.Sequence);
            TranslationSequence c = await translator.TranslateAsync(new TranslationSequence("שלום! באמת"), TranslateLanguages.English, token:cancellationToken);
            Console.WriteLine(c.Sequence);
            Console.ReadLine();
        }
    }
}