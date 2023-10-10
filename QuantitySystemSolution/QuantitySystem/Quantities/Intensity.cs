using QuantitySystem.Quantities.BaseQuantities;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuantitySystem.Quantities
{
    public class Intensity<T> : DerivedQuantity<T>
    {
        public Intensity()
            : base(1, new Mass<T>(), new SpeedRate<T>())
        {
        }

        public Intensity(float exponent)
            : base(exponent, new Mass<T>(exponent), new SpeedRate<T>(exponent))
        {
        }

        public static implicit operator Intensity<T>(T value)
        {
            Intensity<T> Q = new Intensity<T>();

            Q.Value = value;

            return Q;
        }

    }
}
