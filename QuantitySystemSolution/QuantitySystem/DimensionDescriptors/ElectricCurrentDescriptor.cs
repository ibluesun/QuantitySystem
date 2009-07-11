using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuantitySystem.DimensionDescriptors
{
    public struct ElectricCurrentDescriptor : IDimensionDescriptor<ElectricCurrentDescriptor>
    {

        public ElectricCurrentDescriptor(float exponent):this()
        {
            this.Exponent = exponent;
        }

        #region IDimensionDescriptor<ElectricCurrentDescriptor> Members
        public float Exponent
        {
            get;
            set;
        }

        public ElectricCurrentDescriptor Add(ElectricCurrentDescriptor dimensionDescriptor)
        {
            ElectricCurrentDescriptor desc = new ElectricCurrentDescriptor();
            desc.Exponent = this.Exponent+ dimensionDescriptor.Exponent;
            return desc;
        }

        public ElectricCurrentDescriptor Subtract(ElectricCurrentDescriptor dimensionDescriptor)
        {
            ElectricCurrentDescriptor desc = new ElectricCurrentDescriptor();
            desc.Exponent = this.Exponent - dimensionDescriptor.Exponent;
            return desc;
        }

        public ElectricCurrentDescriptor Multiply(float exponent)
        {
            ElectricCurrentDescriptor desc = new ElectricCurrentDescriptor();
            desc.Exponent = this.Exponent * exponent;
            return desc;
        }

        public ElectricCurrentDescriptor Invert()
        {
            ElectricCurrentDescriptor l = new ElectricCurrentDescriptor();
            l.Exponent = 0 - Exponent;

            return l;
        }
        #endregion
    }
}
