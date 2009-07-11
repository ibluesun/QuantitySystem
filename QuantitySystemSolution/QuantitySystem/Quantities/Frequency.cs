using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities
{

    public class Frequency<T> : DerivedQuantity<T>
    {

        public Frequency()
            : base(1, new Time<T>(-1))
        {
        }


        public Frequency(float exponent)
            : base(exponent, new Time<T>(-1 * exponent))
        {
        }

        public static implicit operator Frequency<T>(T value)
        {
            Frequency<T> Q = new Frequency<T>();

            Q.Value = value;

            return Q;
        }
    }
}
