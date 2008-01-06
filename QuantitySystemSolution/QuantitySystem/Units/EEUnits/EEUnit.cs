using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.EEUnits
{
    public class EEUnit<TQuantity> : Unit<TQuantity>, IEEUnit where TQuantity : AnyQuantity, new()
    {
        public override UnitSystem UnitSystem { get { return UnitSystem.SIUnitSystem; } }
        public override bool IsSpecialName { get { return false; } }

    }


}
