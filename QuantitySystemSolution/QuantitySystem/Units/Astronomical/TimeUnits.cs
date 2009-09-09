using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;


namespace QuantitySystem.Units.Astronomical
{
    [Unit("a", typeof(Time<>))]
    [ReferenceUnit(31557600)]
    public sealed class JulianYear : Unit
    {
    }
}
