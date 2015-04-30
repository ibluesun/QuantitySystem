using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuantitySystem.DimensionDescriptors
{
    public struct DigitalDescriptor : IDimensionDescriptor<DigitalDescriptor>
    {

        public DigitalDescriptor(float exponent)
            : this()
        {
            this.Exponent = exponent;
        }

        #region IDimensionDescriptor<InformationDescriptor> Members

        public float Exponent
        {
            get;
            set;
        }

        public DigitalDescriptor Add(DigitalDescriptor dimensionDescriptor)
        {
            DigitalDescriptor desc = new DigitalDescriptor();
            desc.Exponent = this.Exponent + dimensionDescriptor.Exponent;
            return desc;
        }

        public DigitalDescriptor Subtract(DigitalDescriptor dimensionDescriptor)
        {
            DigitalDescriptor desc = new DigitalDescriptor();
            desc.Exponent = this.Exponent - dimensionDescriptor.Exponent;
            return desc;
        }

        public DigitalDescriptor Multiply(float exponent)
        {
            DigitalDescriptor desc = new DigitalDescriptor();
            desc.Exponent = this.Exponent * exponent;
            return desc;
        }

        public DigitalDescriptor Invert()
        {
            DigitalDescriptor l = new DigitalDescriptor();
            l.Exponent = 0 - Exponent;
            return l;
        }

        #endregion
    }
}
