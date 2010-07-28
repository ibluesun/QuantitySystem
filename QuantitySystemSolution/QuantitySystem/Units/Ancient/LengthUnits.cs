using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.Ancient
{
    [Unit("cubit", typeof(Length<>))]
    [ReferenceUnit(0.4572)]
    public sealed class Cubit : Unit
    {
    }
}
