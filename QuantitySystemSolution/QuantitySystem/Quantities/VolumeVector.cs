using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class VolumeVector<T> : DerivedQuantity<T>
    {
        public VolumeVector()
            : base(1, new LengthVector<T>(3))
        {
        }

        public VolumeVector(float exponent)
            : base(exponent, new LengthVector<T>(3 * exponent))
        {
        }


        public static implicit operator VolumeVector<T>(T value)
        {
            VolumeVector<T> Q = new VolumeVector<T>();

            Q.Value = value;

            return Q;
        }
    }
}
