using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qs
{
    /// <summary>
    /// Contains the rest of the trionogometric functions
    /// </summary>
    public static class CoMath
    {
        public static double Sec(double x)
        {
            return 1.0 / Math.Cos(x);
        }

        public static double Csc(double x)
        {
            return 1.0 / Math.Sin(x);
        }

        public static double Cot(double x)
        {
            return 1.0 / Math.Tan(x);
        }


        public static double Sech(double x)
        {
            return 1.0 / Math.Cosh(x);
        }

        public static double Csch(double x)
        {
            return 1.0 / Math.Sinh(x);
        }

        public static double Coth(double x)
        {
            return 1.0 / Math.Tanh(x);
        }


        public static double Asec(double x)
        {
            return Math.Acos(1.0 / x);
        }

        public static double Acsc(double x)
        {
            return Math.Asin(1.0 / x);
        }

        public static double Acot(double x)
        {
            return Math.Atan(1.0 / x);
        }


        public static double Acosh(double x)
        {
            return Math.Log(x + Math.Sqrt(x * x - 1.0));
        }

        public static double Asinh(double x)
        {
            return Math.Log(x + Math.Sqrt(x * x + 1.0));
        }

        public static double Atanh(double x)
        {
            return 0.5 * Math.Log((1.0 + x) / (1.0 - x));
        }


        public static double Asech(double x)
        {
            return Acosh(1.0 / x);
        }

        public static double Acsch(double x)
        {
            return Asinh(1.0 / x);
        }

        public static double Acoth(double x)
        {
            return Atanh(1.0 / x);
        }

    }
}
