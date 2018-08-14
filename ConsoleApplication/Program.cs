using System;
using System.Threading;
using System.Threading.Tasks;
using EasyTranslate.Translators;
using EasyTranslate.Words;

namespace ConsoleApplication
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            ITranslator translator = new GoogleTranslator();
            Translate(translator);
        }

        private static void Translate(ITranslator translator)
        {
            var tokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = tokenSource.Token;
            
            Task<TranslateSequence> a = translator.TranslateAsync(new TranslateSequence("שלום!"), TranslateLanguages.English, cancellationToken);
            Console.WriteLine(a.Result.Word);
        }
    }
}