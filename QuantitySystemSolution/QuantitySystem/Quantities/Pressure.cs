using System;
using System.Collections.Generic;
using System.Text;

namespace QuantitySystem.Quantities
{
    public class Pressure<T> : DerivedQuantity<T>
    {
        public Pressure()
            : base(1, new Intensity<T>(), new Area<T>(-1))
        {
        }

        public Pressure(float exponent)
            : base(exponent, new Intensity<T>(exponent), new Area<T>(-1 * exponent))
        {
        }


        public static implicit operator Pressure<T>(T value)
        {
            Pressure<T> Q = new Pressure<T>();

            Q.Value = value;

            return Q;
        }


    }
}