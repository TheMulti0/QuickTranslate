using EasyTranslate.Enums;
using EasyTranslate.Translators;
using EasyTranslate.Words;

namespace ConsoleApplication
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            ITranslator translator = new GoogleTranslateClassicTranslator();
            new Program().Translate(translator);
        }

        private void Translate(ITranslator translator)
        {
            translator.Translate(new TranslateWord("שלום!"), TranslateLanguages.English);
        }
    }
}