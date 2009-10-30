using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities
{
    public class Stress<T> : DerivedQuantity<T>
    {
        public Stress()
            : base(1, new Force<T>(), new Area<T>(-1))
        {
        }

        public Stress(float exponent)
            : base(exponent, new Force<T>(exponent), new Area<T>(-1 * exponent))
        {
        }


        public static implicit operator Stress<T>(T value)
        {
            Stress<T> Q = new Stress<T>();

            Q.Value = value;

            return Q;
        }


    }
}
