using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuantitySystem.DimensionDescriptors
{
    public struct AmountOfSubstanceDescriptor : IDimensionDescriptor<AmountOfSubstanceDescriptor>
    {

        public AmountOfSubstanceDescriptor(int exponent):this()
        {
            this.Exponent = exponent;
        }

        #region IDimensionDescriptor<AmountOfSubstanceDescriptor> Members

        public int Exponent
        {
            get;
            set;
        }

        public AmountOfSubstanceDescriptor Add(AmountOfSubstanceDescriptor dimensionDescriptor)
        {
            AmountOfSubstanceDescriptor desc = new AmountOfSubstanceDescriptor();
            desc.Exponent = this.Exponent + dimensionDescriptor.Exponent;
            return desc;

        }

        public AmountOfSubstanceDescriptor Subtract(AmountOfSubstanceDescriptor dimensionDescriptor)
        {
            AmountOfSubstanceDescriptor desc = new AmountOfSubstanceDescriptor();
            desc.Exponent = this.Exponent -  dimensionDescriptor.Exponent;
            return desc;
        }

        public AmountOfSubstanceDescriptor Multiply(int exponent)
        {
            AmountOfSubstanceDescriptor desc = new AmountOfSubstanceDescriptor();
            desc.Exponent = this.Exponent * exponent;
            return desc;
        }

        

        public AmountOfSubstanceDescriptor Invert()
        {
            AmountOfSubstanceDescriptor l = new AmountOfSubstanceDescriptor();
            l.Exponent = 0 - Exponent;
            
            return l;
        }

        #endregion
    }
}
