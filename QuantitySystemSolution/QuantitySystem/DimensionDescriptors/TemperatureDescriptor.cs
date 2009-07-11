using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuantitySystem.DimensionDescriptors
{
    public struct TemperatureDescriptor : IDimensionDescriptor<TemperatureDescriptor>
    {


        public TemperatureDescriptor(float exponent):this()
        {
            this.Exponent = exponent;
        }

        #region IDimensionDescriptor<TemperatureDescriptor> Members

        public float Exponent
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

        public TemperatureDescriptor Multiply(float exponent)
        {
            TemperatureDescriptor desc = new TemperatureDescriptor();
            desc.Exponent = this.Exponent * exponent;
            return desc;
        }

        public TemperatureDescriptor Invert()
        {
            TemperatureDescriptor l = new TemperatureDescriptor();
            l.Exponent = 0 - Exponent;

            return l;
        }


        #endregion
    }
}
