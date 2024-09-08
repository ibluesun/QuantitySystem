using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuantitySystem.DimensionDescriptors
{
    public struct LengthDescriptor : IDimensionDescriptor<LengthDescriptor>
    {

        public LengthDescriptor(float scalarExponent, float vectorExponent, float matrixExponent, float vectorMatrixExponent, float matrixMatrixExponent):this()
        {
            this.ScalarExponent = scalarExponent;
            this.VectorExponent = vectorExponent;
            this.MatrixExponent = matrixExponent;
            this.VectorMatrixExponent = vectorMatrixExponent;
            this.MatrixMatrixExponent = matrixMatrixExponent;
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

        public float MatrixExponent
        {
            get;
            set;
        }

        public float VectorMatrixExponent
        {
            get;
            set;
        }

        public float MatrixMatrixExponent
        {
            get;
            set;
        }

        #endregion

        public bool IsHigherRank => VectorExponent != 0 | MatrixExponent != 0 | VectorMatrixExponent != 0 | MatrixMatrixExponent != 0;

        public override bool Equals(object obj)
        {
            try
            {
                LengthDescriptor ld = (LengthDescriptor)obj;
                {
                    if (this.ScalarExponent != ld.ScalarExponent) return false;

                    if (this.VectorExponent != ld.VectorExponent) return false;

                    if (this.MatrixExponent != ld.MatrixExponent) return false;

                    if (this.VectorMatrixExponent != ld.VectorMatrixExponent) return false;

                    if (this.MatrixMatrixExponent != ld.MatrixMatrixExponent) return false;


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
            return ScalarExponent.GetHashCode() ^ VectorExponent.GetHashCode() ^ MatrixExponent.GetHashCode() ^ VectorMatrixExponent.GetHashCode() ^ MatrixMatrixExponent.GetHashCode();
        }

        #region IDimensionDescriptor<LengthDescriptor> Members


        public float Exponent
        {
            get { return ScalarExponent + VectorExponent + MatrixExponent + VectorMatrixExponent + MatrixMatrixExponent; }
            set { }
        }



        public LengthDescriptor Add(LengthDescriptor dimensionDescriptor)
        {
            LengthDescriptor l = new LengthDescriptor();
            l.ScalarExponent = this.ScalarExponent + dimensionDescriptor.ScalarExponent;
            l.VectorExponent = this.VectorExponent + dimensionDescriptor.VectorExponent;
            l.MatrixExponent = this.MatrixExponent + dimensionDescriptor.MatrixExponent;
            l.VectorMatrixExponent = this.VectorMatrixExponent + dimensionDescriptor.VectorMatrixExponent;
            l.MatrixMatrixExponent = this.MatrixMatrixExponent + dimensionDescriptor.MatrixMatrixExponent;

            return l;
        }

        public LengthDescriptor Subtract(LengthDescriptor dimensionDescriptor)
        {
            LengthDescriptor l = new LengthDescriptor();
            l.ScalarExponent = this.ScalarExponent - dimensionDescriptor.ScalarExponent;
            l.VectorExponent = this.VectorExponent - dimensionDescriptor.VectorExponent;
            l.MatrixExponent = this.MatrixExponent - dimensionDescriptor.MatrixExponent;
            l.VectorMatrixExponent = this.VectorMatrixExponent - dimensionDescriptor.VectorMatrixExponent;
            l.MatrixMatrixExponent = this.MatrixMatrixExponent - dimensionDescriptor.MatrixMatrixExponent;

            return l;
        }

        public LengthDescriptor Multiply(float exponent)
        {
            LengthDescriptor l = new LengthDescriptor();
            l.ScalarExponent = this.ScalarExponent * exponent;
            l.VectorExponent = this.VectorExponent * exponent;

            l.MatrixExponent = this.MatrixExponent * exponent;
            l.VectorMatrixExponent = this.VectorMatrixExponent * exponent;
            l.MatrixMatrixExponent = this.MatrixMatrixExponent * exponent;

            return l;
        }

        public LengthDescriptor Invert()
        {
            LengthDescriptor l = new LengthDescriptor();
            l.ScalarExponent = 0 - ScalarExponent;
            l.VectorExponent = 0 - VectorExponent;

            l.MatrixExponent = 0 - MatrixExponent ;
            l.VectorMatrixExponent = 0 - VectorMatrixExponent ;
            l.MatrixMatrixExponent = 0 - MatrixMatrixExponent ;
            return l;
        }


        #endregion


        #region Helper Instantiators
        public static LengthDescriptor ScalarLength(int exponent)
        {

            return new LengthDescriptor(exponent, 0, 0, 0, 0);

        }
        public static LengthDescriptor VectorLength(int exponent)
        {

            return new LengthDescriptor(0, exponent, 0, 0, 0);

        }
        #endregion
    }
}
