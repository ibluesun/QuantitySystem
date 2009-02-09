using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units
{
    public static partial  class UnitSystem
    {
        #region Helper Methods
        private static AnyQuantity<T> MakeQuantity<T>(Unit unit, T value)
        {


            //create the corresponding quantity
            AnyQuantity<T> qty = unit.GetThisUnitQuantity<T>();

            //assign the unit to the created quantity
            qty.Unit = unit;

            //assign the value to the quantity
            qty.Value = value;

            return qty;

        }
        #endregion

        public static AnyQuantity<double> None<TUnit>(double value) where TUnit : Unit, new()
        {
            return MakeQuantity<double>(new TUnit(), value);


        }
    }
}
