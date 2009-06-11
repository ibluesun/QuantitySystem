﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Quantities
{
    public class SpecificHeat<T> : DerivedQuantity<T>
    {

        public SpecificHeat()
            : base(1, new Energy<T>(), new Mass<T>(-1), new Temperature<T>(-1))
        {
        }


        public SpecificHeat(int exponent)
            : base(exponent, new Energy<T>(exponent), new Mass<T>(-1 * exponent), new Temperature<T>(-1 * exponent))
        {
        }


        public static implicit operator SpecificHeat<T>(T value)
        {
            SpecificHeat<T> Q = new SpecificHeat<T>();

            Q.Value = value;

            return Q;
        }

    }
}
