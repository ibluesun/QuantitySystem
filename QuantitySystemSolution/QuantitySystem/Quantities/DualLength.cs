using QuantitySystem.Quantities.BaseQuantities;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuantitySystem.Quantities
{
    public class ContraLength<T> : DerivedQuantity<T>
    {
        public ContraLength()
        : base(1, new Area<T>(), new PolarLength<T>(-1))
        {
        }

        public ContraLength(float exponent)
            : base(exponent, new Area<T>(exponent), new PolarLength<T>(-1 * exponent))
        {
        }


        public static implicit operator ContraLength<T>(T value)
        {
            ContraLength<T> Q = new ContraLength<T>();

            Q.Value = value;

            return Q;
        }
    }


    public class CoLength<T> : DerivedQuantity<T>
    {
        public CoLength()
        : base(1, new PolarArea<T>(), new Length<T>(-1))
        {
        }

        public CoLength(float exponent)
            : base(exponent, new PolarArea<T>(exponent), new Length<T>(-1 * exponent))
        {
        }


        public static implicit operator CoLength<T>(T value)
        {
            CoLength<T> Q = new CoLength<T>();

            Q.Value = value;

            return Q;
        }
    }

}
