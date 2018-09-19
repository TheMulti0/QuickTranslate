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
        public async Task<string> GetUrl(
            TranslationSequence sequence, 
            TranslateLanguages targetLanguage,
            TranslateLanguages? sourceLanguage)
        {
            string token = await new GoogleKeyTokenGenerator().GenerateAsync(sequence.Sequence);
            //string token = await GetTokenAsync(sequence);

            var builder = new UriBuilder("https://translate.google.com/translate_a/single");
            NameValueCollection query = HttpUtility.ParseQueryString("", Encoding.Unicode);

            string langValue = targetLanguage.GetDescriptionAttributeString();
            var sourceLangValue = sourceLanguage?.GetDescriptionAttributeString();
            const string dtParameter = "at&dt=bd&dt=ex&dt=ld&dt=md&dt=qca&dt=rw&dt=rm&dt=ss&dt=t&dt=['at', 'bd', 'ex', 'ld', 'md', 'qca', 'rw', 'rm', 'ss', 't']";

            query["client"] = "t";
            query["sl"] = sourceLangValue ?? "auto";
            query["tl"] = langValue;
            query["hl"] = "en";
            query["dt"] = dtParameter;
            query["ie"] = "UTF-8";
            query["oe"] = "UTF-8";
            query["dt"] = "at&dt=bd&dt=ex&dt=ld&dt=md&dt=qca&dt=rw&dt=rm&dt=ss&dt=t";
            query["otf"] = "1";
            query["ssel"] = "0";
            query["tsel"] = "0";
            query["kc"] = "7";
            query["tk"] = token;
            query["q"] = sequence.Sequence;

            builder.Query = HttpUtility.UrlDecode(query.ToString());
            return builder.Uri.ToString();
        }

        private static async Task<string> GetTokenAsync(TranslationSequence sequence)
        {
            var tkkGenerator = new TkkGenerator();
            var tkk = await tkkGenerator.GetTkkAsync();

            var tokenGenerator = new TokenGenerator();
            var token = tokenGenerator.GetToken(sequence.Sequence, tkk);
            return token;
        }
    }
}