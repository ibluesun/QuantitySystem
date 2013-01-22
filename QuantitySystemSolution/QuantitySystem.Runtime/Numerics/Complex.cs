using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Qs.Numerics
{
    /// <summary>
    /// Qs Complex storage type.
    /// </summary>
    public struct Complex
    {
        private double _real, _imaginary;

        public double Real
        {
            get
            {
                return _real;
            }
        }

        public double Imaginary
        {
            get
            {
                
                return _imaginary;
            }
        }


        public static readonly Complex Zero = new Complex(0, 0);
        public static readonly Complex One = new Complex(1.0, 0.0);
        public static readonly Complex ImaginaryOne = new Complex(0.0, 1.0);



        public Complex(double real, double imaginary)
        {

            _real = real;
            _imaginary = imaginary;
        }



        public override string ToString()
        {
            return "(" + Real.ToString(CultureInfo.InvariantCulture) + ", " + Imaginary.ToString(CultureInfo.InvariantCulture) + "i)";
        }
        public string ToQsSyntax()
        {
            return "C{" + Real.ToString(CultureInfo.InvariantCulture) + ", " + Imaginary.ToString(CultureInfo.InvariantCulture) + "}";
        }


        public static implicit operator Complex(double d)
        {
            return new Complex(d, 0);
        }

        public static bool operator ==(Complex lhs, Complex rhs)
        {
            if ((lhs._real == rhs._real) && (lhs._imaginary == rhs._imaginary))
                return true;
            else
                return false;
        }

        public static bool operator !=(Complex lhs, Complex rhs)
        {
            if ((lhs._real != rhs._real) || (lhs._imaginary != rhs._imaginary))
                 return true;
            else
                return false;
        }

       
        public override int GetHashCode()
        {
            return _real.GetHashCode() ^ _imaginary.GetHashCode();
        }

        public bool IsZero
        {
            get
            {
                return _real == 0.0 && _imaginary == 0.0;
            }
        }


        #region Operations
        public static Complex operator +(Complex a, Complex b)
        {
            return new Complex(a._real + b._real, a._imaginary + b._imaginary);
        }

        public static Complex operator -(Complex a, Complex b)
        {
            return new Complex(a._real - b._real, a._imaginary - b._imaginary);
        }

        public static Complex operator *(Complex a, Complex b)
        {
            return new Complex(a._real * b._real - a._imaginary * b._imaginary, a._real * b._imaginary + a._imaginary * b._real);
        }

        public static Complex operator /(Complex a, Complex b)
        {
            if (b.IsZero)
            {
                throw new DivideByZeroException("complex division by zero");
            }

            double real, imag, den, r;

            if (System.Math.Abs(b._real) >= System.Math.Abs(b._imaginary))
            {
                r = b._imaginary / b._real;
                den = b._real + r * b._imaginary;
                real = (a._real + a._imaginary * r) / den;
                imag = (a._imaginary - a._real * r) / den;
            }
            else
            {
                r = b._real / b._imaginary;
                den = b._imaginary + r * b._real;
                real = (a._real * r + a._imaginary) / den;
                imag = (a._imaginary * r - a._real) / den;
            }

            return new Complex(real, imag);
        }


        public static Complex operator /(double a, Complex b)
        {
            var result = new Complex(a, 0) / b;
            return result;
        }

        public static Complex operator /(Complex a, double b)
        {
            var result = a / new Complex(b, 0);
            return result;
        }

        public static Complex operator *(double a, Complex b)
        {
            var result = new Complex(a, 0) * b;
            return result;
        }

        public static Complex operator *(Complex a, double b)
        {
            var result = a * new Complex(b, 0);
            return (result);
        }



        public Complex Power(Complex y)
        {
            double c = y._real;
            double d = y._imaginary;
            int power = (int)c;

            if (power == c && power >= 0 && d == .0)
            {
                Complex result = One;
                if (power == 0) return result;
                Complex factor = this;
                while (power != 0)
                {
                    if ((power & 1) != 0)
                    {
                        result = result * factor;
                    }
                    factor = factor * factor;
                    power >>= 1;
                }
                return result;
            }
            else if (IsZero)
            {
                return y.IsZero ? One : Zero;
            }
            else
            {
                double a = _real;
                double b = _imaginary;
                double powers = a * a + b * b;
                double arg = System.Math.Atan2(b, a);
                double mul = System.Math.Pow(powers, c / 2) * System.Math.Exp(-d * arg);
                double common = c * arg + .5 * d * System.Math.Log(powers);
                return new Complex(mul * System.Math.Cos(common), mul * System.Math.Sin(common));
            }
        }


        public static Complex Pow(Complex a, Complex power)
        {
            return a.Power(power);
        }        
        
        
        public static Complex Pow(Complex a, double power)
        {
            var result = a.Power(new Complex(power, 0));
            return (result);
        }
        



        #endregion
    }
}
