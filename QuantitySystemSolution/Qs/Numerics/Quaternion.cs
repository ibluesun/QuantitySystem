﻿

//  Source code is translated from boost C++ to C# 
//  Translator: Ahmed Sadek


//  boost quaternion.hpp header file

//  (C) Copyright Hubert Holin 2001.
//  Distributed under the Boost Software License, Version 1.0. (See
//  accompanying file LICENSE_1_0.txt or copy at
//  http://www.boost.org/LICENSE_1_0.txt)

// See http://www.boost.org for updates, documentation, and revision history.



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using Qs.Types;

namespace Qs.Numerics
{
    /// <summary>
    /// Qs Quaternion storage type
    /// </summary>
    public struct Quaternion : IEquatable<Quaternion>
    {
        private double a;
        private double b;
        private double c;
        private double d;

        public double Real
        {
            get
            {
                return a;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        public double i => b;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        public double j => c;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        public double k => d;

        /// <summary>
        /// Quaternion constructor
        /// </summary>
        /// <param name="real"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="k"></param>
        public Quaternion(double real = 0, double i = 0, double j = 0, double k = 0)
        {
            a = real;
            b = i;
            c = j;
            d = k;
        }

        public Quaternion(Quaternion quaternion)
        {
            a = quaternion.a;
            b = quaternion.b;
            c = quaternion.c;
            d = quaternion.d;
        }

        public static implicit operator Quaternion(double d)
        {
            Quaternion dd = new Quaternion(d);
            return dd;
        }

        public static implicit operator Quaternion(Complex d)
        {
            Quaternion dd = new Quaternion(d.Real, d.Imaginary);
            return dd;
        }


        public override string ToString()
        {
            return "(" 
                + a.ToString(CultureInfo.InvariantCulture) + ", " 
                + b.ToString(CultureInfo.InvariantCulture)
                + "i, " 
                + c.ToString(CultureInfo.InvariantCulture) 
                + "j, " 
                + d.ToString(CultureInfo.InvariantCulture) + "k )";
        }


        public string ToQsSyntax()
        {
            
                return "H{ "
                + a.ToString(CultureInfo.InvariantCulture) + ", "
                + b.ToString(CultureInfo.InvariantCulture) + ", "
                + c.ToString(CultureInfo.InvariantCulture) + ", "
                + d.ToString(CultureInfo.InvariantCulture) + " }";
            
        }

        #region The code is based on http://www.boost.org/doc/libs/1_43_0/boost/math/quaternion.hpp



        public static Quaternion operator +(Quaternion lhs, double rhs)
        {
            Quaternion result = new Quaternion(lhs);
            result.a += rhs;
            return result;
        }

        public static Quaternion operator +(Quaternion lhs, Complex rhs)
        {
            Quaternion result = new Quaternion(lhs);
            result.a += rhs.Real;
            result.b += rhs.Imaginary;
            return result;
        }

        public static Quaternion operator +(Quaternion lhs, Quaternion rhs)
        {
            Quaternion result = new Quaternion(lhs);
            result.a += rhs.a;
            result.b += rhs.b;
            result.c += rhs.c;
            result.d += rhs.d;
            return result;
        }

        public static Quaternion operator -(Quaternion lhs, double rhs)
        {
            Quaternion result = new Quaternion(lhs);
            result.a -= rhs;
            return result;
        }

        public static Quaternion operator -(Quaternion lhs, Complex rhs)
        {
            Quaternion result = new Quaternion(lhs);
            result.a -= rhs.Real;
            result.b -= rhs.Imaginary;
            return result;
        }

        public static Quaternion operator -(Quaternion lhs, Quaternion rhs)
        {
            Quaternion result = new Quaternion(lhs);
            result.a -= rhs.a;
            result.b -= rhs.b;
            result.c -= rhs.c;
            result.d -= rhs.d;
            return result;
        }




        public static Quaternion operator *(Quaternion lhs, double rhs)
        {
            Quaternion result = new Quaternion(lhs);
            result.a *= rhs;
            result.b *= rhs;
            result.c *= rhs;
            result.d *= rhs;
            return result;
        }

        public static Quaternion operator *(Quaternion lhs, Complex rhs)
        {

            double ar = rhs.Real;
            double br = rhs.Imaginary;

            double at = lhs.a * ar - lhs.b * br;
            double bt = lhs.a * br + lhs.b * ar;
            double ct = lhs.c * ar + lhs.d * br;
            double dt = -lhs.c * br + lhs.d * ar;                                             
                                                                                 
            Quaternion result = new Quaternion(at, bt, ct, dt);
            return result;
        }

        public static Quaternion operator *(Quaternion lhs, Quaternion rhs)
        {

            double ar = (rhs.a);
            double br = (rhs.b);
            double cr = (rhs.c);
            double dr = (rhs.d);

            double at = lhs.a * ar - lhs.b * br - lhs.c * cr - lhs.d * dr;
            double bt = lhs.a * br + lhs.b * ar + lhs.c * dr - lhs.d * cr;
            double ct = lhs.a * cr - lhs.b * dr + lhs.c * ar + lhs.d * br;
            double dt = lhs.a * dr + lhs.b * cr - lhs.c * br + lhs.d * ar;

            Quaternion result = new Quaternion(at, bt, ct, dt);
            
            return result;
        }

        public static Quaternion operator /(Quaternion lhs, double rhs)
        {
            Quaternion result = new Quaternion(lhs);
            result.a /= rhs;
            result.b /= rhs;
            result.c /= rhs;
            result.d /= rhs;
            return result;
        }

        public static Quaternion operator /(Quaternion lhs, Complex rhs)
        {

            double ar = rhs.Real;
            double br = rhs.Imaginary;

            double denominator = ar * ar + br * br;

            double at = (lhs.a * ar + lhs.b * br) / denominator;    //(a*ar+b*br)/denominator;
            double bt = (-lhs.a * br + lhs.b * ar) / denominator;    //(ar*b-a*br)/denominator;
            double ct = (lhs.c * ar - lhs.d * br) / denominator;    //(ar*c-d*br)/denominator;
            double dt = (lhs.c * br + lhs.d * ar) / denominator;    //(ar*d+br*c)/denominator;

            Quaternion result = new Quaternion(at, bt, ct, dt);
            return result;
        }

        public static Quaternion operator /(Quaternion lhs, Quaternion rhs)
        {

            double ar = (rhs.a);
            double br = (rhs.b);
            double cr = (rhs.c);
            double dr = (rhs.d);

            double denominator = ar * ar + br * br + cr * cr + dr * dr;

            double at = (lhs.a * ar + lhs.b * br + lhs.c * cr + lhs.d * dr) / denominator;    //(a*ar+b*br+c*cr+d*dr)/denominator;
            double bt = (-lhs.a * br + lhs.b * ar - lhs.c * dr + lhs.d * cr) / denominator;    //((ar*b-a*br)+(cr*d-c*dr))/denominator;
            double ct = (-lhs.a * cr + lhs.b * dr + lhs.c * ar - lhs.d * br) / denominator;    //((ar*c-a*cr)+(dr*b-d*br))/denominator;
            double dt = (-lhs.a * dr - lhs.b * cr + lhs.c * br + lhs.d * ar) / denominator;    //((ar*d-a*dr)+(br*c-b*cr))/denominator;
                

            Quaternion result = new Quaternion(at, bt, ct, dt);

            return result;
        }


        #endregion


        public static Quaternion operator /(double a, Quaternion b)
        {
            return new Quaternion(a) / b;
        }

        public static Quaternion operator *(double a, Quaternion b)
        {
            return new Quaternion(a) * b;
        }


        public static Quaternion Pow(Quaternion q, double nn)
        {
            int n = (int)nn;

            if(n > 1)
            {
                int    m = n>>1;

                Quaternion result = Pow(q, m);
                
                result *= result;
                
                if    (n != (m<<1))
                {
                    result *= q; // n odd
                }
                
                return(result);
            }
            else if    (n == 1)
            {
                return(q);
            }
            else if    (n == 0)
            {
                return (new Quaternion(1));
            }
            else    /* n < 0 */
            {
                return (Pow(new Quaternion(1) / q, -n));
            }
        }



        public static bool operator ==(Quaternion lhs, Quaternion rhs)
        {
            if (lhs.a == rhs.a &&
                lhs.b == rhs.b &&
                lhs.c == rhs.c &&
                lhs.d == rhs.d
                )
                return true;
            else
                return false;
        }

        public static bool operator !=(Quaternion lhs, Quaternion rhs)
        {
            return !(lhs == rhs);
        }

        public bool Equals(Quaternion other)
        {
            if (this.a == other.a &&
                this.b == other.b &&
                this.c == other.c &&
                this.d == other.d
                )
                return true;
            else
                return false;
        }
        public override bool Equals(object obj)
        {
            if (obj is Quaternion rhs)
                return Equals(rhs);
            return false;
        }

        public override int GetHashCode()
        {
            return a.GetHashCode() ^ b.GetHashCode() ^ c.GetHashCode() ^ d.GetHashCode();
        }


        /// <summary>
        /// 3x3 Rotation Matrix From normalized quaternion
        /// </summary>
        /// <returns></returns>
        public QsMatrix To_3x3_RotationMatrix()
        {
            
            var w = a;
            var x = i;
            var y = j;
            var z = k;

            var v0 = new QsVector();
            var v1 = new QsVector();
            var v2 = new QsVector();

            v0.AddComponent(1 - 2 * y * y - 2 * z * z);
            v0.AddComponent(2 * x * y - 2 * w * z);
            v0.AddComponent(2 * x * z + 2 * w * y);

            v1.AddComponent(2 * x * y + 2 * w * z);
            v1.AddComponent(1 - 2 * x * x - 2 * z * z);
            v1.AddComponent(2 * y * z - 2 * w * x);

            v2.AddComponent(2 * x * z - 2 * w * y);
            v2.AddComponent(2 * y * z + 2 * w * x);
            v2.AddComponent(1 - 2 * x * x - 2 * y * y);

            var rm = new QsMatrix(v0, v1, v2);
            return rm;

        }
    }
}
