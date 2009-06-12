using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuantitySystem.DimensionDescriptors
{
    public struct MassDescriptor : IDimensionDescriptor<MassDescriptor>
    {

        public MassDescriptor(int exponent):this()
        {
            this.Exponent = exponent;
        }


        #region IDimensionDescriptor<MassDescriptor> Members
        public int Exponent
        {
            get;
            set;
        }




        public MassDescriptor Add(MassDescriptor dimensionDescriptor)
        {
            MassDescriptor desc = new MassDescriptor();
            desc.Exponent = this.Exponent + dimensionDescriptor.Exponent;
            return desc;
        }

        public MassDescriptor Subtract(MassDescriptor dimensionDescriptor)
        {
            MassDescriptor desc = new MassDescriptor();
            desc.Exponent = this.Exponent - dimensionDescriptor.Exponent;
            return desc;
        }

        public MassDescriptor Multiply(int exponent)
        {
            MassDescriptor desc = new MassDescriptor();
            desc.Exponent = this.Exponent * exponent;
            return desc;
        }

        public MassDescriptor Invert()
        {
            MassDescriptor l = new MassDescriptor();
            l.Exponent = 0 - Exponent;

            return l;
        }

        #endregion
    }
}
