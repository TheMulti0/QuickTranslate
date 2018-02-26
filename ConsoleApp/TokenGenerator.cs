using System;
using System.Collections.Generic;
using System.Text;

namespace EasyTranslate.Translators
{
    internal class TokenGenerator
    {
        private int ASCIICode(string str)

        {

            //byte[] bt = Encoding.ASCII.GetBytes(str);

            //return Convert.ToInt32(bt[0]);



            return Convert.ToInt32(Convert.ToString(str[0], 10));

        }



        /// <summary>

        /// 时间戳处理值 再计算函数

        /// </summary>

        /// <param name="a"></param>

        /// <param name="b"></param>

        /// <returns></returns>

        private long Xi(long a, string b)

        {

            for (int i = 0; i < b.Length - 2; i += 3)

            {

                string c = b.Substring(i + 2, 1);

                int d = string.Compare("a", c) <= 0 ? ASCIICode(c) - 87 : Convert.ToInt32(c);    // "a" <= evenChar 比较大小

                long e = "+" == b.Substring(i + 1, 1) ? a >> d : a << d;

                a = "+" == b.Substring(i, 1) ? a + e & 4294967295 : a ^ e;

            }

            return a;

        }



        /// <summary>

        /// 取得字符串的token值

        /// </summary>

        /// <param name="str">计算token值的字符串</param>

        /// <param name="TKK">取得的TKK值</param>

        /// <returns>计算失败即返回Null</returns>

        public string GetToken(string str, string TKK)

        {

            if (string.IsNullOrWhiteSpace(str) || string.IsNullOrWhiteSpace(TKK)) return null;



            string[] tkk = TKK.Split('.');//(string.IsNullOrEmpty(TKK) ? "0.0" : TKK).Split('.');

            long h = int.Parse(tkk[0]);



            List<int> g = new List<int>();

            for (int i = 0; i < str.Length; i++)

            {

                int e = ASCIICode(str.Substring(i, 1));



                if (128 > e)

                {

                    g.Add(e);

                }

                else

                {

                    if (2048 > e)

                    {

                        g.Add(e >> 6 | 192);

                    }

                    else

                    {

                        if (55296 == (e & 64512) && i + 1 < str.Length && 56320 == (ASCIICode(str.Substring(i + 1)) & 64512))

                        {

                            e = 65536 + ((e & 1023) << 10) + (ASCIICode(str.Substring(++i)) & 1023);

                            g.Add(e >> 18 | 240);

                            g.Add(e >> 12 & 63 | 128);

                        }

                        else

                        {

                            g.Add(e >> 12 | 224);

                            g.Add(e >> 6 & 63 | 128);

                        }

                    }

                    g.Add(e & 63 | 128);

                }

            }



            long a = h;

            foreach (int t in g)
            {
                a += t;

                a = Xi(a, "+-a^+6");
            }

            a = Xi(a, "+-3^+b+-f");

            a ^= Convert.ToInt64(tkk[1]);

            if (0 > a) a = (a & 2147483647) + 2147483648;

            a %= 1000000;

            return a + "." + (a ^ h);

        }
    }
}
