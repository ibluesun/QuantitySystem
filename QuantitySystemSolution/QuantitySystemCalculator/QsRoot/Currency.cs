using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParticleLexer.StandardTokens;
using System.IO;
using System.Globalization;

namespace QsRoot
{
    public static class Currency
    {

        static ParticleLexer.Token CurrenciesJson;
        static Dictionary<string, double> Currs ;

        static Currency()
        {
            ReadCurrenciesJson();
        }

        /// <summary>
        /// Getting the exchange rate file each day so we don't hit the service too much
        /// </summary>
        static string TodayChangeRatesFile
        {
            get
            {
                string file = string.Format("XChangeRates-{0}-{1}-{2}.json", DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
                return file;
            }
        }


        static void DownloadExchangeFile()
        {
            string web = "http://openexchangerates.org/api/latest.json?app_id=25e93e39fd2b43939b2b62939479609d";
            System.Net.WebClient wc = new System.Net.WebClient();
            byte[] xch = wc.DownloadData(web);
            File.WriteAllBytes(TodayChangeRatesFile, xch);
        }

        static void ReadCurrenciesJson()
        {
            if (!File.Exists(TodayChangeRatesFile))
            {
                // get the file from http://openexchangerates.org
                DownloadExchangeFile();
            }

            using (var rr = new System.IO.StreamReader(TodayChangeRatesFile))
            {
                CurrenciesJson = ParticleLexer.Token.ParseText(rr.ReadToEnd());
            }

            /*
             * json format is 
             * 
             * { "key": value,  "key": "other value", "key":{ "key":value }
             * 
             * value is either string or number
             * 
             */
            CurrenciesJson = CurrenciesJson.TokenizeTextStrings();
            CurrenciesJson = CurrenciesJson.MergeTokens<WordToken>();
            CurrenciesJson = CurrenciesJson.RemoveAnySpaceTokens();
            CurrenciesJson = CurrenciesJson.RemoveNewLineTokens();
            CurrenciesJson = CurrenciesJson.MergeTokens<NumberToken>();
            CurrenciesJson = CurrenciesJson.MergeSequenceTokens<MergedToken>(
                typeof(ParticleLexer.CommonTokens.TextStringToken),
                typeof(ColonToken),
                typeof(NumberToken)
                );


            Currs = new Dictionary<string, double>();

            // find rates key
            foreach (var tok in CurrenciesJson)
            {
                if (tok.TokenClassType == typeof(MergedToken))
                {
                    Currs.Add(tok[0].TrimTokens(1, 1).TokenValue, double.Parse(tok[2].TokenValue, CultureInfo.InvariantCulture));
                }
            }
        }

        public static double CurrencyConverter(string currency)
        {
            return 1.0 / Currs[currency];
        }

        /// <summary>
        /// updates the currency file and the conversion factors
        /// </summary>
        public static void Update()
        {
            DownloadExchangeFile();

            ReadCurrenciesJson();
        }
    }
}
