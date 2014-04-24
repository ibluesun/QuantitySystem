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
    public class PolarVolume<T> : DerivedQuantity<T>
    {
        public PolarVolume()
            : base(1, new PolarLength<T>(3))
        {
        }

        public PolarVolume(float exponent)
            : base(exponent, new PolarLength<T>(3 * exponent))
        {
        }


        public static implicit operator PolarVolume<T>(T value)
        {
            PolarVolume<T> Q = new PolarVolume<T>();

            Q.Value = value;

            return Q;
        }
    }
}
