﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities
{
    /// <summary>
    /// Dynamic Viscosity.
    /// Pascal Second.
    /// </summary>
    public class Viscosity<T> : DerivedQuantity<T>
    {
        public Viscosity()
            : base(1, new Pressure<T>(), new Time<T>())
        {
        }

        public Viscosity(int exponent)
            : base (exponent, new Pressure<T>(exponent), new Time<T>(exponent))
        {
        }


        public static implicit operator Viscosity<T>(T value)
        {
            Viscosity<T> Q = new Viscosity<T>();

            Q.Value = value;

            return Q;
        }

    }
}
