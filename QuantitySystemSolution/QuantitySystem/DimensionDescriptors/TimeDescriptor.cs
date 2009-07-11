using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuantitySystem.DimensionDescriptors
{
    public struct TimeDescriptor : IDimensionDescriptor<TimeDescriptor>
    {


        public TimeDescriptor(float exponent):this()
        {
            this.Exponent = exponent;
        }

        #region IDimensionDescriptor<TimeDescriptor> Members
        public float Exponent
        {
            get;
            set;
        }

        public TimeDescriptor Add(TimeDescriptor dimensionDescriptor)
        {
            TimeDescriptor desc = new TimeDescriptor();
            desc.Exponent = this.Exponent + dimensionDescriptor.Exponent;
            return desc;
        }

        public TimeDescriptor Subtract(TimeDescriptor dimensionDescriptor)
        {
            TimeDescriptor desc = new TimeDescriptor();
            desc.Exponent = this.Exponent - dimensionDescriptor.Exponent;
            return desc;
        }

        public TimeDescriptor Multiply(float exponent)
        {
            TimeDescriptor desc = new TimeDescriptor();
            desc.Exponent = this.Exponent * exponent;
            return desc;
        }

        public TimeDescriptor Invert()
        {
            TimeDescriptor l = new TimeDescriptor();
            l.Exponent = 0 - Exponent;

            return l;
        }

        #endregion
    }
}
