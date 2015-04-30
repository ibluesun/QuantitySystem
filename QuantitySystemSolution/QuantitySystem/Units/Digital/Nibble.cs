using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.Digital
{
    [Unit("nibble", typeof(Digital<>))]
    [ReferenceUnit(4, UnitType = typeof(Bit))]
    public sealed class Nibble : Unit
    {
    }
}
