using GoogleTranslate.Implementations;
using GoogleTranslate.Translators;

namespace GoogleTranslateTests
{
    public class TranslateWrapper
    {
        public ITranslator Translator { get; set; }
        
        public IRemoteWebDriver Driver { get; set; }
    }
}