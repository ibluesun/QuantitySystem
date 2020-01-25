using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuantitySystem.DimensionDescriptors
{
    public struct LengthDescriptor : IDimensionDescriptor<LengthDescriptor>
    {

        public LengthDescriptor(float scalarExponent, float vectorExponent):this()
        {
            this.ScalarExponent = scalarExponent;
            this.VectorExponent = vectorExponent;
        }

        #region Length Properties Types

        public float ScalarExponent
        {
            get;
            set;
        }

        public float VectorExponent
        {
            get;
            set;
        }

        #endregion


        public override bool Equals(object obj)
        {
            try
            {
                LengthDescriptor ld = (LengthDescriptor)obj;
                {
                    if (this.ScalarExponent != ld.ScalarExponent) return false;

                    if (this.VectorExponent != ld.VectorExponent) return false;

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return ScalarExponent.GetHashCode() ^ VectorExponent.GetHashCode();
        }

        #region IDimensionDescriptor<LengthDescriptor> Members


        public float Exponent
        {
            get { return ScalarExponent + VectorExponent; }
            set { }
        }



        public LengthDescriptor Add(LengthDescriptor dimensionDescriptor)
        {
            LengthDescriptor l = new LengthDescriptor();
            l.ScalarExponent = this.ScalarExponent + dimensionDescriptor.ScalarExponent;
            l.VectorExponent = this.VectorExponent + dimensionDescriptor.VectorExponent;

            return l;
        }

        public LengthDescriptor Subtract(LengthDescriptor dimensionDescriptor)
        {
            LengthDescriptor l = new LengthDescriptor();
            l.ScalarExponent = this.ScalarExponent - dimensionDescriptor.ScalarExponent;
            l.VectorExponent = this.VectorExponent - dimensionDescriptor.VectorExponent;

            return l;
        }

        public LengthDescriptor Multiply(float exponent)
        {
            LengthDescriptor l = new LengthDescriptor();
            l.ScalarExponent = this.ScalarExponent * exponent;

            var ve = this.VectorExponent;
            if (exponent == 0.5)
            {
                while (ve > 1)
                {
                    ve = ve - 2;
                    l.ScalarExponent = l.ScalarExponent + 1;
                }

            }

            l.VectorExponent = ve * exponent;

            //if (this.VectorExponent == 2 && exponent == 0.5)
            //{
            //    // getting square root of vector exponent should return scalar
            //    l.ScalarExponent = l.ScalarExponent  + 1;    // add the vector sqrt to the scalar component.
            //}
            //else
            //{
            //    l.VectorExponent = this.VectorExponent * exponent;
            //}


            return l;
        }

        public LengthDescriptor Invert()
        {
            LengthDescriptor l = new LengthDescriptor();
            l.ScalarExponent = 0 - ScalarExponent;
            l.VectorExponent = 0 - VectorExponent;
            return l;
        }


        #endregion


        #region Helper Instantiators
        public static LengthDescriptor NormalLength(int exponent)
        {

            return new LengthDescriptor(exponent, 0);

        }
        public static LengthDescriptor RadiusLength(int exponent)
        {

            return new LengthDescriptor(0, exponent);

        }
        #endregion
    }
}
