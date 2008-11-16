using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities
{
    public class Acceleration<T> : DerivedQuantity<T>
    {
        public Acceleration()
            : base(1, new Velocity<T>(), new Time<T>(-1))
        {
        }

        public Acceleration(int exponent)
            : base(exponent, new Velocity<T>(exponent), new Time<T>(-1 * exponent))
        {
        }


        public static implicit operator Acceleration<T>(T value)
        {
            Acceleration<T> Q = new Acceleration<T>();

            Q.Value = value;

            return Q;
        }


    }
}
