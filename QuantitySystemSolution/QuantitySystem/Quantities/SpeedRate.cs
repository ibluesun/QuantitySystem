using QuantitySystem.Quantities.BaseQuantities;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuantitySystem.Quantities
{
    public class SpeedRate<T> : DerivedQuantity<T>
    {
        public SpeedRate()
        : base(1, new Speed<T>(), new Time<T>(-1))
        {
        }

        public SpeedRate(float exponent)
            : base(exponent, new Speed<T>(exponent), new Time<T>(-1 * exponent))
        {
        }


        public static implicit operator SpeedRate<T>(T value)
        {
            SpeedRate<T> Q = new SpeedRate<T>();

            Q.Value = value;

            return Q;
        }
    }
}
