using QuantitySystem.Quantities;
using QuantitySystem.Attributes;

namespace QuantitySystem.Units.Misc
{
    [Unit("cc", typeof(Volume<>))]
    [ReferenceUnit(10e-6)]
    public class CubicCentimetre : Unit
    {
    }
}
