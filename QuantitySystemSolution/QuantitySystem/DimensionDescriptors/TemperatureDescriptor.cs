using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuantitySystem.DimensionDescriptors
{
    public struct TemperatureDescriptor : IDimensionDescriptor<TemperatureDescriptor>
    {


        public TemperatureDescriptor(int exponent):this()
        {
            this.Exponent = exponent;
        }

        #region IDimensionDescriptor<TemperatureDescriptor> Members

        public int Exponent
        {
            get;
            set;
        }




        public TemperatureDescriptor Add(TemperatureDescriptor dimensionDescriptor)
        {
            TemperatureDescriptor desc = new TemperatureDescriptor();
            desc.Exponent = this.Exponent + dimensionDescriptor.Exponent;
            return desc;
        }

        public TemperatureDescriptor Subtract(TemperatureDescriptor dimensionDescriptor)
        {
            TemperatureDescriptor desc = new TemperatureDescriptor();
            desc.Exponent = this.Exponent - dimensionDescriptor.Exponent;
            return desc;
        }

        public TemperatureDescriptor Multiply(int exponent)
        {
            TemperatureDescriptor desc = new TemperatureDescriptor();
            desc.Exponent = this.Exponent * exponent;
            return desc;
        }

        #endregion
    }
}
