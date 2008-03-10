using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuantitySystem.DimensionDescriptors
{
    public struct LengthDescriptor : IDimensionDescriptor<LengthDescriptor>
    {

        public LengthDescriptor(int normalExponent, int radiusExponent):this()
        {
            this.NormalExponent = normalExponent;
            this.RadiusExponent = radiusExponent;
        }

        #region Length Properties Types

        public int NormalExponent
        {
            get;
            set;
        }

        public int RadiusExponent
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
                    if (this.NormalExponent != ld.NormalExponent) return false;

                    if (this.RadiusExponent != ld.RadiusExponent) return false;

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }


        #region IDimensionDescriptor<LengthDescriptor> Members


        public int Exponent
        {
            get { return NormalExponent + RadiusExponent; }
            set { }
        }



        public LengthDescriptor Add(LengthDescriptor dimensionDescriptor)
        {
            LengthDescriptor l = new LengthDescriptor();
            l.NormalExponent = this.NormalExponent + dimensionDescriptor.NormalExponent;
            l.RadiusExponent = this.RadiusExponent + dimensionDescriptor.RadiusExponent;

            return l;
        }

        public LengthDescriptor Subtract(LengthDescriptor dimensionDescriptor)
        {
            LengthDescriptor l = new LengthDescriptor();
            l.NormalExponent = this.NormalExponent - dimensionDescriptor.NormalExponent;
            l.RadiusExponent = this.RadiusExponent - dimensionDescriptor.RadiusExponent;

            return l;
        }

        public LengthDescriptor Multiply(int exponent)
        {
            LengthDescriptor l = new LengthDescriptor();
            l.NormalExponent = this.NormalExponent * exponent;
            l.RadiusExponent = this.RadiusExponent * exponent;

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
