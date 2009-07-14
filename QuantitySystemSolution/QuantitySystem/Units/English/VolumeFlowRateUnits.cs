using QuantitySystem.Attributes;
using QuantitySystem.Quantities;

namespace QuantitySystem.Units.English
{
    [DefaultUnit("cfm", typeof(VolumeFlowRate<>))]
    [ReferenceUnit(0.0004719474432)]
    public class CubicFeetPerMinute : Unit
    {

    }

    [Unit("gpm", typeof(VolumeFlowRate<>))]
    [ReferenceUnit(0.160526349049199, UnitType = typeof(CubicFeetPerMinute))]
    public class GallonPerMinute : Unit
    {
    }
}
