using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace QuantitySystem.Units.UnitSystems
{

    public struct SIPrefix
    {
        private readonly string _Prefix;

        public string Prefix
        {
            get { return _Prefix; }
        }

        private readonly string _Symbol;

        public string Symbol
        {
            get { return _Symbol; }
        } 


        private double PrefixExponent;


        public SIPrefix(string prefix, string symbol, double exponent)
        {
            _Prefix = prefix;
            _Symbol = symbol;
            PrefixExponent = exponent;
        }

        public double Transfer(SIPrefix prefix)
        {
            return prefix.Factor / this.Factor;
        }

        #region SI Standard prefixes as static properties


        #region Positive
        public static SIPrefix Yotta { get { return new SIPrefix("yotta", "Y", 24); } }
        public static SIPrefix Zetta { get { return new SIPrefix("zetta", "Z", 21); } }
        public static SIPrefix Exa { get { return new SIPrefix("exa", "E", 18); } }
        public static SIPrefix Peta { get { return new SIPrefix("peta", "P", 15); } }
        public static SIPrefix Tera { get { return new SIPrefix("tera", "T", 12); } }
        public static SIPrefix Giga { get { return new SIPrefix("giga", "G", 9); } }
        public static SIPrefix Mega { get { return new SIPrefix("mega", "M", 6); } }
        public static SIPrefix Kilo { get { return new SIPrefix("kilo", "k", 3); } }
        public static SIPrefix Hecto { get { return new SIPrefix("hecto", "h", 2); } }
        public static SIPrefix Deka { get { return new SIPrefix("deka", "da", 1); } }

        #endregion

        public static SIPrefix Default { get { return new SIPrefix("", "", 0); } }

        #region Negative 
        public static SIPrefix Deci { get { return new SIPrefix("deci", "d", -1); } }
        public static SIPrefix Centi { get { return new SIPrefix("centi", "c", -2); } }
        public static SIPrefix Milli { get { return new SIPrefix("milli", "m", -3); } }
        public static SIPrefix Micro { get { return new SIPrefix("micro", "µ", -6); } }
        public static SIPrefix Nano { get { return new SIPrefix("nano", "n", -9); } }
        public static SIPrefix Pico { get { return new SIPrefix("pico", "p", -12); } }
        public static SIPrefix Femto { get { return new SIPrefix("femto", "f", -15); } }
        public static SIPrefix Atto { get { return new SIPrefix("atto", "a", -18); } }
        public static SIPrefix Zepto { get { return new SIPrefix("zepto", "z", -21); } }
        public static SIPrefix Yocto { get { return new SIPrefix("yocto", "y", -24); } }
        #endregion
        
        #endregion

        #region static constructor

        public static SIPrefix FromExponent(double exponent)
        {
            int exp = (int)exponent; 
            switch (exp)
            {
                case -24:
                    return Yocto;
                case -21:
                    return Zepto;
                case -18:
                    return Atto;
                case -15:
                    return Femto;
                case -12:
                    return Pico;
                case -9:
                    return Nano;
                case -6:
                    return Micro;
                case -3:
                    return Milli;
                case -2:
                    return Centi;
                case -1:
                    return Deci;
                case 0:
                    return Default;
                case 1:
                    return Deka;
                case 2:
                    return Hecto;
                case 3:
                    return Kilo;
                case 6:
                    return Mega;
                case 9:
                    return Giga;
                case 12:
                    return Tera;
                case 15:
                    return Peta;
                case 18:
                    return Exa;
                case 21:
                    return Zetta;
                case 24:
                    return Yotta;
                default:
                    throw new SIPrefixException("No SI Prefix found for exponent = " + exponent.ToString(CultureInfo.InvariantCulture));
            }
        }

        #endregion

        #region Properties
        public double Exponent
        {
            get
            {
                return this.PrefixExponent;
            }
        }

        public double Factor
        {
            get
            {
                return Math.Pow(10, PrefixExponent);
            }
        }
        #endregion



        #region Operations
        public SIPrefix Invert()
        {
            return SIPrefix.FromExponent(0 - this.PrefixExponent);
        }

        public static SIPrefix Add(SIPrefix firstPrefix, SIPrefix secondPrefix)
        {
            double exp = firstPrefix.Exponent + secondPrefix.Exponent;
            
            SIPrefix prefix = SIPrefix.FromExponent(exp);
            return prefix;
        }

        public static SIPrefix Subtract(SIPrefix firstPrefix, SIPrefix secondPrefix)
        {
            double exp = firstPrefix.Exponent - secondPrefix.Exponent;

            SIPrefix prefix = SIPrefix.FromExponent(exp);
            return prefix;
        }

        public static SIPrefix operator +(SIPrefix firstPrefix, SIPrefix secondPrefix)
        {
            return Add(firstPrefix, secondPrefix);
        }

        public static SIPrefix operator -(SIPrefix firstPrefix, SIPrefix secondPrefix)
        {
            return Subtract(firstPrefix, secondPrefix);
        }


        #endregion
    }

}
