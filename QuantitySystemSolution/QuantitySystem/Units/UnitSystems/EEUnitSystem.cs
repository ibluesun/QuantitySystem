using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.Units.EEUnits;
using QuantitySystem.Quantities.BaseQuantities;



namespace QuantitySystem.Units.UnitSystems
{
    public static class EEUnitSystem
    {

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static AnyQuantity Default<TUnit>(double value) where TUnit : IEEUnit, new()
        {
            return null;

        }
    }
}
