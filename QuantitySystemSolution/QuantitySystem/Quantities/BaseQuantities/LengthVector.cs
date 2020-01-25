using System;
using System.Collections.Generic;
using System.Text;

namespace QuantitySystem.Quantities.BaseQuantities
{


    /// <summary>
    /// The Length but in vector mode
    /// very usefull in differentiating of anlges and Angular quantities in general.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LengthVector<T> : Length<T>
    {
        public LengthVector() : base(1)
        {
            QuantityType = QuantityType.Vector;
        }

        public LengthVector(float exponent) : base(exponent)
        {
            QuantityType = QuantityType.Vector;
        }

    }
}
