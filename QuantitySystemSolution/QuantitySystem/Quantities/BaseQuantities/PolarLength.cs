using System;
using System.Collections.Generic;
using System.Text;

namespace QuantitySystem.Quantities.BaseQuantities
{


    /// <summary>
    /// The Length but in Polar mode
    /// very usefull in differentiating of anlges and Angular quantities in general.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PolarLength<T> : Length<T>
    {
        public PolarLength() : base(1)
        {
            LengthType = LengthType.Polar;
        }

        public PolarLength(float exponent) : base(exponent)
        {
            LengthType = LengthType.Polar;
        }

    }
}
