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
    public class Displacement<T> : Length<T>
    {
        public Displacement() : base(1)
        {
            LengthRank = DimensionTensorRank.Vector;
        }

        public Displacement(float exponent) : base(exponent)
        {
            LengthRank = DimensionTensorRank.Vector;
        }

    }
}
