using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities
{
    public class Stiffness<T> : DerivedQuantity<T>
    {
        public Stiffness()
            : base(1, new Force<T>(), new Length<T>(-1))
        {
        }

        public Stiffness(float exponent)
            : base(exponent, new Force<T>(exponent), new Length<T>(-1 * exponent))
        {
        }


        public static implicit operator Stiffness<T>(T value)
        {
            Stiffness<T> Q = new Stiffness<T>();

            Q.Value = value;

            return Q;
        }

    }
}
