using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Quantities.DimensionlessQuantities;
using QuantitySystem.Quantities;
using System;

namespace QuantitySystem.Units.Misc
{
    [Unit("rpm", typeof(AngularVelocity<>))]
    [ReferenceUnit(2* Math.PI, 60)]
    public sealed class RevolutionPerMinute : Unit
    {
    }
}
