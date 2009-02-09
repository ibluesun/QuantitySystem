using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuantitySystemTestingProject
{
    /// <summary>
    /// Complex Number Struct used as a sample for testing 
    /// the container value of the quantity.
    /// </summary>
    public struct ComplexNumber
    {
        public double Real { get; set; }
        public double Imaginary { get; set; }

        public override string ToString()
        {
            return Real + " + " + Imaginary + "i";
        }

        public static ComplexNumber operator +(ComplexNumber left, ComplexNumber right)
        {
            ComplexNumber cplx = new ComplexNumber();
            cplx.Real = left.Real + right.Real;
            cplx.Imaginary = left.Imaginary + right.Imaginary;

            return cplx;
        }

        public static ComplexNumber operator +(ComplexNumber left, int right)
        {
            ComplexNumber cplx = new ComplexNumber();
            cplx.Real = left.Real + right;
            cplx.Imaginary = left.Imaginary;

            return cplx;
        }

        public static ComplexNumber operator -(ComplexNumber left, ComplexNumber right)
        {
            ComplexNumber cplx = new ComplexNumber();
            cplx.Real = left.Real - right.Real;
            cplx.Imaginary = left.Imaginary - right.Imaginary;

            return cplx;

        }


        public static ComplexNumber operator *(ComplexNumber left, ComplexNumber right)
        {
            ComplexNumber cplx = new ComplexNumber();
            cplx.Real = left.Real * right.Real;
            cplx.Imaginary = left.Imaginary * right.Imaginary;

            return cplx;

        }

        public static ComplexNumber operator /(ComplexNumber left, ComplexNumber right)
        {
            ComplexNumber cplx = new ComplexNumber();
            cplx.Real = left.Real / right.Real;
            cplx.Imaginary = left.Imaginary / right.Imaginary;

            return cplx;

        }

        public static ComplexNumber operator /(double f, ComplexNumber c)
        {
            ComplexNumber cplx = new ComplexNumber();

            cplx.Real = f / c.Real;
            cplx.Imaginary = f / c.Imaginary;

            return cplx;
        }

        public override bool Equals(object obj)
        {
            if (this.GetType() == obj.GetType())
            {
                ComplexNumber cn = (ComplexNumber)obj;

                if (cn.Imaginary == this.Imaginary
                    &&
                    cn.Real == this.Real
                    )
                {
                    return true;

                }
                else
                {
                    return false;
                }

            }
            else return false;
        }
    }
}
