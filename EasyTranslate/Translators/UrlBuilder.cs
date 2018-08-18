using System;
using System.Collections.Specialized;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using EasyTranslate.Extentions;
using EasyTranslate.TranslationData;

namespace EasyTranslate.Translators
{
    internal class UrlBuilder
    {
        public async Task<string> GetUrl(TranslateSequence sequence, TranslateLanguages lang)
        {
            string token = await GetTokenAsync(sequence);

            var builder = new UriBuilder("https://translate.google.com/translate_a/single");
            NameValueCollection query = HttpUtility.ParseQueryString("", Encoding.Unicode);

            string langValue = lang.GetDescriptionAttributeString();
            const string dtParameter = "at&dt=bd&dt=ex&dt=ld&dt=md&dt=qca&dt=rw&dt=rm&dt=ss&dt=t&dt=['at', 'bd', 'ex', 'ld', 'md', 'qca', 'rw', 'rm', 'ss', 't']";

            query["client"] = "t";
            query["sl"] = "auto";
            query["tl"] = langValue;
            query["hl"] = langValue;
            query["dt"] = dtParameter;
            query["ie"] = "UTF-8";
            query["oe"] = "UTF-8";
            query["otf"] = "2";
            query["ssel"] = "0";
            query["tsel"] = "4";
            query["kc"] = "20";
            query["tk"] = token;
            query["q"] = sequence.Word;

            builder.Query = HttpUtility.UrlDecode(query.ToString());
            return builder.Uri.ToString();
        }

        private static async Task<string> GetTokenAsync(TranslateSequence sequence)
        {
            var tkkGenerator = new TkkGenerator();
            string tkk = await tkkGenerator.GetTkkAsync();

            var tokenGenerator = new TokenGenerator();
            string token = tokenGenerator.GetToken(sequence.Word, tkk);
            return token;
        }
    }
}