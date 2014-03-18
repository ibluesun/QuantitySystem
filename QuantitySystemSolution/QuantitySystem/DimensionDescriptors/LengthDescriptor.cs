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
            this.RegularExponent = normalExponent;
            this.PolarExponent = radiusExponent;
        }

        #region Length Properties Types

        public float RegularExponent
        {
            get;
            set;
        }

        public float PolarExponent
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
                    if (this.RegularExponent != ld.RegularExponent) return false;

                    if (this.PolarExponent != ld.PolarExponent) return false;

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
            return RegularExponent.GetHashCode() ^ PolarExponent.GetHashCode();
        }

        #region IDimensionDescriptor<LengthDescriptor> Members


        public float Exponent
        {
            get { return RegularExponent + PolarExponent; }
            set { }
        }



        public LengthDescriptor Add(LengthDescriptor dimensionDescriptor)
        {
            LengthDescriptor l = new LengthDescriptor();
            l.RegularExponent = this.RegularExponent + dimensionDescriptor.RegularExponent;
            l.PolarExponent = this.PolarExponent + dimensionDescriptor.PolarExponent;

            return l;
        }

        public LengthDescriptor Subtract(LengthDescriptor dimensionDescriptor)
        {
            LengthDescriptor l = new LengthDescriptor();
            l.RegularExponent = this.RegularExponent - dimensionDescriptor.RegularExponent;
            l.PolarExponent = this.PolarExponent - dimensionDescriptor.PolarExponent;

            return l;
        }

        public LengthDescriptor Multiply(float exponent)
        {
            LengthDescriptor l = new LengthDescriptor();
            l.RegularExponent = this.RegularExponent * exponent;
            l.PolarExponent = this.PolarExponent * exponent;

            return l;
        }

        public LengthDescriptor Invert()
        {
            LengthDescriptor l = new LengthDescriptor();
            l.RegularExponent = 0 - RegularExponent;
            l.PolarExponent = 0 - PolarExponent;
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
