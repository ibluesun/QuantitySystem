using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuantitySystem.DimensionDescriptors
{
    public struct TimeDescriptor : IDimensionDescriptor<TimeDescriptor>
    {


        public TimeDescriptor(int exponent):this()
        {
            this.Exponent = exponent;
        }

        #region IDimensionDescriptor<TimeDescriptor> Members
        public int Exponent
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

        public TimeDescriptor Multiply(int exponent)
        {
            TimeDescriptor desc = new TimeDescriptor();
            desc.Exponent = this.Exponent * exponent;
            return desc;
        }

        #endregion
    }
}
