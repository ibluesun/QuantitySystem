using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.Quantities.BaseQuantities;


namespace QuantitySystem.Quantities
{
    public class Force<T> : DerivedQuantity<T>
    {
        public Force()
            : base(1, new Mass<T>(), new Acceleration<T>())
        {
        }

        public Force(float exponent)
            : base(exponent, new Mass<T>(exponent), new Acceleration<T>(exponent))
        {
        }

        public static implicit operator Force<T>(T value)
        {
            Force<T> Q = new Force<T>();

            Q.Value = value;

            return Q;
        }

    }
}
