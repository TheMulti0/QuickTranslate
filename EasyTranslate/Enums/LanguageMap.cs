using System.Collections.Generic;
using System.Linq;

namespace EasyTranslate.Enums
{
    internal class LanguageMap
    {
        public Dictionary<string, TranslateLanguages> Map { get; set; }

        public LanguageMap()
        {
            Map = new Dictionary<string, TranslateLanguages>();

            Map.Add("af", TranslateLanguages.Afrikaans);
            Map.Add("sq", TranslateLanguages.Albanian);
            Map.Add("am", TranslateLanguages.Amharic);
            Map.Add("ar", TranslateLanguages.Arabic);
            Map.Add("hy", TranslateLanguages.Armenian);
            Map.Add("az", TranslateLanguages.Azerbaijani);
            Map.Add("eu", TranslateLanguages.Basque);
            Map.Add("be", TranslateLanguages.Belarusian);
            Map.Add("bn", TranslateLanguages.Bengali);
            Map.Add("bs", TranslateLanguages.Bosnian);
            Map.Add("bg", TranslateLanguages.Bulgarian);
            Map.Add("ca", TranslateLanguages.Catalan);
            Map.Add("ceb", TranslateLanguages.Cebuano);
            Map.Add("ny", TranslateLanguages.Chichewa);
            Map.Add("zh-cn", TranslateLanguages.ChineseSimplified);
            Map.Add("zh-tw", TranslateLanguages.ChineseTraditional);
            Map.Add("co", TranslateLanguages.Corsican);
            Map.Add("hr", TranslateLanguages.Croatian);
            Map.Add("da", TranslateLanguages.Danish);
            Map.Add("nl", TranslateLanguages.Dutch);
            Map.Add("en", TranslateLanguages.English);
            Map.Add("eo", TranslateLanguages.Esperanto);
            Map.Add("et", TranslateLanguages.Estonan);
            Map.Add("tl", TranslateLanguages.Filipino);
            Map.Add("fi", TranslateLanguages.Finnish);
            Map.Add("fr", TranslateLanguages.French);
            Map.Add("fy", TranslateLanguages.Frisian);
            Map.Add("gl", TranslateLanguages.Galician);
            Map.Add("ka", TranslateLanguages.Georgian);
            Map.Add("de", TranslateLanguages.German);
            Map.Add("el", TranslateLanguages.Greek);
            Map.Add("gu", TranslateLanguages.Gujarati);
            Map.Add("ht", TranslateLanguages.HaitianCreole);
            Map.Add("ha", TranslateLanguages.Hausa);
            Map.Add("haw", TranslateLanguages.Hawaiian);
            Map.Add("iw", TranslateLanguages.Hebrew);
            Map.Add("hi", TranslateLanguages.Hindi);
            Map.Add("hmn", TranslateLanguages.Hmong);
            Map.Add("hu", TranslateLanguages.Hungarian);
            Map.Add("is", TranslateLanguages.Icelandic);
            Map.Add("ig", TranslateLanguages.Igbo);
            Map.Add("id", TranslateLanguages.Indonesian);
            Map.Add("ga", TranslateLanguages.Irish);
            Map.Add("it", TranslateLanguages.Italian);
            Map.Add("ja", TranslateLanguages.Japanese);
            Map.Add("jw", TranslateLanguages.Javanese);
            Map.Add("kn", TranslateLanguages.Kannada);
            Map.Add("kk", TranslateLanguages.Kazakh);
            Map.Add("km", TranslateLanguages.Khmer);
            Map.Add("ko", TranslateLanguages.Korean);
            Map.Add("ku", TranslateLanguages.KurdishKurmanji);
            Map.Add("ky", TranslateLanguages.Kyrgyz);
            Map.Add("lo", TranslateLanguages.Lao);
            Map.Add("la", TranslateLanguages.Latin);
            Map.Add("lv", TranslateLanguages.Latvian);
            Map.Add("lt", TranslateLanguages.Lithuanian);
            Map.Add("lb", TranslateLanguages.Luxembourgish);
            Map.Add("mk", TranslateLanguages.Macedonian);
            Map.Add("mg", TranslateLanguages.Malagasy);
            Map.Add("ms", TranslateLanguages.Malay);
            Map.Add("ml", TranslateLanguages.Malayalam);
            Map.Add("mt", TranslateLanguages.Maltese);
            Map.Add("mi", TranslateLanguages.Maori);
            Map.Add("mr", TranslateLanguages.Marathi);
            Map.Add("mn", TranslateLanguages.Mongolian);
            Map.Add("my", TranslateLanguages.MynamarBurmese);
            Map.Add("ne", TranslateLanguages.Nepali);
            Map.Add("no", TranslateLanguages.Norwegian);
            Map.Add("ps", TranslateLanguages.Pashto);
            Map.Add("fa", TranslateLanguages.Persian);
            Map.Add("pl", TranslateLanguages.Polish);
            Map.Add("pt", TranslateLanguages.Portuguese);
            Map.Add("ma", TranslateLanguages.Punjabi);
            Map.Add("ro", TranslateLanguages.Romanian);
            Map.Add("ru", TranslateLanguages.Russian);
            Map.Add("sm", TranslateLanguages.Samoan);
            Map.Add("gd", TranslateLanguages.ScotsGaelic);
            Map.Add("sr", TranslateLanguages.Serbian);
            Map.Add("st", TranslateLanguages.Sesotho);
            Map.Add("sn", TranslateLanguages.Shona);
            Map.Add("sd", TranslateLanguages.Sindhi);
            Map.Add("si", TranslateLanguages.Sinhala);
            Map.Add("sk", TranslateLanguages.Slovak);
            Map.Add("sl", TranslateLanguages.Slovenian);
            Map.Add("so", TranslateLanguages.Somali);
            Map.Add("es", TranslateLanguages.Spanish);
            Map.Add("su", TranslateLanguages.Sundanese);
            Map.Add("sw", TranslateLanguages.Swahili);
            Map.Add("sv", TranslateLanguages.Swedish);
            Map.Add("tg", TranslateLanguages.Tajik);
            Map.Add("ta", TranslateLanguages.Tamil);
            Map.Add("te", TranslateLanguages.Telugu);
            Map.Add("th", TranslateLanguages.Thai);
            Map.Add("tr", TranslateLanguages.Turkish);
            Map.Add("uk", TranslateLanguages.Ukrainian);
            Map.Add("ur", TranslateLanguages.Urdu);
            Map.Add("uz", TranslateLanguages.Uzbek);
            Map.Add("vi", TranslateLanguages.Vietnamese);
            Map.Add("cy", TranslateLanguages.Welsh);
            Map.Add("xh", TranslateLanguages.Xhosa);
            Map.Add("yi", TranslateLanguages.Yiddish);
            Map.Add("yo", TranslateLanguages.Yoruba);
            Map.Add("zu", TranslateLanguages.Zulu);
        }

        public KeyValuePair<string, TranslateLanguages> Find(string key) 
            => Map.FirstOrDefault(pair => pair.Key == key);

        public KeyValuePair<string, TranslateLanguages> Find(TranslateLanguages value) 
            => Map.FirstOrDefault(pair => pair.Value == value);
    }
}