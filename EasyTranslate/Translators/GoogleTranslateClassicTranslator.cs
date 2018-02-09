using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Web;
using EasyTranslate.Enums;
using EasyTranslate.Implementations;
using EasyTranslate.Words;

namespace EasyTranslate.Translators
{
    public class GoogleTranslateClassicTranslator : ITranslator
    {
        public TranslateWord Translate(TranslateWord word, TranslateLanguages targetLanguage, IRemoteWebDriver driver)
        {
            
            return new TranslateWord("dsa");
        }

        static void Main()
        {
            var builder = new UriBuilder("https://translate.google.com/translate_a/single");

            var languageMap = new LanguageMap();
            NameValueCollection query = HttpUtility.ParseQueryString(builder.Query);

            query["client"] = "t";

            query["sl"] = "auto";

            query["tl"] = "fr";

            query["hl"] = "fr";

            query["dt"] = "['at', 'bd', 'ex', 'ld', 'md', 'qca', 'rw', 'rm', 'ss', 't']";

            query["ie"] = "UTF-8";

            query["oe"] = "UTF-8";

            query["otf"] = "1";

            query["ssel"] = "0";

            query["tsel"] = "0";

            query["kc"] = "7";

            query["q"] = "hello";

            builder.Query = query.ToString();
            //https://translate.google.com/translate_a/single?client=t&sl=auto&tl=en&hl=en&dt=['at', 'bd', 'ex', 'ld', 'md', 'qca', 'rw', 'rm', 'ss', 't']&ie=UTF-8&oe=UTF-8&otf=1&ssel=0&tsel=0&kc=7&q=bonjour
            WebRequest W = WebRequest.Create(builder.Uri);
            var r = W.GetResponse();
        }

        public TranslateWord Detect(TranslateWord word, IRemoteWebDriver driver)
        {
            throw new NotImplementedException();
        }
    }
}
