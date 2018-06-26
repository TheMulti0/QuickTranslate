using EasyTranslate.Enums;
using EasyTranslate.Translators;
using EasyTranslate.Words;

namespace ConsoleTests
{
    public class Class1
    {
        public static void Main(string[] args)
        {
            var translator = new GoogleTranslateClassicTranslator();
            new Class1().Translate(translator);
        }

        private void Translate(GoogleTranslateClassicTranslator translator)
        {
            translator.Translate(new TranslateWord("Hello"), TranslateLanguages.French);
        }
    }
}