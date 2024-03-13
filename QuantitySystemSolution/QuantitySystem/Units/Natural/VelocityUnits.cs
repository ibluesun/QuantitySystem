using QuantitySystem.Attributes;
using QuantitySystem.Quantities;

namespace QuantitySystem.Units.Natural
{
    [Unit("c0", typeof(Speed<>))]
    [ReferenceUnit(299792458)]
    public sealed class LightSpeed : Unit
    {

    }
}
