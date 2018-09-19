using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using EasyTranslate.Exceptions;
using EasyTranslate.Extentions;

namespace EasyTranslate.Translators
{
    internal class GoogleKeyTokenGenerator
    {
        private readonly Uri _address = new Uri("https://translate.google.com");

        private ExternalKey _currentExternalKey;

        public GoogleKeyTokenGenerator()
        {
            _currentExternalKey = new ExternalKey(0, 0);
        }

        public int UnixTotalHours
        {
            get
            {
                var unixTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                return (int) DateTime.UtcNow.Subtract(unixTime).TotalHours;
            }
        }

        /// <summary>
        ///     True, if the current key cannot be used for a token generate
        /// </summary>
        public bool IsExternalKeyObsolete => _currentExternalKey.Time != UnixTotalHours;


        /// <summary>
        ///     The proxy server that is used to send requests
        /// </summary>
        public IWebProxy Proxy { get; set; }

        /// <summary>
        ///     Requests timeout
        /// </summary>
        public TimeSpan TimeOut { get; set; } = TimeSpan.FromSeconds(10);

        /// <summary>
        ///     <p>Generate the token for a string</p>
        /// </summary>
        /// <param name="source">The string to receive the token</param>
        /// <returns>Token for the current string</returns>
        public virtual async Task<string> GenerateAsync(string source)
        {
            if (IsExternalKeyObsolete)
            {
                try
                {
                    _currentExternalKey = await GetNewExternalKeyAsync();
                }
                catch (TokenGenerationException)
                {
                    throw new NotSupportedException();
                }
            }

            var time = DecrypthAlgorythm(source);

            return time.ToString() + '.' + (time ^ _currentExternalKey.Time);
        }

        public async Task<ExternalKey> GetNewExternalKeyAsync()
        {
            HttpWebRequest request = WebRequest.CreateHttp(_address);
            HttpWebResponse response;
            request.Proxy = Proxy;
            request.ContinueTimeout = (int) TimeOut.TotalMilliseconds;
            request.ContentType = "application/x-www-form-urlencoded";


            try
            {
                response = (HttpWebResponse) await request.GetResponseAsync();
            }
            catch (WebException e)
            {
                throw new TokenGenerationException(e);
            }

            string result;

            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                result = await streamReader.ReadToEndAsync();
            }


            long number1, number2;
            string textNumber1, textNumber2;

            try
            {
                var index = result.IndexOf(@"var a\x3d", StringComparison.Ordinal);
                textNumber1 = result.GetTextBetween(@"var a\x3d", ";", index);
                textNumber2 = result.GetTextBetween(@"var b\x3d", ";", index);

                if (textNumber1 == null || textNumber2 == null)
                {
                    throw new TokenGenerationException();
                }
            }
            catch (ArgumentException e)
            {
                throw new TokenGenerationException(e);
            }


            if (!long.TryParse(textNumber1, out number1) || !long.TryParse(textNumber2, out number2))
            {
                throw new TokenGenerationException();
            }

            var newExternalKey = new ExternalKey(UnixTotalHours, number1 + number2);
            return newExternalKey;
        }

        private long DecrypthAlgorythm(string source)
        {
            var code = new List<long>();

            for (var g = 0; g < source.Length; g++)
            {
                int l = source[g];
                if (l < 128)
                {
                    code.Add(l);
                }
                else
                {
                    if (l < 2048)
                    {
                        code.Add((l >> 6) | 192);
                    }
                    else
                    {
                        if (55296 == (l & 64512) && g + 1 < source.Length && 56320 == (source[g + 1] & 64512))
                        {
                            l = 65536 + ((l & 1023) << 10) + (source[++g] & 1023);
                            code.Add((l >> 18) | 240);
                            code.Add(((l >> 12) & 63) | 128);
                        }
                        else
                        {
                            code.Add((l >> 12) | 224);
                        }

                        code.Add(((l >> 6) & 63) | 128);
                    }

                    code.Add((l & 63) | 128);
                }
            }

            var time = _currentExternalKey.Time;

            foreach (var i in code)
            {
                time += i;
                Xr(ref time, "+-a^+6");
            }

            Xr(ref time, "+-3^+b+-f");

            time ^= _currentExternalKey.Value;

            if (time < 0)
            {
                time = (time & 2147483647) + 2147483648;
            }

            time %= (long) 1e6;

            return time;
        }

        private static void Xr(ref long a, string b)
        {
            for (var c = 0; c < b.Length - 2; c += 3)
            {
                long d = b[c + 2];

                d = 'a' <= d ? d - 87 : (long) char.GetNumericValue((char) d);
                d = '+' == b[c + 1] ? (long) ((ulong) a >> (int) d) : a << (int) d;
                a = '+' == b[c] ? (a + d) & 4294967295 : a ^ d;
            }
        }
    }
}