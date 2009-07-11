using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.Quantities.DimensionlessQuantities;

namespace QuantitySystem.Quantities
{
    /// <summary>
    /// Power per unit Solid Angle
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RadiantIntensity<T> : DerivedQuantity<T>
    {
        public RadiantIntensity()
            : base(1, new Power<T>(), new SolidAngle<T>(-1))
        {
        }

        public RadiantIntensity(float exponent)
            : base(1, new Power<T>(exponent), new SolidAngle<T>(-1 * exponent))
        {
        }

        public static implicit operator RadiantIntensity<T>(T value)
        {
            RadiantIntensity<T> Q = new RadiantIntensity<T>();

            Q.Value = value;

            return Q;
        }

    }
}
