using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.DimensionDescriptors;

namespace QuantitySystem.Quantities.BaseQuantities
{

    /// <summary>
    /// Scalar Quantity 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Length<T> : AnyQuantity<T>
    {

        public Length() : base(1) 
        {
        }

        public Length(float exponent)
            : base(exponent) 
        {
        }

        public Length(float exponent, QuantityType lengthType)
            : base(exponent)
        {
            QuantityType = lengthType;
        }


        public static implicit operator Length<T>(T value)
        {
            Length<T> Q = new Length<T>();

            Q.Value = value;

            return Q;
        }

    }

}
