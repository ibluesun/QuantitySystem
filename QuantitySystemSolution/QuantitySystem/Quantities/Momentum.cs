using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities
{
    public class Momentum<T> : DerivedQuantity<T>
    {
        public Momentum()
            : base(1, new Mass<T>(), new Speed<T>())
        {
        }

        public Momentum(float exponent)
            : base(exponent, new Mass<T>(exponent), new Speed<T>(exponent))
        {
        }


        public static implicit operator Momentum<T>(T value)
        {
            Momentum<T> Q = new Momentum<T>();

            Q.Value = value;

            return Q;
        }

    }
}

