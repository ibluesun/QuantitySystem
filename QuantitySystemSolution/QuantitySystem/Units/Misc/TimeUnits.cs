using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.Misc
{

    [DefaultUnit("min", typeof(Time<>))]
    [ReferenceUnit(60)]
    public sealed class Minute : Unit
    {

    }


    [Unit("h", typeof(Time<>))]
    [ReferenceUnit(60, UnitType = typeof(Minute))]
    public sealed class Hour : Unit
    {

    }


    [Unit("d", typeof(Time<>))]
    [ReferenceUnit(24, UnitType = typeof(Hour))]
    public sealed class Day : Unit
    {

    }
}
