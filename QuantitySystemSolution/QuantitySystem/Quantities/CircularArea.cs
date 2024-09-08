using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities
{
    public class CircularArea<T> : DerivedQuantity<T>
    {
        public CircularArea()
            : base(1, new Length<T>(1), new Displacement<T>(1))
        {
        }

        public CircularArea(float exponent)
            : base(exponent, new Length<T>(exponent), new Displacement<T>(exponent))
        {
        }


        public static implicit operator CircularArea<T>(T value)
        {
            CircularArea<T> Q = new CircularArea<T>();

            Q.Value = value;

            return Q;
        }


    }
}
