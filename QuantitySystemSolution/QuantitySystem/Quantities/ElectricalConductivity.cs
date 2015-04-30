using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities
{
    class ElectricalConductivity<T> : DerivedQuantity<T>
    {
        public ElectricalConductivity()
            : base(1, new ElectricConductance<T>(), new Length<T>(-1))
        {
        }

        public ElectricalConductivity(float exponent)
            : base(exponent, new ElectricConductance<T>(exponent), new Length<T>(-1 * exponent))
        {
        }


        public static implicit operator ElectricalConductivity<T>(T value)
        {
            ElectricalConductivity<T> Q = new ElectricalConductivity<T>();

            Q.Value = value;

            return Q;
        }

    }
}
