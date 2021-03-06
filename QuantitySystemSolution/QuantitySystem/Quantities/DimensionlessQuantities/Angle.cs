﻿using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities.DimensionlessQuantities
{
    public class Angle<T> : DerivedQuantity<T>
    {

        public Angle()
            //: base(1, new Length<T>(1, LengthType.Regular), new Length<T>(-1, LengthType.Polar))
            :base(1, new Length<T>(), new PolarLength<T>(-1))
        {
        }

        public Angle(float exponent)
            //: base(exponent, new Length<T>(exponent, LengthType.Regular), new Length<T>(-1 * exponent, LengthType.Polar))
            : base(exponent, new Length<T>(), new PolarLength<T>(-1 * exponent))
        {
        }


        public static implicit operator Angle<T>(T value)
        {
            Angle<T> Q = new Angle<T>();

            Q.Value = value;

            return Q;
        }


    }
}
