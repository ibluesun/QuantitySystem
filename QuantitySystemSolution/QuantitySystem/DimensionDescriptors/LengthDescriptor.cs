using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuantitySystem.DimensionDescriptors
{
    public struct LengthDescriptor : IDimensionDescriptor<LengthDescriptor>
    {

        public LengthDescriptor(float normalExponent, float radiusExponent):this()
        {
            this.NormalExponent = normalExponent;
            this.RadiusExponent = radiusExponent;
        }

        #region Length Properties Types

        public float NormalExponent
        {
            get;
            set;
        }

        public float RadiusExponent
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

        public override int GetHashCode()
        {
            return NormalExponent.GetHashCode() ^ RadiusExponent.GetHashCode();
        }

        #region IDimensionDescriptor<LengthDescriptor> Members


        public float Exponent
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

        public LengthDescriptor Multiply(float exponent)
        {
            LengthDescriptor l = new LengthDescriptor();
            l.NormalExponent = this.NormalExponent * exponent;
            l.RadiusExponent = this.RadiusExponent * exponent;

            return l;
        }

        public LengthDescriptor Invert()
        {
            LengthDescriptor l = new LengthDescriptor();
            l.NormalExponent = 0 - NormalExponent;
            l.RadiusExponent = 0 - RadiusExponent;
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
