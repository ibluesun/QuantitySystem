using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Qs.Numerics
{
    public struct Rational
    {
        public float num, den;

        public Rational(float num, float den)
        {
            this.num = num;
            this.den = den;
        }


        /// <summary>
        /// Simplify the output  
        ///    for example if the result is 4/8  then it is simplified to 2/4 then 1/2
        /// </summary>
        /// <param name="num"></param>
        /// <param name="den"></param>
        private static void Simplify(ref float num, ref float den)
        {
            for (int fac = 24; fac >= 2; fac--)
            {
                if ((num % fac == 0) && (den % fac == 0))
                {
                    num = num / fac;
                    den = den / fac;
                    break;
                }
            }
        }

        /// <summary>
        /// Add operator
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Rational operator +(Rational a, Rational b)
        {
            float num = a.num * b.den + b.num * a.den;
            float den = a.den * b.den;

            Simplify(ref num, ref den);

            return new Rational(num, den);
        }

        public static Rational operator -(Rational a, Rational b)
        {
            float num = a.num * b.den - b.num * a.den;
            float den = a.den * b.den;

            Simplify(ref num, ref den);

            return new Rational(num, den);
        }

        // overload operator *
        public static Rational operator *(Rational a, Rational b)
        {
            return new Rational(a.num * b.num, a.den * b.den);
        }

        public static Rational operator /(Rational a, Rational b)
        {
            return new Rational(a.num * b.den, a.den * b.num);
        }

        public static Rational operator /(double a, Rational b)
        {
            var am = new Rational((float)a,1);
            return am / b;
        }

        public static Rational operator *(Rational a, double b)
        {
            return new Rational(a.num * (float)b, a.den);
        }

        public static Rational operator *(double a, Rational b)
        {
            return new Rational((float)a * b.num, b.den);
        }

        // define operator double
        public static implicit operator double(Rational f)
        {
            return (double)f.num / f.den;
        }

        public static Rational Pow(Rational a, double power)
        {
            var result = new Rational((float)System.Math.Pow(a.num, (float)power), (float)System.Math.Pow(a.den, (float)power));
            return result;
        }

        public double Value
        {
            get
            {
                return (double)num / den;
            }
        }

        public override string ToString()
        {
            return "(" + num.ToString(CultureInfo.InvariantCulture) + "/" + den.ToString(CultureInfo.InvariantCulture) + ")";
        }


        public string ToQsSyntax()
        {

            return "Q{ "
            + num.ToString(CultureInfo.InvariantCulture) + ", "
            + den.ToString(CultureInfo.InvariantCulture) + "}";
        }

    }
}
