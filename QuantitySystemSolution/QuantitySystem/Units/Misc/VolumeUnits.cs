using QuantitySystem.Quantities;
using QuantitySystem.Attributes;

namespace QuantitySystem.Units.Misc
{
    [Unit("cc", typeof(Volume<>))]
    [ReferenceUnit(1e-6)]
    public sealed class CubicCentimetre : Unit
    {
    }
}
