using EasyTranslate.Implementations;
using EasyTranslate.Translators;

namespace EasyTranslateTests
{
    public class TranslateWrapper
    {
        public ITranslator Translator { get; set; }
        
        public IRemoteWebDriver Driver { get; set; }
    }
}