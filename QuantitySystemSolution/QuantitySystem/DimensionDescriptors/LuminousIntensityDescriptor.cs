using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuantitySystem.DimensionDescriptors
{
    public struct LuminousIntensityDescriptor : IDimensionDescriptor<LuminousIntensityDescriptor>
    {

        public LuminousIntensityDescriptor(int exponent):this()
        {
            this.Exponent = exponent;
        }


        #region IDimensionDescriptor<LuminousIntensityDescriptor> Members


        public int Exponent
        {
            get;
            set;
        }




        public LuminousIntensityDescriptor Add(LuminousIntensityDescriptor dimensionDescriptor)
        {
            LuminousIntensityDescriptor desc = new LuminousIntensityDescriptor();
            desc.Exponent  = this.Exponent + dimensionDescriptor.Exponent;
            return desc;
        }

        public LuminousIntensityDescriptor Subtract(LuminousIntensityDescriptor dimensionDescriptor)
        {
            LuminousIntensityDescriptor desc = new LuminousIntensityDescriptor();
            desc.Exponent = this.Exponent - dimensionDescriptor.Exponent;
            return desc;
        }

        public LuminousIntensityDescriptor Multiply(int exponent)
        {
            LuminousIntensityDescriptor desc = new LuminousIntensityDescriptor();
            desc.Exponent = this.Exponent * exponent;
            return desc;
        }

        #endregion
    }
}
